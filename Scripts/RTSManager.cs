using Godot;
using System;

/// <summary>
/// A singleton class that houses various RTS-related tools. Ideally, things that
/// relate more to game management/overseeing go here, while BaseRTSEntity handles
/// things that involve the BaseRTSEntity itself.
/// </summary>
public class RTSManager : Node
{
    // [Signal] public delegate void AmplifierBonusChanged( int team, float newVal, float oldVal );

    [Export]
    public readonly Color[,] TeamColors =
    {
        { new Color("3078f3"), new Color("1c61d5"), new Color("88b4ff"), new Color() }   // Team 0 - Blue
        ,{ new Color("a83e50"), new Color("882255"), new Color("ab6da1"), new Color() }  // Team 1 - Red
        ,{ new Color("CCCCCC"), new Color("AAAAAA"), new Color("FFFFFF"), new Color() }  // Team 2 - Unaffiliated/Neutral
        ,{ new Color("CCCCCC"), new Color("AAAAAA"), new Color("FFFFFF"), new Color() }  // Team 3 - Unused
    };

    public enum EntityType { Marble, Structure, Mobile, Projectile }

    public enum ColorCategory { PrimaryColor, SecondaryColor, TertiaryColor, QuaternaryColor }

    public enum TeamGroupName { Blue, Red, Neutral, Team4 }

    public float[] AmplifierBonus { get; private set; } = new float[4];

    public Gradient[] MarbleGradients { get; private set; } = new Gradient[4];

    public override void _Ready()
    {
        base._Ready();

        for ( int i = 0; i < 4; i++ )
        {
            var gradient = new Gradient();
            gradient.SetColor( 0, TeamColors[i,0] );
            gradient.SetColor( 1, TeamColors[i,1] * new Color(1,1,1,0) );
            MarbleGradients[i] = gradient;
        }
    }

    /// <summary>
    /// Recursively traverses a node's ancestors until it finds one that implements ITeam.
    /// </summary>
    /// <param name="node">The node to start scanning for ITeam, inclusive.</param>
    /// <returns>Integer of GetTeam() once it finds an ancestor that implements ITeam, otherwise 0.</returns>
    public BaseRTSEntity FindRTSEntityOf( Node node )
    {
        if ( (node as BaseRTSEntity) != null )
            return node as BaseRTSEntity;
        else if ( node == GetNode("/root") || node.GetParent() == null )
            return null;
        else return FindRTSEntityOf( node.GetParent() );
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
    public void AssignLayersAndMasks( RigidBody2D entity, EntityType role, int team )
    {
        uint layer = 0b0;
        uint mask = 0b0;

        switch( role )
        {
            // Marbles exists on the 1st layer and detect other Marbles and Structures.
            case EntityType.Marble:
                layer = 0b1;
                mask = 0b0011;
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
        
        // Non-Marbles must detect non-team entities. Apply these bits to the other three teams instead.
        if ( role == EntityType.Marble )
            mask <<= team*4;
        else
        {
            uint temp = mask;
            mask = 0b0;
            for ( int i = 0; i < 4; i++ )
                if ( i != team )
                    mask += temp << 4*i;
        }

        entity.CollisionLayer = layer;
        entity.CollisionMask = mask;
    }

    public float RecalcuateAmplifierBonusFor( int teamIndex )
    {
        float total = 0;
        foreach( Amplifier amp in GetTree().GetNodesInGroup("Amplifier") )
            if ( amp.GetTeam() == teamIndex )
                total += amp.BonusMultiplier;
        
        var oldVal = AmplifierBonus[ teamIndex ];
        AmplifierBonus[ teamIndex ] = total;
        // GD.Print($"Bonus for {((RTSManager.TeamGroupName)teamIndex).ToString()} is " + total);
        // EmitSignal( nameof(AmplifierBonusChanged), teamIndex, AmplifierBonus[teamIndex], oldVal );

        return total;
    }

    public bool IsBitSet(uint b, int pos) { return ((b >> pos) & 1) != 0; }
}
