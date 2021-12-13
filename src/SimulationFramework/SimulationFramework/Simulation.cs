﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationFramework;

/// <summary>
/// Base class for simulations.
/// </summary>
public abstract class Simulation : IDisposable
{
    /// <summary>
    /// Gets the presently running simulation.
    /// </summary>
    public static Simulation Current { get; private set; }

    private readonly List<ISimulationComponent> components = new();
    private ISimulationEnvironment environment;

    public event Action Initialized;
    public event Action BeforeRendering;
    public event Action AfterRendering;
    public event Action Uninitialize;

    /// <summary>
    /// Called when the simulation should initialize.
    /// </summary>
    /// <param name="config"></param>
    public abstract void OnInitialize(AppConfig config);
    
    /// <summary>
    /// Called when the simulation should render.
    /// </summary>
    /// <param name="canvas"></param>
    public abstract void OnRender(ICanvas canvas);
    
    /// <summary>
    /// Called when the simulation should uninitialize.
    /// </summary>
    public abstract void OnUnitialize();

    /// <summary>
    /// Creates a new instance of the <see cref="Simulation"/> class.
    /// </summary>
    public Simulation()
    {
    }

    public T GetComponent<T>() where T : ISimulationComponent
    {
        return (T)components.Single(c => c.GetType().GetInterfaces().Contains(typeof(T)));
    }

    /// <summary>
    /// Destroys the simulation.
    /// </summary>
    public void Dispose()
    {
        try
        {
            environment.Dispose();
        }
        finally
        {
            GC.SuppressFinalize(this);
            if (Current == this)
                Current = null;
        }
    }
    
    /// <summary>
    /// Sets the simulation to use the provided enviroment. This may only be called once on each simulation.
    /// </summary>
    /// <param name="environment"></param>
    /// <remarks>
    /// To use a SimulationFramework-included environment, call the method for that environment on the <see cref="AppConfig"/> instance provided in <see cref="OnInitialize(AppConfig)"/>.
    /// </remarks>
    public void SetEnvironment(ISimulationEnvironment environment)
    {
        if (this.environment is not null)
            throw new InvalidOperationException("This simulation already has an environment!");

        this.environment = environment;

        foreach (var component in this.environment.CreateSupportedComponents())
        {
            component.Apply(this);
            this.components.Add(component);
        }
    }

    // starts a simulation. This is only to be called from `Run()`.
    private void Start()
    {
        this.OnInitialize(new AppConfig(this));

        if (environment is null)
            throw new Exception("Simulation must select an enviroment");

        while (!environment.ShouldExit())
        {
            environment.ProcessEvents();

            using var canvas = Graphics.GetFrameCanvas();

            using (canvas.Push())
            {
                this.OnRender(canvas);
            }

            canvas.Flush();

            environment.EndFrame();
        }

        this.OnUnitialize();
    }

    /// <summary>
    /// Starts the provided simulation.
    /// </summary>
    /// <param name="simulation">The simulation to start.</param>
    public static void Run(Simulation simulation)
    {
        Current = simulation;
        simulation.Start();
    }

    /// <summary>
    /// Starts a simulation using delegates in place of overridden methods.
    /// </summary>
    /// <param name="init">The delegate to call when simulation should initialize.</param>
    /// <param name="draw">The delegate to call when simulation should draw.</param>
    /// <param name="uninit">The delegate to call when simulation should uninitialize.</param>
    public static void Run(Action<AppConfig> init, Action<ICanvas> draw, Action uninit = null)
    {
        using var simulation = new DelegateSimulation(init, draw, uninit);
        Run(simulation);
    }

    // enables use of delegates as an alterative to overrides.
    private sealed class DelegateSimulation : Simulation
    {
        private readonly Action<AppConfig> init = null;
        private readonly Action<ICanvas> draw = null; 
        private readonly Action uninit = null;

        public DelegateSimulation(Action<AppConfig> init, Action<ICanvas> draw, Action uninit)
        {
            this.init = init;
            this.draw = draw;
            this.uninit = uninit;
        }

        public override void OnRender(ICanvas canvas)
        {
            this.draw?.Invoke(canvas);
        }

        public override void OnInitialize(AppConfig config)
        {
            this.init?.Invoke(config);
        }

        public override void OnUnitialize()
        {
            this.uninit?.Invoke();
        }
    }
}