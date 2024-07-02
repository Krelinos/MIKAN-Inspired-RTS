using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

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
    /// <returns>A BaseRTSEntity matching the specified filters, or null if one cannot be found.</returns>
    protected BaseRTSEntity FindNearestEnemy( byte targetTypes )
    {
        var validTargets = new Godot.Collections.Array();

        if ( RTSManager.IsBitSet(targetTypes, (int)RTSManager.EntityType.Marble) )  // Target Marbles
            foreach ( BaseRTSEntity e in GetTree().GetNodesInGroup(RTSManager.EntityType.Marble.ToString()) )
                if ( e.Team != Team )
                    validTargets.Add( e );
        
        if ( RTSManager.IsBitSet(targetTypes, (int)RTSManager.EntityType.Structure) )  // Target Structures
            foreach ( BaseRTSEntity e in GetTree().GetNodesInGroup(RTSManager.EntityType.Structure.ToString()) )
                if ( e.Team != Team )
                    validTargets.Add( e );

        if ( RTSManager.IsBitSet(targetTypes, (int)RTSManager.EntityType.Mobile) )  // Target Mobiles
            foreach ( BaseRTSEntity e in GetTree().GetNodesInGroup(RTSManager.EntityType.Mobile.ToString()) )
                if ( e.Team != Team )
                    validTargets.Add( e );
        
        if ( RTSManager.IsBitSet(targetTypes, (int)RTSManager.EntityType.Projectile) )  // Target Projectiles
            foreach ( BaseRTSEntity e in GetTree().GetNodesInGroup(RTSManager.EntityType.Projectile.ToString()) )
                if ( e.Team != Team )
                    validTargets.Add( e );


        float nearestDistance = Int32.MaxValue;
        BaseRTSEntity nearest = null;
        foreach ( BaseRTSEntity e in validTargets )
        {
            Vector2 offset = e.GlobalPosition - GlobalPosition;
            float distance = offset.Length();

            if ( distance < nearestDistance )
            {
                nearestDistance = distance;
                nearest = e;
            }
        }

        return nearest;
    }

    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }
}