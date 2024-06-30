using Godot;
using System;

public class BaseCannon : Node2D
{
    [Export]
    protected int WaveRange = 15;       // Maximum angle in degrees that the cannon
                                        // turns during its sin wave.
    [Export]
    protected int WaveFrequency = 5;    // Number of sin wave cycles per minute.

    protected Receiver Receiver;        // Receiver is expected to emit signals
                                        // when a marble collides with it.
    protected Spawner Spawner;          // Spawner handles firing the cannon's payload.
    protected Indicator Indicator;      // Indicator is optional, and displays cannon
                                        // status information to the user.
    
    protected double WaveAngle = 0;
    protected float WaveTime = 0;
    protected float WaveInit;

    public override void _Ready()
    {
        Receiver = GetNode<Receiver>("Receiver");
        Spawner = GetNode<Spawner>("Spawner");
        Indicator = GetNode<Indicator>("Indicator");

        WaveInit = RotationDegrees;

        Receiver.Connect( "MarbleReceived", this, nameof(_OnReceiverMarbleReceived) );

        GetNode<ColorPalettes>("/root/ColorPalettes").ApplyPaletteTo( this, true );
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        WaveTime += delta;
        WaveAngle = WaveInit + Math.Sin( WaveTime * WaveFrequency/60 * Math.PI*2 ) * WaveRange;
        RotationDegrees = (float)WaveAngle;
    }

    private void _OnReceiverMarbleReceived( int flags )
    {
        GD.Print("Nyaa~");
        Spawner.StartFiring();
    }
}