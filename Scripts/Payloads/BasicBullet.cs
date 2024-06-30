using Godot;
using System;
using System.Runtime.InteropServices;

public class BasicBullet : RigidBody2D, IPayload
{
    public int Team { get; private set; } = -1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var impulse = new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) );
        ApplyCentralImpulse( impulse * 400 );
    }
    
    public void SetTeam( int team )
    {
        Team = team;
    }
}
