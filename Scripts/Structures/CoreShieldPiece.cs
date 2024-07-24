using Godot;
using System;

public class CoreShieldPiece : BaseStructure
{
    [Export] public float RotationCoefficent;
    [Export] public CollisionPolygon2D Collision;
    [Export] public Polygon2D Polygon;

    public CoreShieldPiece()
    {
        MaxHP = 20f;
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        Rotation += delta * RotationCoefficent;
    }

    public override void AlterHealth(int amt)
    {
        base.AlterHealth(amt);
        HP = Math.Max( HP, 0 );
        Polygon.Modulate = new Color(1,1,1,HP/MaxHP);
    }
    
    public override void SetHealth(int amt)
    {
        base.SetHealth(amt);
        HP = Math.Max( HP, 0 );
        Polygon.Modulate = new Color(1,1,1,HP/MaxHP);
    }
}