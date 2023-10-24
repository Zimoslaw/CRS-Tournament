using Godot;
using System;

public partial class PlayerMovement : Node3D
{
    // Movement parameters
    public float MovementSpeed = 5;
    private const float RotationSpeed = 360;
    private const float JumpingSpeed = 10;
    private const float JumpingLength = 2;

    // Movement helper variables
    private int _rotationDirection = 0;
    private float _prevDegrees = 0;
    private float _deltaHeight = 0;
    private float _deltaRotation = 0;

    // States
    private bool _isRotating = false;
    private bool _isJumping = false;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        DoJump((float)delta);

        DoRotation((float)delta);

        DoMovement((float)delta);
    }

    private void DoMovement(float delta)
    {
        if (_isJumping)
            return;


        if (Input.IsActionPressed("MoveForward")) // Move forward
            
            Translate(new Vector3(0, 0, MovementSpeed * delta));


        if (Input.IsActionPressed("MoveBackwards")) // Move forward (2 x slower)
            Translate(new Vector3(0, 0, (-MovementSpeed * delta) / 2));

    }

    private void DoRotation(float delta)
    {
        if (_isJumping)
            return;

        if (Input.IsActionJustPressed("MoveRight") && !_isRotating) // Turn right
        {
            _isRotating = true;
            _rotationDirection = -1;
            _prevDegrees = RotationDegrees.Y;
        }
        else if (Input.IsActionJustPressed("MoveLeft") && !_isRotating) //Turn left
        {
            _isRotating = true;
            _rotationDirection = 1;
            _prevDegrees = RotationDegrees.Y;
        }
        if (_isRotating)
        {
            RotationDegrees += new Vector3(0, _rotationDirection * RotationSpeed * delta, 0);
            _deltaRotation += _rotationDirection * RotationSpeed * delta;

            if (Mathf.Abs(_deltaRotation) > 85)
            {
                RotationDegrees = new Vector3(0, _prevDegrees + (90 * _rotationDirection), 0);
                _deltaRotation = 0;
                _isRotating = false;
            }
        }
    }
    private void DoJump(float delta)
    {
        if (_isRotating)
            return;

        if (Input.IsActionJustPressed("Jump") && !_isJumping) // Jump
		    _isJumping = true;

        if (_isJumping)
        {
            _deltaHeight += (JumpingSpeed * delta);
            Translate(new Vector3(0, (float)0.1 * Mathf.Sin(_deltaHeight), (float)(0.314 * JumpingSpeed * delta)));

            if (_deltaHeight >= (JumpingLength * Mathf.Pi))
            {
                _isJumping = false;

                _deltaHeight = 0;

                Position = new Vector3(Position.X, 1, Position.Z); //$GroundCollider.get_collider().getNode()
            }

            AddChild(new MeshInstance3D());
        }
    }

    private float JumpFunction(float x)
    {
        return 2 * x - Mathf.Pow(x, 2);
    }
}
