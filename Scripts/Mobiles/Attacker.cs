using Godot;
using System;
/*
    An Attacker valiantly/stupidly runs head first toward the nearest enemy Mobile 
    or Structure to ram it while firing with any Spawners equipped until it dies.
*/
public class Attacker : BaseMobile
{
    public Node2D Target { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        AddToGroup( RTSManager.EntityType.Mobile.ToString() );

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

        if ( Target != null )
        {
            var angleToTarget = GetAngleTo( Target.GlobalPosition );
            ApplyTorqueImpulse( Math.Sign(angleToTarget) * delta * 100 );
        }

    }

    protected void TargetDied( bool wasKiller )
    {
        Target = null;
    }

    protected void Pursue()
    {
        var oldTarget = Target;
        Target = FindNearestEnemy();
        if ( Target != null && oldTarget != Target )
            Target.Connect( nameof(Died), this, nameof(TargetDied) );
    }

    protected Node2D FindNearestEnemy()
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
}