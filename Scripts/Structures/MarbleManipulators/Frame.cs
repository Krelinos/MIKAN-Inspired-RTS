using Godot;
using System;

public class Frame : BaseStructure
{
    public override void _Ready()
    {
        base._Ready();

        foreach ( Node child in GetChildren() )
			if ( child is Polygon2D poly2D )
			{
				var collision = new CollisionPolygon2D
				{
					Polygon = poly2D.Polygon
					,Position = poly2D.Position
					,Rotation = poly2D.Rotation
				};
				// CallDeferred( "add_child", collision );
				AddChild( collision );
			}
    }
}
