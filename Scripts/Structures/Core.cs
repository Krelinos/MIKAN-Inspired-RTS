using Godot;
using System;

public class Core : BaseStructure
{
	[Export] private NodePath CorePolygon1Path;
	[Export] private NodePath CorePolygon2Path;

	private readonly GDScript AntialiasedPolygonScript = (GDScript) GD.Load("res://addons/antialiased_line2d/antialiased_polygon2d.gd");
	private Node2D CorePolygon1;
	private Node2D CorePolygon2;

	public Core()
	{
		Targetable = true;
	}

	public override void _Ready()
	{
		base._Ready();

		CorePolygon1 = GetNode( CorePolygon1Path ) as Node2D;
		CorePolygon2 = GetNode( CorePolygon2Path ) as Node2D;

		for ( int i = 0; i < 3; i+=2 )
			for ( int o = 0; o < 8; o++ )
			{
				var AAPolygon = CreateAAPolygon();
					AAPolygon.Set( "polygon", CreatePointsForArcPolygon(45*o, 45*o+45, 9*i+28, 9*i+36, 4) );
				AddChild( (Node)AAPolygon );
			}
		
		for ( int i = 1; i < 4; i+=2 )
			for ( int o = 0; o < 8; o++ )
			{
				var AAPolygon = CreateAAPolygon();
					AAPolygon.Set( "polygon", CreatePointsForArcPolygon(45*o+22.5f, 45*o+67.5f, 9*i+28, 9*i+36, 4) );
				AddChild( (Node)AAPolygon );
			}
	}

	private Godot.Object CreateAAPolygon()
	{
		Godot.Object AAPolygon = (Godot.Object) AntialiasedPolygonScript.New();
			AAPolygon.Set("stroke_width", 2);
			AAPolygon.Set("color", RTSManager.TeamColors[Team, 0]);
			AAPolygon.Set("stroke_color", RTSManager.TeamColors[Team, 1]);
		
		return AAPolygon;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CorePolygon1.Rotation = Time.GetTicksMsec()/1000f % 360;
		CorePolygon2.Rotation = Time.GetTicksMsec()/1000f % 360 *-1;
	}

	/// <param name="startDegrees">Degree the arc begins. Not radians.</param>
	/// <param name="endDegrees">Degree the arc ends. Not radians.</param>
	/// <param name="startRadius">1st radius of the arc. Typically the inner radius.</param>
	/// <param name="endRadius">2nd radius of the arc. Typically the outer radius.</param>
	/// <param name="segments">How many pieces the arc will be divided into.</param>
	/// <returns>A Vector2[] of vertexes for a Polygon2D. Counter-clockwise.</returns>
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

		return points;
	}
}
