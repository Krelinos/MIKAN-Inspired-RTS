using Godot;
using System;

/*
    This is the base class of all entities in this RTS that houses
    all of their common implementations.
*/
public class BaseEntity : RigidBody2D, ITeam
{
    [Export]
    protected int Team;

    public override void _Ready()
    {
        base._Ready();
        
        GetNode<RTSManager>("/root/RTSManager").ApplyPaletteTo( this, true );
    }

    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }
}