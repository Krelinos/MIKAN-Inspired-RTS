using Godot;
using System;
using System.Runtime.InteropServices;

public class BasicBullet : RigidBody2D, ICanTeam
{
    private int Team = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<ColorPalettes>("/root/ColorPalettes").ApplyPaletteTo( this, true );

        var impulse = new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) );
        ApplyCentralImpulse( impulse * 400 );
    }
    
    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }
}
