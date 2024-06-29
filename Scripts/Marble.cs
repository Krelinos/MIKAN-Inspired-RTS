using Godot;
using System;
using System.Linq;

public class Marble : RigidBody2D
{
	private Line2D Trail;

	private Vector2[] TrailPointsGlobal;

	public override void _Ready()
	{
		Trail = GetNode<Line2D>("Trail");

		var TrailColor = GetNode<Polygon2D>("AntialiasedRegularPolygon2D").Color;
		var TrailOutline = (Color)GetNode("AntialiasedRegularPolygon2D").Get("stroke_color");
		// Ignore the error. AntialiasedRegularPolygon2D is from a GDScript and VSCode cannot detect it.
		var TrailSize = (Vector2)GetNode("AntialiasedRegularPolygon2D").Get("size");

		Trail.Gradient.Colors = new Color[]{ TrailOutline, TrailColor };
		Trail.Width = Math.Min( TrailSize.x, TrailSize.y );

		TrailPointsGlobal = new Vector2[ Trail.Points.Length ];
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		// var _velocity = Velocity;
		// _velocity += new Vector2( 0, Gravity );

		// Velocity = _velocity;

		// MoveAndSlide( Velocity * delta );

		Update();
	}

	public override void _Draw()
	{
		base._Draw();

		// Right shift the trail array so the older positions are closer to the tail end.
		var Len = TrailPointsGlobal.Length;
		var RShiftedTrail = new Vector2[ Len ];
		for ( int i = 1; i < Len; i++ )
			RShiftedTrail[ i ] = TrailPointsGlobal[ i - 1 ];
		RShiftedTrail[ 0 ] = GlobalPosition;

		TrailPointsGlobal = RShiftedTrail;

		// Then translate the global positions to the marble's local position
		var TrailPointsLocal = new Vector2[ Len ];
		for( int i = 0; i < Len; i++ )
			TrailPointsLocal[ i ] = ToLocal( TrailPointsGlobal[ i ] );
		
		Trail.Points = TrailPointsLocal;
	}
}
