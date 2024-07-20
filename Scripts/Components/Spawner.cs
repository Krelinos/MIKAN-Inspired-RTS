using System;
using Godot;

/// <summary>
///    A Spawner handles firing the payload of a cannon. Payloads are PackedScenes
///    declared in the exported variable Payload.
/// </summary>
public class Spawner : Node2D
{
    [Signal]
    public delegate void StartedFiring();   // Attack sequence begun
    [Signal]
    public delegate void StoppedFiring();   // Attack sequence ended
    [Signal]
    public delegate void Fired();           // A payload was spawned

    // Payload is the Node2D projectile spawned by this Spawner. Note that this
    // could be anything as long as it is a PackedScene; simple bullets to 
    // complex contraptions that even have their own cannons are all valid.
    [Export]
    public PackedScene Payload { get; protected set; }

    [Export]
    public int SPM { get; protected set; } = 60;    // Shots per minute, assuming this Spawner
                                                    // will call Fire() multiple times per
                                                    // BeginFiring() call.
    [Export]
    public int AmmoCost { get; protected set; } = 1;  // Amount of ammo deducted per payload spawn.
                                                    // Spawner will emit StoppedFiring when this
                                                    // cost cannot be met.
    
    [Export] public int MuzzleVelocity { get; protected set; } = 200;
    [Export] public bool ImmediatelyFire { get; protected set; } = false;

    public BaseRTSEntity Entity;

    public int Ammo { get; protected set; } = 0;
    public double Cooldown { get; protected set; } = 0;
    public bool Firing { get; protected set; } = false;

    public override void _Ready()
    {
        base._Ready();

        Entity = GetNode<RTSManager>("/root/RTSManager").FindRTSEntityOf( this );

        if ( ImmediatelyFire ) StartFiring();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if ( Cooldown > 0 )
            Cooldown -= delta;
        
        if ( Firing && Cooldown <= 0 )
        {
            Firing = Fire();
            if ( !Firing )      // Ammo has just ran out.
                EmitSignal( nameof(StoppedFiring) );
        }
    }

    public void AddAmmo( int amt ) { Ammo += amt; }

    public void StartFiring()
    {
        if ( Ammo >= AmmoCost )
        {
            Firing = true;
            EmitSignal( nameof(StartedFiring) );
        }
    }

    private bool Fire()
    {
        if ( Ammo >= AmmoCost )
        {
            var payload = Payload.Instance() as BaseRTSEntity;
            payload.GlobalPosition = GlobalPosition;
            payload.GlobalRotation = GlobalRotation;
            payload.ApplyCentralImpulse( new Vector2( (float)Math.Cos(GlobalRotation), (float)Math.Sin(GlobalRotation) ) * MuzzleVelocity );
            payload.SetTeam( Entity.Team );
            GetNode("/root").AddChild( payload );

            Ammo -= AmmoCost;
            Cooldown += 1 / ( SPM/60.0 );
            EmitSignal( nameof(Fired) );

            return true;
        }
        else
            return false;
    }
}