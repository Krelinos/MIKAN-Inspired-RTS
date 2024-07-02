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
        { new Color("CCCCCC"), new Color("AAAAAA"), new Color("FFFFFF"), new Color() }   // Team 0 - Unaffiliated/Neutral
        ,{ new Color("3078f3"), new Color("1c61d5"), new Color("88b4ff"), new Color() }  // Team 1 - Blue
        ,{ new Color("a83e50"), new Color("882255"), new Color("ab6da1"), new Color() }  // Tean 2 - Red
    };

    public enum EntityType { Marble, Structure, Mobile, Projectile }

    public enum ColorCategory { PrimaryColor, SecondaryColor, TertiaryColor, QuaternaryColor }

    public enum TeamGroupName { Neutral, Blue, Red }

    /// <summary>
    /// Recursively traverses a node's ancestors until it finds one that implements ITeam.
    /// </summary>
    /// <param name="node">The node to start scanning for ITeam, inclusive.</param>
    /// <returns>Integer of GetTeam() once it finds an ancestor that implements ITeam, otherwise 0.</returns>
    public int FindTeamOf( Node node )
    {
        if ( (node as ITeam) != null )
            return (node as ITeam).GetTeam();
        else if ( node == GetNode("/root") || node.GetParent() == null )
            return 0;
        else return FindTeamOf( node.GetParent() );
    }

    /// <summary>
    /// Colors Polygon2Ds based on team and color index.
    /// Color index is specified via Godot's Group system.
    /// </summary>
    public void ApplyPaletteTo( Node node, int team, bool recursive = true )
    {
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
                ApplyPaletteTo( n, team, true );
    }

    /// <summary>
    /// Sets a CollisionObject2D's layer and mask to certain preset configurations
    /// depending on its team and role. Any existing bits will be overwritten.
    ///
    /// The layers and masks can be freely changed afterward of course; this is
    /// just a preset. There can be Projectiles that detect Marbles to destroy
    /// them or Mobiles that exit the Mobile layer temporarily to be intangible.
    /// </summary>
    public void AssignLayersAndMasks( BaseRTSEntity entity, EntityType role, int team )
    {
        uint layer = 0b0;
        uint mask = 0b0;

        switch( role )
        {
            // Marbles exists on the 1st layer and detect Structures.
            case EntityType.Marble:
                layer = 0b1;
                mask = 0b0010;
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
                mask = 0b0110;
                break;
        }
        
        layer <<= team*4;
        
        // Mask must detect non-team entities. Apply these bits to the other three teams.
        uint temp = mask;
        mask = 0b0;
        for ( int i = 0; i < 4; i++ )
            if ( i != team )
                mask += temp << 4*i;

        entity.CollisionLayer = layer;
        entity.CollisionMask = mask;
    }

    public bool IsBitSet(uint b, int pos) { return ((b >> pos) & 1) != 0; }
}
