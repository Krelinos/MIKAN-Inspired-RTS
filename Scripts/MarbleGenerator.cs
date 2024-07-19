using Godot;
using System;

public class MarbleGenerator : BaseRTSEntity
{
	[Export] public PackedScene Marble { get; private set; }

	// With no amplifiers, one Marble spawns every BaseChargeTime seconds.
	[Export] public float BaseChargeTime { get; private set; } = 3;

	private float CurrentCharge;		// Increases by 1 * AmpliferBonus every second.

	public override void _Ready()
	{
		base._Ready();
		// RTSManager.Connect( "AmplifierBonusChanged", this, nameof(OnAmplifierBonusChanged) );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CurrentCharge += delta * (1+RTSManager.AmplifierBonus[Team]);
		while ( CurrentCharge > BaseChargeTime )
		{
			SpawnMarble();
			CurrentCharge -= BaseChargeTime;
		}
	}

	public void SpawnMarble()
	{
		var marble = Marble.Instance() as Marble;
		marble.Position = Vector2.Zero;
		marble.SetTeam( Team );
		AddChild( marble );
	}

	// private void OnAmplifierBonusChanged( int team, float newVal, float oldVal )
	// {
	// 	if ( team == Team )
	// 		ChargeBonus = BaseChargeTime / ( 1 + newVal );
	// }
}
