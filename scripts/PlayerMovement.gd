extends Node3D

var MovementSpeed: float = 5
var RotationSpeed: float = 360
var isRotating: bool = false
var rotationDirection: int = 0

var prevDegrees: float

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	doRotation(delta)
	doMovement(delta)
	
func doMovement(delta: float):
	if Input.is_action_pressed("MoveForward"):
		translate_object_local(Vector3(0, 0, MovementSpeed * delta))
		
	if Input.is_action_pressed("MoveBackwards"):
		translate_object_local(Vector3(0, 0, (-MovementSpeed * delta) /2))

func doRotation(delta: float):
	if Input.is_action_just_pressed("MoveRight") && !isRotating:
		isRotating = true
		rotationDirection = -1
		prevDegrees = rotation.y
	elif Input.is_action_just_pressed("MoveLeft") && !isRotating:
		isRotating = true
		rotationDirection = 1
		prevDegrees = rotation.y
		
	if isRotating:
		rotate_object_local(Vector3(0, rotationDirection, 0), deg_to_rad(RotationSpeed * delta))
		if rad_to_deg(abs(rotation.y - prevDegrees)) >= 90:
			rotation.y = deg_to_rad(prevDegrees + (90 * rotationDirection))
			isRotating = false
