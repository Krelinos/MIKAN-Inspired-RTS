using Godot;
using System;
using System.Collections.Generic;

public class BaseTurret : BaseStructure
{
    protected List<Receiver> Receivers;     // Receivers are expected to emit signals
                                            // when a marble collides with them.
    protected List<Spawner> Spawners;       // Spawners handle firing the cannon's payload.
    protected List<Indicator> Indicators;   // Indicators are optional and display cannon
                                            // status information to the user.

    public override void _Ready()
    {
        base._Ready();

        Receivers = new List<Receiver>();
        Spawners = new List<Spawner>();
        Indicators = new List<Indicator>();

        foreach ( Node n in GetChildren() )
            switch( n )
            {
                case Receiver r:
                    Receivers.Add( r );
                    r.Connect( "MarbleReceived", this, nameof(_OnReceiverMarbleReceived) );
                    break;
                case Spawner s:
                    Spawners.Add( s );
                    break;
                case Indicator i:
                    Indicators.Add( i );
                    break;
            }
        
    }
    
    protected void _OnReceiverMarbleReceived( int flags )
    {
        foreach ( Spawner s in Spawners )
        {
            s.AddAmmo(1);
            s.StartFiring();
        }
    }
}