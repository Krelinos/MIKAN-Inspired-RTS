using Godot;
using System;
using System.Threading;

/*
    A Mobile is an entity that acts independently from the team's static structures.

    This BaseMobile charges valiantly/stupidly toward the nearest enemy Mobile or
    Structure while firing any weapons equipped until it dies or collides with something.
    BaseMobile inherits from BaseProjectile, so Mobiles are essentially more complex Projectiles.
*/
public class BaseMobile : BaseProjectile
{
    [Export]
    public int Speed { get; private set; } = 50;

    public override void _Ready()
    {
        base._Ready();

        Initialize();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        ApplyCentralImpulse( new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * Speed * delta );
    }

    // Separated this from _Ready() so that inheriters can safely do base._Ready()
    // and override initial actions instead of being forced to fire immediately.
    protected void Initialize()
    {
        foreach ( Node n in GetChildren() )
            (n as Spawner)?.StartFiring();
    }
}
