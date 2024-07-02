using Godot;
using System;

/*
    This is the base class of all entities in this RTS.
*/
public class BaseRTSEntity : RigidBody2D, ITeam
{
    [Export]
    protected int Team;

    protected RTSManager RTSManager { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        RTSManager = GetNode<RTSManager>("/root/RTSManager");

        RTSManager.ApplyPaletteTo( this, Team, true );
        AddToGroup( ((RTSManager.TeamGroupName)Team).ToString() );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bits">The bits</param>
    /// <returns>A BaseRTSEntity matching the specified filters, or null if one cannot be found.</returns>
    protected BaseRTSEntity FindNearestEnemy( uint bits )
    {
        float nearestDistance = Int32.MaxValue;
        Node2D nearest = null;
        foreach ( Node n in GetTree().GetNodesInGroup(RTSManager.EntityType.Mobile.ToString()) )
            if ( (n as ITeam)?.GetTeam() != Team )
                {
                    Vector2 offset = (n as Node2D).GlobalPosition - GlobalPosition;
                    float distance = offset.Length();

                    if ( distance < nearestDistance )
                    {
                        nearestDistance = distance;
                        nearest = n as Node2D;
                    }
                }

        return nearest;
    }

    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }
}