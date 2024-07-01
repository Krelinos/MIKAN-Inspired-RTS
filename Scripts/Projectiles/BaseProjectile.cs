using Godot;
using System;

/*
    A Projectile is an entity with collision that causes an effect
    whenever it hits something.

    This BaseProjectile lowers the HP of whatever it hits (assuming
    whatever it hit implemented IHealth) then disappears.
*/
public class BaseProjectile : RigidBody2D, ITeam
{
    [Export]
    public int InitialVelocity = 400;   // Velocity when spawned from a Spawner.
    [Export]
    public int CollisionDamage = 1;

    protected int Team = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<RTSManager>("/root/RTSManager").ApplyPaletteTo( this, true );

        Connect( "body_entered", this, nameof(OnBodyEntered) );

        var impulse = new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) );
        ApplyCentralImpulse( impulse * InitialVelocity );
    }
    
    protected virtual void DoEffectOn( Node node )
    {
        if ( (node as IHealth) != null )
            (node as IHealth).AlterHealth(-CollisionDamage);
    }

    protected virtual void OnBodyEntered( Node node )
    {
        GD.Print(node.Name);
        DoEffectOn( node );
        QueueFree();
    }

    public int GetTeam() { return Team; }
    public void SetTeam( int team ) { Team = team; }
}
