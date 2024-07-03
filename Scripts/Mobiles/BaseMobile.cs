using Godot;
using System;

/*
	A Mobile is an entity that acts independently from the team's static structures
	and fill roles that Structures cannot complete (such as capturing a point of
	interest.) They are intended to last longer than Projectiles.

	Also, BaseMobile inherits from BaseProjectile, so Mobiles are essentially more
	complex Projectiles.
*/
public class BaseMobile : BaseProjectile
{
	[Export]
	public int Speed { get; protected set; } = 50;
	[Export]
	public int RotationalSpeed { get; private set; } = 200;

	public override void _Ready()
	{
		base._Ready();
		
		RemoveFromGroup( RTSManager.EntityType.Projectile.ToString() );
        AddToGroup( RTSManager.EntityType.Mobile.ToString() );

		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Mobile, Team );
		CollisionMask += (uint)0b0100 << Team*4;   // Also collide with friendly Mobiles.
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		ApplyCentralImpulse( new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * Speed * delta );
	}

	protected override void DoCollisionEffectOn( BaseRTSEntity entity )
	{
		entity.AlterHealth( -CollisionDamage );
	}
}
