using UnityEngine;
using System.Collections;
using LWG.Core.Fsm;
using TMPro;

// This script moves the character controller forward
// and sideways based on the arrow keys.
// It also jumps when pressing space.
// Make sure to attach a character controller to the same game object.
// It is recommended that you make only one call to Move or SimpleMove per frame.

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BirdFollower))]
public class PlayerBirdController : MonoBehaviour {
    public enum States {
        Idle,
        Walking,
        Flying,
        MaybeFlying,
        Disabled,
    }

    public enum Triggers {
        Moving,
        NotMoving,
        Grounded,
        Airborne,
        Disable,
        ForceExit,
    }


    // NOTE(clark): This is a singleton!! It is an antipattern!! BUt works well for jams!!
    public static PlayerBirdController _isntance = null;
    public static PlayerBirdController Instance {
        get {
            if(_isntance == null) _isntance = FindObjectOfType<PlayerBirdController>();
            return _isntance;
        }
    }

    Camera _cam = null;
    CharacterController _characterController = null;
    BirdFollower _follower = null;

    [SerializeField] Animator _animator = null;
    [SerializeField] SoundDefPlayer _soundPlayer = null;
    [SerializeField] ParticleSystem _jumpParticles = null;
    [SerializeField] CameraTrauma _trauma = null;
    [SerializeField] GameObject _childDisplay = null;
    [SerializeField] float _speed = 6.0f;
    [SerializeField] float _jumpSpeed = 8.0f;
    [SerializeField] float _gravity = 20.0f;

    // const float _maxFallVel = 1f;
    // const float _minFallVel = 0f;
    const float _cameraTrauma = 1f;

    FiniteStateMachine<States, Triggers> _fsm = new FiniteStateMachine<States, Triggers>(States.Idle);

    private Vector3 _moveDirection = Vector3.zero;
    Vector3 NonVerticalMovement => new Vector3(_moveDirection.x, 0f, _moveDirection.z);

    void Awake() {
        _cam = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _follower = GetComponent<BirdFollower>();
        ConstructStateMachine();
    }

    public void Disable() => _fsm.SetTrigger(Triggers.Disable);

    void Start() => _fsm.Start();

    void ConstructStateMachine() {
        // States.
        _fsm.AddState(States.Idle, new State() {
            enter = () => _animator.Play("Idle"),
            update = DoMovementStuff,
        });

        _fsm.AddState(States.Walking, new State() {
            enter = () => _animator.Play("Walking"),
            update = () => {
                DoMovementStuff();
                var speed = NonVerticalMovement.magnitude / _speed; 
                _animator.speed = speed;
            },
            exit = () => _animator.speed = 1f,
        });

        _fsm.AddState(States.MaybeFlying, new TimerState(0.2f) {
            update = () => DoMovementStuff(), 
            timeout = () => _fsm.SetTrigger(Triggers.ForceExit),
        });

        _fsm.AddState(States.Flying, new State() {
            enter = () => _animator.Play("FlyingWalking"),
            update = () => {
                DoMovementStuff();
                var speed = NonVerticalMovement.magnitude / _speed; 
                _animator.speed = speed;
            },
            exit = () => {
                // Add camera trauma
                // var normalizedVelocity = Mathf.Lerp(_minFallVel, _maxFallVel, Mathf.Abs(_moveDirection.y));
                // _trauma.AddTrauma(normalizedVelocity * _cameraTrauma);
                _trauma.AddTrauma(_cameraTrauma);

                // Set animator speed back. 
                _animator.speed = 1f;
            },
        });

        _fsm.AddState(States.Disabled, new State() {});

        // Transitions.
        _fsm.AddTransition(States.Idle, States.Walking, Triggers.Moving);
        _fsm.AddTransition(States.Idle, States.MaybeFlying, Triggers.Airborne);

        _fsm.AddTransition(States.Walking, States.Idle, Triggers.NotMoving);
        _fsm.AddTransition(States.Walking, States.MaybeFlying, Triggers.Airborne);

        _fsm.AddTransition(States.MaybeFlying, States.Idle, Triggers.Grounded, Triggers.NotMoving);
        _fsm.AddTransition(States.MaybeFlying, States.Walking, Triggers.Grounded);
        _fsm.AddTransition(States.MaybeFlying, States.Flying, Triggers.ForceExit);

        _fsm.AddTransition(States.Flying, States.Idle, Triggers.Grounded);

        _fsm.AddTransitionAll(States.Disabled, Triggers.Disable, false);
    }

    void DoMovementStuff() {
        // Project famera forward onto the vertical plane. 
        var planeNormal = Vector3.up;
        var camPlaneProjection = Vector3.ProjectOnPlane(_cam.transform.forward, planeNormal);
        var forward = camPlaneProjection.normalized * Input.GetAxis("Vertical");

        var horizontalVector = Vector3.Cross(camPlaneProjection, Vector3.up);
        // We are grounded, so recalculate
        // move direction directly from axes
        var strafe = (-Input.GetAxis("Horizontal")) * horizontalVector.normalized;

        var previousVelocity = _moveDirection.y;
        _moveDirection = forward + strafe;
        _moveDirection *= _speed;
        _moveDirection.y = previousVelocity;

        if(_characterController.isGrounded) {
            _moveDirection.y = 0f;
        }
        
        // Do the jump
        if (Input.GetButtonDown("Jump")) {
            _soundPlayer.PlaySound(0);
            _jumpParticles.Play();
            _moveDirection.y = _jumpSpeed;
        }
        _moveDirection.y -= _gravity * Time.deltaTime;

        // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);


        // Drive the duck transform forward. 
        _childDisplay.transform.forward = camPlaneProjection;    
    }

    void Update() {
        if(_characterController.isGrounded) {
            _fsm.SetTrigger(Triggers.Grounded);
        }
        else {
            _fsm.SetTrigger(Triggers.Airborne);
        }

        var nonVerticalMovemet = NonVerticalMovement;
        if(nonVerticalMovemet.magnitude > 0.01) {
            _fsm.SetTrigger(Triggers.Moving);
        }
        else {
            _fsm.SetTrigger(Triggers.NotMoving);
        }

        _fsm.Update(Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        var birdController = other.GetComponent<CollectableBirdController>();
        // If they're a bird we care about:
        if(birdController) {
            birdController.SetFollowing(_follower.GenerateHandle());
        }
    }
}