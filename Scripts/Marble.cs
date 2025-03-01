using Godot;
using System;

public class Marble : BaseRTSEntity
{
	private Trail Trail;

	public override void _Ready()
	{
		base._Ready();
        AddToGroup( RTSManager.EntityType.Marble.ToString() );
		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Marble, Team );

		Trail = GetNode<Trail>("Trail");

		var MarbleColor = GetNode<Polygon2D>("Inline").Color;
		var MarbleOutline = GetNode<Polygon2D>("Outline").Color;
		var TrailSize = (Godot.Vector2)GetNode("Inline").Get("size");

		var gradient = new Gradient();
		gradient.SetColor(0, MarbleOutline);
		gradient.SetColor(1, MarbleColor * new Color(1, 1, 1, 0));

		//Trail.Gradient.Colors = new Color[] { MarbleOutline, MarbleColor * new Color(1, 1, 1, 0) };
		Trail.Gradient = RTSManager.MarbleGradients[Team];
		Trail.Width = Math.Min( TrailSize.x, TrailSize.y );
	}
}
