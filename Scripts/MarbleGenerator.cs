using Godot;
using System;

public class MarbleGenerator : BaseRTSEntity
{
	[Export] public PackedScene Marble { get; private set; }

	// With no amplifiers, one Marble spawns every SpawnDelayBase seconds.
	[Export] public float SpawnDelayBase { get; private set; } = 3;

	private float SpawnDelayAmplified;  // The actual delay used for spawning Marbles.
	private float CurrentSpawnTime;     // Time since last Marble spawn.

	public override void _Ready()
	{
		base._Ready();
		
		SpawnDelayAmplified = SpawnDelayBase / ( 1 + RTSManager.AmplifierBonus[Team] );

		RTSManager.Connect( "AmplifierBonusChanged", this, nameof(OnAmplifierBonusChanged) );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CurrentSpawnTime += delta;
		while ( CurrentSpawnTime > SpawnDelayAmplified )
		{
			SpawnMarble();
			CurrentSpawnTime -= SpawnDelayAmplified;
		}
	}

	public void SpawnMarble()
	{
		var marble = Marble.Instance() as Marble;
		marble.GlobalPosition = GlobalPosition;
		marble.SetTeam( Team );
		GetNode("/root").AddChild( marble );
	}

	private void OnAmplifierBonusChanged( int team, float newVal, float oldVal )
	{
		if ( team == Team )
			SpawnDelayAmplified = SpawnDelayBase / ( 1 + newVal );
	}
}
