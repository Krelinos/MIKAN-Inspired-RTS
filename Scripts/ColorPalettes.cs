using Godot;
using System;

public class ColorPalettes : Node
{
    [Export]
    public readonly Color Team1PrimaryColor = new Color("56b4e9ff");
    [Export]
    public Color Team1SecondaryColor = new Color("0072b2ff");
    [Export]
    public Color Team1TertiaryColor = new Color("94cbecff");
    
    [Export]
    public Color Team2PrimaryColor = new Color("d55e00ff");
    [Export]
    public Color Team2SecondaryColor = new Color("e69f00ff");
    [Export]
    public Color Team2TertiaryColor = new Color("f0e442ff");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
