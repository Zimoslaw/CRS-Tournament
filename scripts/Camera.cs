using Godot;
using System;

public partial class Camera : Camera3D
{
	[Export]
	Node3D PositionIndicator, QuaternionIndicator;

	[Export]
	private float _cameraSpeed = 15;

	public override void _Process(double delta)
	{
		var _originTransform = Transform;
		var _targetTransform = PositionIndicator.GlobalTransform;

		var finalQuaternion = Transform.Basis.GetRotationQuaternion().Slerp(
			_targetTransform.Basis.GetRotationQuaternion(),
			_cameraSpeed * (float)delta
		);

		_originTransform.Basis = new Basis(finalQuaternion);
		Transform = _originTransform;

		Position = Position.Lerp(PositionIndicator.GlobalPosition, _cameraSpeed * (float)delta);
	}
}
