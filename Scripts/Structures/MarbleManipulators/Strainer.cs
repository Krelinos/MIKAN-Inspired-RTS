using Godot;
using System;

public class Strainer : Node2D
{
    [Export] public float WaveRange { get; private set; } = 8;
    [Export] public float WaveFrequency { get; private set; } = 30;

    private Vector2 WaveInit;
    private float WaveTime;

    public override void _Ready()
    {
        WaveInit = Position;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        WaveTime += delta;
        Position = WaveInit + new Vector2( (float) Math.Sin( WaveTime * WaveFrequency/60.0 * Math.PI*2 ) * WaveRange, 0 );
    }
}
