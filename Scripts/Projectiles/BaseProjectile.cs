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
    
    protected BaseRTSEntity EntitySpawnedFrom;  // The entity that created this Projectile. ( NOT the entity's Spawner! ) 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        GetNode<RTSManager>("/root/RTSManager").AssignLayersAndMasks( this, RTSManager.EntityType.Projectile, Team );

        Connect( "body_entered", this, nameof(OnBodyEntered) );

        var impulse = new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) );
        ApplyCentralImpulse( impulse * InitialVelocity );
    }
    
    protected virtual void DoCollisionEffectOn( Node node )
    {
        if ( (node as IHealth) != null )
            (node as IHealth).AlterHealth(-CollisionDamage);
    }

    protected virtual void OnBodyEntered( Node node )
    {
        DoCollisionEffectOn( node );
        QueueFree();
    }
}
