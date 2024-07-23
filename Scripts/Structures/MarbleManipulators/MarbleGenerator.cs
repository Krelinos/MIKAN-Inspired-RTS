using Godot;
using System;

public class MarbleGenerator : SinWaveTurret
{
	[Export] public PackedScene Marble { get; private set; }

	// With no amplifiers, one Marble spawns every BaseChargeTime seconds.
	[Export] public float BaseChargeTime { get; private set; } = 3;
	[Export] public NodePath SpawnerPath { get; private set; }
	[Export] public NodePath IndicatorPath { get; private set; }

	private float CurrentCharge;		// Increases by 1 * AmpliferBonus every second.
	private Spawner Spawner;
	private Node Indicator;

	public override void _Ready()
	{
		base._Ready();
		Spawner = GetNode<Spawner>( SpawnerPath );
		Indicator = GetNode( IndicatorPath );
		// RTSManager.Connect( "AmplifierBonusChanged", this, nameof(OnAmplifierBonusChanged) );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		base._Process( delta );

		CurrentCharge += delta * (1+RTSManager.AmplifierBonus[Team]);
		while ( CurrentCharge > BaseChargeTime )
		{
			Spawner.FireOnce();
			CurrentCharge -= BaseChargeTime;
		}

		Indicator.Set( "angle_degrees", 360 - CurrentCharge / BaseChargeTime * 360 );
	}

	// private void OnAmplifierBonusChanged( int team, float newVal, float oldVal )
	// {
	// 	if ( team == Team )
	// 		ChargeBonus = BaseChargeTime / ( 1 + newVal );
	// }
}
