using Godot;
using System;
using System.ComponentModel;

public class Trail : Line2D
{
    [Export] public int Length { get; private set; } = 16;
    public Color StartColor { get; private set; } = new Color(1,1,1);
    public Color EndColor { get; private set; } = new Color(0.8f,0.8f,0.8f);

    private Vector2[] PointsGlobal;

    private float TimeSinceLastUpdate = 0f;
    private const float UpdateIncrementTime = 1/15f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Points = new Vector2[ Length ];
        PointsGlobal = new Vector2[ Length ];
        for( int i = 0; i < Length; i++ )
            PointsGlobal[i] = GlobalPosition;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        TimeSinceLastUpdate += delta;

        if ( TimeSinceLastUpdate >= UpdateIncrementTime )     // Update trail every 1/60 seconds.
        {
            UpdatePointPositions();
            TimeSinceLastUpdate -= UpdateIncrementTime;
        }
    }

    private void UpdatePointPositions()
    {
        // Right shift the trail array so the older positions are closer to the tail end.
		var RShiftedTrail = new Godot.Vector2[ Length ];
		for ( int i = 1; i < Length; i++ )
			RShiftedTrail[ i ] = PointsGlobal[ i - 1 ];
		RShiftedTrail[ 0 ] = GlobalPosition;

		PointsGlobal = RShiftedTrail;

		// Then translate the global positions to the trail's local position
		var TrailPointsLocal = new Godot.Vector2[ Length ];
		for( int i = 0; i < Length; i++ )
			TrailPointsLocal[ i ] = ToLocal( PointsGlobal[ i ] );
		
		Points = TrailPointsLocal;
    }

    public void SetStartColor( Color val ) { StartColor = val; Gradient.Colors = new Color[] { StartColor, EndColor }; }
    public void SetEndColor( Color val ) { EndColor = val; Gradient.Colors = new Color[] { StartColor, EndColor }; }   
}
