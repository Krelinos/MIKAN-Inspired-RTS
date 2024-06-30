using Godot;
using System;
using System.CodeDom;

public class ColorPalettes : Node
{
    [Export]
    public readonly Color Team1PrimaryColor = new Color("3078f3");
    [Export]
    public readonly Color Team1SecondaryColor = new Color("1c61d5");
    [Export]
    public readonly Color Team1TertiaryColor = new Color("88b4ff");
    
    [Export]
    public readonly Color Team2PrimaryColor = new Color("CC6677");
    [Export]
    public readonly Color Team2SecondaryColor = new Color("AA4499");
    [Export]
    public readonly Color Team2TertiaryColor = new Color("882255");

    /*

    */
    public void ApplyPaletteTo( Node node, bool recursive )
    {
        switch( node )
        {
            case Polygon2D p:
                Color color;    // CXY, where X is team number and Y is color index
                switch( node.Name.Substring( node.Name.Length-3 ) )
                {
                    case "C11":
                        color = Team1PrimaryColor;
                        break;
                    case "C12":
                        color = Team1SecondaryColor;
                        break;
                    case "C13":
                        color = Team1TertiaryColor;
                        break;
                    case "C21":
                        color = Team2PrimaryColor;
                        break;
                    case "C22":
                        color = Team2SecondaryColor;
                        break;
                    case "C23":
                        color = Team2TertiaryColor;
                        break;
                    default:
                        color = new Color(1,1,1);
                        GD.PushError( String.Format("Node '{0}' gave unknown palette code '{1}'", node.Name, node.Name.Substring( node.Name.Length-3 )) );
                        break;
                }
                p.Color = color;
                break;
        }
        if ( recursive )
            foreach( Node n in node.GetChildren() )
                ApplyPaletteTo( n, true );
    }
}
