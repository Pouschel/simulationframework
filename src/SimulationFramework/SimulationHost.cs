﻿using SimulationFramework.Components;
using SimulationFramework.Drawing;
using SimulationFramework.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;
public class SimulationHost
{
    public static SimulationHost? Current { get; private set; }

    private Simulation? simulation;
    public MessageDispatcher Dispatcher { get; set; }
    private bool initialized = false;

    private readonly List<ISimulationComponent> components = new();
    private readonly HashSet<Type> requiredComponents = new();

    private static List<Func<ISimulationPlatform?>> platformFactories = new();

    public SimulationHost()
    {
        Dispatcher = new();
    }

    static SimulationHost()
    {
        // we try to register some known platforms so that the user doesn't have to call Register()

        (string assembly, string name)[] knownPlatforms = new[]
        {
            ("SimulationFramework.Desktop", "DesktopPlatform"),
        };


        foreach (var (platformAssembly, platformName) in knownPlatforms)
        {
            try
            {
                Assembly assembly = Assembly.Load(platformAssembly);
                Type? type = assembly.GetType($"{platformAssembly}.{platformName}");
                MethodInfo? register = type?.GetMethod("Register", BindingFlags.Static | BindingFlags.Public);
                register?.Invoke(null, Array.Empty<Type>());
            }
            catch
            {

            }
        }
    }

    public static SimulationHost GetCurrent()
    {
        return Current ?? throw new Exception("No Simulation Host is Currently Active!");
    }

    /// <summary>
    /// </summary>
    /// <param name="factory"></param>
    public static void RegisterPlatform(Func<ISimulationPlatform?> factory)
    {
        platformFactories.Add(factory);
    }

    private static bool TryCreatePlatform([NotNullWhen(true)] out ISimulationPlatform? platform)
    {
        platform = null;

        // we take the first one that doesn't throw or return null.
        foreach (var factory in platformFactories)
        {
            try
            {
                platform = factory();
            }
            catch 
            {
                continue;
            }

            if (platform is not null)
            {
                return true;
            }
        }

        return false;
    }

    public void Initialize(ISimulationPlatform? platform)
    {
        if (initialized)
        {
            throw Exceptions.HostAlreadyInitialized();
        }

        if (Current is not null)
        {
            throw Exceptions.SimulationRunning();
        }

        if (platform is null && !TryCreatePlatform(out platform))
        {
            throw Exceptions.NoPlatform();
        }

        initialized = true;
        Current = this;

        RegisterComponent(platform);
    }

    public void Start<T>() where T : Simulation, new()
    {
        if (!initialized)
            Initialize(null);

        Start(new T());
    }

    /// <summary>
    /// Starts a simulation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if a simulation is already running (<see cref="Current"/> is non-null).</exception>
    public void Start(Simulation simulation)
    {
        if (!initialized)
            Initialize(null);
        
        this.simulation = simulation;
        simulation.OnInitialize();

        var appController = Application.GetComponent<ISimulationController>();
        
        Dispatcher.ImmediateDispatch<InitializeMessage>(new());
        appController.Start(RunFrame);
        Dispatcher.ImmediateDispatch<UninitializeMessage>(new());
    }

    private void RunFrame()
    {
        Dispatcher.ImmediateDispatch<BeforeRenderMessage>(new());

        var canvas = Graphics.GetOutputCanvas();
        Dispatcher.ImmediateDispatch<RenderMessage>(new(canvas));
        simulation?.OnRender(canvas);
        canvas.Flush();

        Dispatcher.ImmediateDispatch<AfterRenderMessage>(new());
    }

    public void Stop()
    {
        this.simulation = null;
        Current = null;
    }

    public void RegisterComponent(ISimulationComponent component)
    {
        component.Initialize(this.Dispatcher);
        components.Add(component);
    }

    public T? GetComponent<T>() where T : class, ISimulationComponent
    {
        return components.SingleOrDefault(c => c is T) as T;
    }
}