using Godot;
using System;

/*
    A Projectile is an entity intended to be spawned from other entities that
    causes an effect whenever it hits something.

    This BaseProjectile lowers the HP of whatever it hits (assuming whatever it
    hit implemented IHealth) then disappears.
*/
public class BaseProjectile : BaseRTSEntity
{
    [Export]
    public int InitialVelocity = 400;   // Velocity when spawned from a Spawner.
    [Export]
    public int CollisionDamage = 1;
    [Export]
    public int Lifetime = 5;
    
    protected BaseRTSEntity EntitySpawnedFrom;  // The entity that created this Projectile. ( NOT the entity's Spawner! ) 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        AddToGroup( RTSManager.EntityType.Projectile.ToString() );
        
		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Projectile, Team );

        Connect( "body_entered", this, nameof(OnBodyEntered) );

        var impulse = new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) );
        ApplyCentralImpulse( impulse * InitialVelocity );

        if ( Lifetime > 0)
            GetTree().CreateTimer( Lifetime, false ).Connect( "timeout", this, nameof(Die) );
    }
    
    protected virtual void DoCollisionEffectOn( BaseRTSEntity entity )
    {
        entity.AlterHealth(-CollisionDamage);
        Die();
    }

    protected virtual void OnBodyEntered( Node node )
    {
        var entity = node as BaseRTSEntity;
        if ( entity != null && entity.GetTeam() != Team )
            DoCollisionEffectOn( entity );
    }
}
