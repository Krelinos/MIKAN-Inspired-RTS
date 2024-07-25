using Godot;
using System;

public class Stage : Node2D
{
    [Export] public int Team { get; private set; }
    public override void _Ready()
    {
        var RTSManager = GetNode<RTSManager>("/root/RTSManager");
        RTSManager.ApplyPaletteTo( this, Team );
    }
}
