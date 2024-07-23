using System;
using Godot;

/// <summary>
/// Structures are invunerable entites that are immobile, but may have parts that rotate.
/// They serve as special game elements such as the Marble distribution system or powerful Core weapons.
/// </summary>
public class BaseStructure : BaseRTSEntity
{   
    public override void _Ready()
    {

        base._Ready();
        AddToGroup( RTSManager.EntityType.Structure.ToString() );

		RTSManager.AssignLayersAndMasks( this, RTSManager.EntityType.Structure, Team );
    }

    public override void SetHealth( int amt ) {
		// Nothing.
	}
	public override void AlterHealth( int amt ) {
        // Nothing. Is this a good practice? Silently absorbing the SetHealth
        // and AlterHealth functions since they do not affect Structures?
        // Message me if you know, whomever is looking at this code.
    }
}