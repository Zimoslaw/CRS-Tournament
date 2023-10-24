using Godot;
using System;

public partial class PlayerMovement : Node3D
{
    // Movement parameters
    [Export]
    public float MovementSpeed = 5; // meters/second
    [Export]
    private float JumpLength = 1; // multiplier
    [Export]
    private float JumpHeight = 0.25f; // multiplier
    private const float RotationSpeed = 360;
    private const float JumpingSpeed = 5;

    // Movement helper variables
    private int _rotationDirection = 0;
    private float _prevDegrees = 0;
    private float _deltaHeight = 1;
    private float _deltaRotation = 0;

    // States
    private bool _isRotating = false;
    private bool _isJumping = false;

    // Debug
    [Export]
    private BaseMaterial3D _debugMaterial;
    private SphereMesh _debugSphere;
    private MeshInstance3D _debugNode;

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
            _deltaHeight -= JumpingSpeed * delta;
            Translate(new Vector3(0, JumpHeight * JumpFunction(_deltaHeight), JumpLength * (float)(JumpingSpeed * delta)));

            if (_deltaHeight <= -0.9)
            {
                _isJumping = false;

                _deltaHeight = 1;

                Position = new Vector3(Position.X, 1, Position.Z); //$GroundCollider.get_collider().getNode()
            }

            _debugSphere = new SphereMesh
            {
                Radius = 0.05f,
                Height = 0.1f,
                RadialSegments = 6,
                Rings = 6
            };
            _debugNode = new MeshInstance3D
            {
                Mesh = _debugSphere
            };
            _debugNode.Position = new Vector3(0, -1, 0) + Position;
            GetParent().AddChild(_debugNode);
        }
    }

    static private float JumpFunction(float x)
    {
        return (float)(0.65 * Mathf.Tan(x));
    }
}
