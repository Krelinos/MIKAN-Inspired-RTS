using Godot;
using Microsoft.SqlServer.Server;
using System;

public class SlotResearchBlock : BaseStructure
{
    [Export] public NodePath LabelPath;

    private Label Label;

    public override void _Ready()
    {
        base._Ready();

        Label = GetNode<Label>( LabelPath );

        Label.Text = HP.ToString();

        Connect( "body_entered", this, nameof(OnBodyEntered) );
    }

    public void OnBodyEntered( Node node )
    {
        if ( node is Marble marble )
        {
            HP -= 1;
            Label.Text = HP.ToString();
            node.QueueFree();
            if ( HP <= 0 )
                QueueFree();
        }
    }
}
