﻿using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using SimulationFramework.Messaging;

Simulation.Start<MySimulation>();

class MySimulation : Simulation
{
    bool wantToggleResize;
    public override void OnInitialize()
    {
        SimulationHost.Current.Dispatcher.Subscribe<BeforeRenderMessage>(m =>
        {
            if (wantToggleResize) 
            {
                if (Window.IsFullscreen)
                {
                    Window.ExitFullscreen();
                }
                else
                {
                    Window.EnterFullscreen(null);
                }
                wantToggleResize = false;
            }
        });
    }

    public override void OnRender(ICanvas canvas)
    {
        canvas.Clear(Color.Red);

        if (Keyboard.IsKeyPressed(Key.Space))
        {
            wantToggleResize = true;
        }
    }
}