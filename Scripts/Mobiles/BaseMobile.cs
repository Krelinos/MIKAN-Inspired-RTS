using Godot;
using System;

/*
	A Mobile is an entity that acts independently from the team's static structures
	and fill roles that Structures cannot complete (such as capturing a point of
	interest.) They are intended to last longer than Projectiles.

	Also, BaseMobile inherits from BaseProjectile, so Mobiles are essentially more
	complex Projectiles.
*/
public class BaseMobile : BaseProjectile, IHealth
{
	[Signal]
	public delegate void HealthChanged();
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
		GetNode<RTSManager>("/root/RTSManager").AssignLayersAndMasks( this, RTSManager.EntityType.Mobile, Team );
		CollisionMask += (uint)0b0100 << Team*4;   // Also collide with friendly Mobiles.

		Connect( nameof(HealthChanged), this, nameof(OnHealthChanged) );
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		ApplyCentralImpulse( new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * Speed * delta );
	}

	protected override void OnBodyEntered( Node node )
	{
		DoCollisionEffectOn( node );
	}

	protected virtual void OnHealthChanged( int oldHealth )
	{
		if ( HP <= 0 )
			Die();
	}

	protected void Die()
	{
        EmitSignal( nameof(Died) );
		QueueFree();
	}

	public void SetHealth( int amt ) { EmitSignal(nameof(HealthChanged), HP); HP = amt; }
	public void AlterHealth( int amt ) { EmitSignal(nameof(HealthChanged), HP); HP += amt; }
	public int GetHealth() { return HP; }
}
