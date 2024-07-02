using Godot;
using System;

/// <summary>
/// SinWaveTurrets rotate in a sin wave tied to the total time it has been alive.
/// </summary>
public class SinWaveTurret : BaseTurret
{
    [Export]
    protected int WaveRange = 15;       // Maximum angle in degrees that the cannon
                                        // turns during its sin wave.
    [Export]
    protected int WaveFrequency = 5;    // Number of sin wave cycles per minute.
    [Export]
    public bool WavePrecise = false;    // A SinWaveTurret that has WavePrecise on 
                                        // will slow down at the middle and speed up
                                        // at the edges.

    protected double WaveAngle = 0;
    protected float WaveTime = 0;
    protected float WaveInit;           // Initial rotation


    public override void _Ready()
    {
        base._Ready();
        WaveInit = RotationDegrees;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        WaveTime += delta;

        if ( WavePrecise )
        {
            var x = WaveTime * WaveFrequency/60.0;
            x = 2 * Math.Abs( 2 * (x - Math.Floor(x+0.5) ) ) - 1;   // https://en.wikipedia.org/wiki/Triangle_wave
            x = Math.Asin( x );
            WaveAngle = WaveRange * x + WaveInit;
        }
        else
            WaveAngle = WaveRange * Math.Sin( WaveTime * WaveFrequency/60.0 * Math.PI*2 ) + WaveInit;

        RotationDegrees = (float)WaveAngle;
    }
}