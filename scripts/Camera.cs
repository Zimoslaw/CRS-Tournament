using Godot;
using System;

public partial class Camera : Camera3D
{
	[Export]
	Node3D Indicator;

	[Export]
	private float _cameraSpeed = 15f;

	public override void _Process(double delta)
	{
		Rotation = Rotation.Slerp(Indicator.GlobalRotation, _cameraSpeed * (float)delta);

		Position = Position.Lerp(Indicator.GlobalPosition, _cameraSpeed * (float)delta);
	}
}
