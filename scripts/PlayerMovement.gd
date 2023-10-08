extends Node3D

# Movement parameters
var movementSpeed: float = 5
const rotationSpeed: float = 360
const jumpingSpeed: float = 10
const jumpingLength: float = 2

var rotationDirection: int = 0
var prevDegrees: float = 0
var deltaHeight: float = 0

# States
var isRotating: bool = false
var isJumping: bool = false

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	doJump(delta)
	doRotation(delta)
	doMovement(delta)
	
func doMovement(delta: float):
	if Input.is_action_pressed("MoveForward"):
		translate(Vector3(0, 0, movementSpeed * delta))
		
	if Input.is_action_pressed("MoveBackwards"):
		translate(Vector3(0, 0, (-movementSpeed * delta) /2))

func doRotation(delta: float):
	if Input.is_action_just_pressed("MoveRight") && !isRotating && !isJumping:
		isRotating = true
		rotationDirection = -1
		prevDegrees = rotation_degrees.y
	elif Input.is_action_just_pressed("MoveLeft") && !isRotating && !isJumping:
		isRotating = true
		rotationDirection = 1
		prevDegrees = rotation_degrees.y
		
	if isRotating:
		rotation_degrees.y += rotationDirection * rotationSpeed * delta
		if abs(rotation_degrees.y - prevDegrees) >= 90:
			rotation_degrees.y = prevDegrees + (90 * rotationDirection)
			isRotating = false

func doJump(delta: float):
	if Input.is_action_just_pressed("Jump") && !isJumping:
		isJumping = true
		
	if isJumping:
		deltaHeight += (jumpingSpeed * delta)
		translate(Vector3(0, 0.2 * sin(deltaHeight), 0.314 * jumpingSpeed * delta))
		if deltaHeight >= (jumpingLength * PI):
			isJumping = false
			deltaHeight = 0
