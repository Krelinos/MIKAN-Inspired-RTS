using Godot;
using System;
using System.Linq;

/// <summary>
/// Generates a CollisionPolygon2D based on the Polygon2D direct children this class is attached to.
/// </summary>
public class AutoHitbox : Node
{
	public override void _Ready()
	{
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

		// // Collision poly will be a direct child of the closest PhysicsBody2D ancestor.
		// Node pBody2D = this;
		// while ( !(pBody2D is PhysicsBody2D) )
		// 	if ( (pBody2D = GetParent()) is PhysicsBody2D)
		// 		break;
		
		// if ( pBody2D is PhysicsBody2D )
		// {
		// 	var collision = new CollisionPolygon2D();
		// 	collision.Polygon = this.Polygon;
		// 	pBody2D.CallDeferred( "add_child", collision );
		// }
		// else
		// 	GD.PushError("AutoHitbox does not have a PhysicsBody2D ancestor.");
	}
}
