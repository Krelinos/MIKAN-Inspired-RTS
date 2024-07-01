using Godot;
using System;

/*
    A Mobile is an entity that acts independently from the team's static structures.

    This BaseMobile charges valiantly/stupidly toward the nearest enemy Mobile or Structure
    while firing any Spawners equipped until it dies or collides with something.
    
    Also, BaseMobile inherits from BaseProjectile, so Mobiles are essentially more
    complex Projectiles.
*/
public class BaseMobile : BaseProjectile, IHealth
{
    [Signal]
    public delegate void Died();

    [Export]
    public int MaxHP { get; protected set; } = 5;
    [Export]
    public int Speed { get; protected set; } = 50;

    protected int HP;

    public override void _Ready()
    {
        base._Ready();

        HP = MaxHP;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        ApplyCentralImpulse( new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * Speed * delta );
    }

    protected override void OnBodyEntered( Node node )
    {
        if ( HP <= 0 )
        {
            Die();
            EmitSignal("Died");
        }
    }

    protected void Die()
    {
        QueueFree();
    }

    public void SetHealth( int amt ) { HP = amt; }
    public void AlterHealth( int amt ) { HP += amt; }
    public int GetHealth() { return HP; }
}
