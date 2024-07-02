using Godot;
using System;
/*
    An Attacker valiantly/stupidly runs head first toward the nearest enemy Mobile 
    or Structure to ram it while firing with any Spawners equipped until it dies.
*/
public class Attacker : BaseMobile
{
    public Vector2 AimPosition { get; private set; }
    public Vector2 AimPrediction { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        AimPosition = GlobalPosition + new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * 1000;
        AimPrediction = Vector2.Zero;

        foreach ( Node n in GetChildren() )
            (n as Spawner)?.StartFiring();

        Timer timer = new Timer
        {
            WaitTime = 2,
            OneShot = false
        };
        AddChild(timer);
        timer.Start();
        timer.Connect( "timeout", this, nameof(Pursue) );
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        AimPosition += AimPrediction * delta;
        var angleToTarget = GetAngleTo( AimPosition );
        ApplyTorqueImpulse( Math.Min( Math.Max(angleToTarget, -1), 1 ) * delta * RotationalSpeed );
    }

    protected void Pursue()
    {
        var target = FindNearestEnemy( 0b0110 );
        if ( target != null )
        {
            AimPosition = target.GlobalPosition;
            AimPrediction = target.LinearVelocity;
        }
    }

    
}