using Godot;
using System;

/*
    A singleton class that houses various RTS-related tools.
*/
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
        Marble
        ,Structure
        ,Mobile
        ,Projectile
    }

    public enum ColorCategory
    {
        PrimaryColor
        ,SecondaryColor
        ,TertiaryColor
        ,QuaternaryColor
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

    /*
        Colors Polygon2Ds based on team and color index.
        Uses a class that implements ITeam to determine team.
        Color index is specified via Godot's Group system.
    */
    public void ApplyPaletteTo( Node node, bool recursive = true )
    {
        int team = GetTeamOf(node);
        int colorIndex = -1;
        
        if ( node.IsInGroup(ColorCategory.PrimaryColor.ToString()) )
            colorIndex = 0;
        else if ( node.IsInGroup(ColorCategory.SecondaryColor.ToString()) )
            colorIndex = 1;
        else if ( node.IsInGroup(ColorCategory.TertiaryColor.ToString()) )
            colorIndex = 2;
        else if ( node.IsInGroup(ColorCategory.QuaternaryColor.ToString()) )
            colorIndex = 3;
        
        if ( colorIndex >= 0 )
        {
            Color color = TeamColors[ team , colorIndex ];
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

    /// <summary>
    /// Sets a CollisionObject2D's layer and mask to certain preset configurations
    /// depending on its team and role. Any existing bits will be overwritten.
    ///
    /// The layers and masks can be freely changed afterward of course; this is
    /// just a preset. There can be Projectiles that detect Marbles to destroy
    /// them or Mobiles that exit the Mobile layer temporarily to be intangible.
    /// </summary>
    public void AssignLayersAndMasks( CollisionObject2D body, EntityType role, int team )
    {
        uint layer = 0b0;
        uint mask = 0b0;

        switch( role )
        {
            // Marbles exists on the 1st layer and detect Structures.
            case EntityType.Marble:
                layer = 0b1;
                mask = 0b10;
                break;
            // Structures exist on the 2nd layer and detect Mobiles and Projectiles.
            case EntityType.Structure:
                layer = 0b10;
                mask = 0b1100;
                break;
            // Mobiles exists on the 3rd layer and detect Structures, other Mobiles, and Projectiles.
            case EntityType.Mobile:
                layer = 0b100;
                mask = 0b1110;
                break;
            // Projectiles exists on the 4th layer and detect Structures and Mobiles.
            case EntityType.Projectile:
                layer = 0b1000;
                mask = 0b110;
                break;
        }

        layer <<= team*4;
        // Mask must detect non-team entities. Apply these bits to the other three teams.
        uint temp = mask;
        mask = 0b0;
        for ( int i = 0; i < 4; i++ )
            if ( i != team )
                mask += temp << 4*i;

        body.CollisionLayer = layer;
        body.CollisionMask = mask;
    }
}
