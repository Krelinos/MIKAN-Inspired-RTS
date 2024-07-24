using Godot;
using System;

public class Core : BaseStructure
{
	public Core()
	{
		Targetable = true;
	}

	public override void _Ready()
	{
		base._Ready();

		var test = new Polygon2D();

		test.Polygon = CreatePointsForArcPolygon( 0, 90, 24, 28, 4 );

		AddChild( test );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		
	}

	private Vector2[] CreatePointsForArcPolygon( float startDegrees, float endDegrees, float startRadius, float endRadius, int segments )
	{
		Vector2[] points = new Vector2[ 2 + segments*2 ];
		float segmentAngleIncrement = ( endDegrees - startDegrees ) % 360 / segments;

		for ( int i = 0; i <= segments; i++ )
			points[i] = new Vector2(
				(float)Math.Cos( (i*segmentAngleIncrement+startDegrees)*Math.PI/180 )*startRadius
				,(float)Math.Sin( (i*segmentAngleIncrement+startDegrees)*Math.PI/180 )*startRadius
			);
		
		for ( int i = segments; i >= 0; i-- )
			points[ segments*2-i+1 ] = new Vector2(
				(float)Math.Cos( (i*segmentAngleIncrement+startDegrees)*Math.PI/180 )*endRadius
				,(float)Math.Sin( (i*segmentAngleIncrement+startDegrees)*Math.PI/180 )*endRadius
			);
	
		GD.Print( points.ToString() );

		return points;
	}
}
