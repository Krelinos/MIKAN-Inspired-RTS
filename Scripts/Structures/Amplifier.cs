using Godot;
using System;

/// <summary>
/// An Amplifier is a Structure that provides a multiplicative bonus
/// to the Marble production of the team that owns it. The bonus stacks
/// additively, not cumulatively ( two Amplifiers that provide +20% will
/// result in +40%, not +44% ).
/// 
/// Amplifiers are initially neutral/unaffilliated, but can be captured by
/// destroying its turret then having a Mobile collide with it.
/// </summary>
public class Amplifier : BaseStructure
{
    [Export]
    private PackedScene InitialTurret;

    private CollisionShape2D CollisionShape2D;
    private PinJoint2D PinJoint2D;
    public BaseMobile Turret { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        PinJoint2D = GetNode<PinJoint2D>("PinJoint2D");

        Turret = InitialTurret.Instance() as BaseMobile;
        if ( Turret == null )
            GD.PushError($"Amplifier '{this.Name}' has a non-BaseMobile initial turret!");
        else
        {
            AddChild( Turret );
            AttachMobileAsTurret( Turret );
        }
        
        Connect( "body_entered", this, nameof(OnBodyEntered) );
    }

    /// <summary>
    /// Absorb a BaseMobile, using it as a turret to defend this Amplifier.
    /// Also sets the Amplifier's team to match the BaseMobile's team.
    /// Damage done to the BaseMobile is instead received by the Amplifier.
    /// </summary>
    /// <param name="mobile">The mobile to be attached.</param>
    protected void AttachMobileAsTurret( BaseMobile mobile )
    {
        Team = mobile.GetTeam();
        GD.Print(Team);
        RTSManager.ApplyPaletteTo( this, Team, true );
		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Structure, Team );

        mobile.GlobalPosition = GlobalPosition;
        mobile.LinearVelocity = Vector2.Zero;
        mobile.Connect( "Died", this, nameof(OnTurretDestroyed) );
    
        PinJoint2D.NodeB = PinJoint2D.GetPathTo( mobile );
        CollisionShape2D.SetDeferred("disabled", true);
    }

    protected void OnBodyEntered( Node node )
    {
        if (node is BaseMobile entity)
            AttachMobileAsTurret(entity);
    }

    protected void OnTurretDestroyed()
    {
        Team = 0;
        RTSManager.ApplyPaletteTo( this, Team, true );
		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Structure, Team );

        CollisionShape2D.SetDeferred("disabled", false);
    }
}
