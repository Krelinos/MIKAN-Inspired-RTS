using System;
using Godot;

/*
    A Receiver is an Area2D of a cannon that detects when a marble has
    reached the cannon. It signals out when this happens, along with
    the flags the marble has.
*/
public class Receiver : Area2D
{
    [Signal]
    public delegate void MarbleReceived( int flags );

    public override void _Ready()
    {
        base._Ready();

        
    }

    public void _OnReceiverBodyEntered( Node body )
    {
        var marble = body as Marble;
        if ( marble == null )   // body is not a marble. Ignore it.
            return;
        
        marble.QueueFree();

        EmitSignal( nameof(MarbleReceived), 0b0 );
    }
}