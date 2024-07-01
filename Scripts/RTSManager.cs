using Godot;
using System;

public class RTSManager : Node
{
    [Export]
    public readonly Color[,] TeamColors =
    {
        { new Color("CCCCCC"), new Color("AAAAAA"), new Color("FFFFFF"), new Color() }   // Team 0 - Unaffiliated
        ,{ new Color("3078f3"), new Color("1c61d5"), new Color("88b4ff"), new Color() }  // Team 1 - Blue
        ,{ new Color("a83e50"), new Color("882255"), new Color("ab6da1"), new Color() }  // Tean 2 - Red
    };

    public enum EntityType
    {
        Marbles
        ,Structures
        ,Mobiles
        ,Projectiles
    }

    /*
        Colors a shape based on team and color index.
        Color index is specified via Godot's Group system.
    */
    public void ApplyPaletteTo( Node node, bool recursive = true )
    {
        int colorIndex = -1;
        if ( node.IsInGroup("ColorPrimary") )
            colorIndex = 0;
        else if ( node.IsInGroup("ColorSecondary") )
            colorIndex = 1;
        else if ( node.IsInGroup("ColorTertiary") )
            colorIndex = 2;
        else if ( node.IsInGroup("ColorQuaternary") )
            colorIndex = 3;
        
        if ( colorIndex >= 0 )
        {
            Color color = TeamColors[ GetTeamOf(node) , colorIndex ];
            switch( node )
            {
                case Polygon2D p:
                    p.Color = color;
                    break;
            }
        }
        
        if ( recursive )
            foreach( Node n in node.GetChildren() )
                ApplyPaletteTo( n, true );
    }

    // Recursively traverses a node's ancestors, checking if any have implemented ITeam.
    // Returns int of GetTeam() if an ancestor can, otherwise 0.
    public int GetTeamOf( Node node )
    {
        if ( (node as ITeam) != null )
            return (node as ITeam).GetTeam();
        else if ( node == GetNode("/root") || node.GetParent() == null )
            return 0;
        else return GetTeamOf( node.GetParent() );
    }
}
