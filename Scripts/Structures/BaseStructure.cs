using System;
using Godot;

public class BaseStructure : BaseRTSEntity
{
    public override void _Ready()
    {
        base._Ready();
        AddToGroup( RTSManager.EntityType.Structure.ToString() );
        
		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Structure, Team );
    }
}