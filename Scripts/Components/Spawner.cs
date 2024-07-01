using System;
using Godot;

/*
    A Spawner handles firing the payload of a cannon. Payloads are PackedScenes
    declared in the exported variable Payload. It also signals out when it
    starts and stops firing.
*/
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
    public PackedScene Payload { get; private set; }

    [Export]
    public int SPM { get; private set; } = 60;      // Shots per minute, assuming this Spawner
                                                    // will call Fire() multiple times per
                                                    // BeginFiring() call.
    [Export]
    public int AmmoCost { get; private set; } = 1;  // Amount of ammo deducted per payload spawn.
                                                    // Spawner will emit StoppedFiring when this
                                                    // cost cannot be met.

    public int Ammo { get; private set; } = 0;
    public double Cooldown { get; private set; } = 0;
    public bool Firing { get; private set; } = false;

    public override void _Ready()
    {
        base._Ready();

        // Check if Payload is null
        if ( Payload == null )
            GD.PushError( String.Format("Spawner of node '{0}' does not have a payload.", this.GetParent().Name) );

        // Spawner sets the payload's team based on the team of its parent. Check if the parent implements it.
        var ParentCanTeam = GetParent() as ITeam;
        if ( ParentCanTeam == null )
            GD.PushError( String.Format("Node '{0}' does not implement ICanTeam.", this.GetParent().Name) );

        // And check if the payload implements the same interface.
        var PayloadCanTeam = GetParent() as ITeam;
        if ( PayloadCanTeam == null )
            GD.PushError( String.Format("Payload '{0}' does not implement ICanTeam.", Payload.ResourceName) );

        // Only Node2D inherited nodes can be payloads.
        var PayloadAs2D = Payload.Instance() as Node2D;
        if ( PayloadAs2D == null )
            GD.PushError( String.Format("Spawner of '{0}' does not have payload that inherits from Node2D.", this.GetParent().Name) );
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
            var payload = Payload.Instance() as Node2D;
            payload.GlobalPosition = GlobalPosition;
            payload.GlobalRotation = GlobalRotation;
            (payload as ITeam).SetTeam( (GetParent() as ITeam).GetTeam() );
            GetNode("/root").AddChild( payload );

            Ammo -= AmmoCost;
            Cooldown = 1 / ( SPM/60.0 );
            EmitSignal( nameof(Fired) );
            return true;
        }
        else
            return false;
    }
}