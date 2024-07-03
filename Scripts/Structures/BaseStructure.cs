using System;
using Godot;

public class BaseStructure : BaseRTSEntity
{
    public override void _Ready()
    {
        MaxHP = Int32.MaxValue;

        base._Ready();
        AddToGroup( RTSManager.EntityType.Structure.ToString() );

		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Structure, Team );
    }
}