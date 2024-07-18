using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the base class of all entities in this RTS.
/// </summary>
public class BaseRTSEntity : RigidBody2D
{
	[Signal]
	public delegate void HealthChanged();
	[Signal]
	public delegate void Died();

    [Export]
    public int Team { get; protected set; }
	[Export]
	public int MaxHP { get; protected set; } = 5;

	protected int HP;

    protected RTSManager RTSManager { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        HP = MaxHP;

        RTSManager = GetNode<RTSManager>("/root/RTSManager");

        RTSManager.ApplyPaletteTo( this, Team, true );
        AddToGroup( ((RTSManager.TeamGroupName)Team).ToString() );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetTypes"></param>
    /// <param name="maxDist"></param>
    /// <returns>A BaseRTSEntity matching the specified filters, or null if one cannot be found.</returns>
    protected BaseRTSEntity FindNearestEnemy( byte targetTypes, int maxDist = Int32.MaxValue )
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


        float nearestDistance = maxDist;
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

    protected virtual void Die()
	{
        EmitSignal( nameof(Died) );
		QueueFree();
	}

    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }

	public virtual void SetHealth( int amt ) {
		HP = amt;
		if ( HP <= 0 )
			Die();
	}
	public virtual void AlterHealth( int amt ) {
		HP += amt;
		if ( HP <= 0 )
			Die();
	}
	public int GetHealth() { return HP; }
}