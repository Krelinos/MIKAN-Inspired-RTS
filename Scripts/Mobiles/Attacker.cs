using Godot;
using System;
/*
    An Attacker valiantly/stupidly runs head first toward the nearest enemy Mobile 
    or Structure to ram it while firing with any Spawners equipped until it dies.
*/
public class Attacker : BaseMobile
{
    public override void _Ready()
    {
        base._Ready();

        AddToGroup( RTSManager.EntityType.Mobiles.ToString() );

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

    protected void Pursue()
    {
        LookAt( FindNearestEnemy().GlobalPosition );
    }

    protected Node2D FindNearestEnemy()
    {
        float nearestDistance = Int32.MaxValue;
        Node2D nearest = null;
        foreach ( Node n in GetTree().GetNodesInGroup(RTSManager.EntityType.Mobiles.ToString()) )
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