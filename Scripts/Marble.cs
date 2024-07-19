using Godot;
using System;

public class Marble : BaseRTSEntity
{
	private Trail Trail;

	public override void _Ready()
	{
		Trail = GetNode<Trail>("Trail");

		var MarbleColor = GetNode<Polygon2D>("AntialiasedRegularPolygon2D").Color;
		var MarbleOutline = (Color)GetNode("AntialiasedRegularPolygon2D").Get("stroke_color");
		var TrailSize = (Godot.Vector2)GetNode("AntialiasedRegularPolygon2D").Get("size");

		Trail.SetStartColor( MarbleOutline );
		Trail.SetEndColor( MarbleColor * new Color(1, 1, 1, 0) );
		Trail.Width = Math.Min( TrailSize.x, TrailSize.y );
	}
}
