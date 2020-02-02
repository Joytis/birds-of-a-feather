using UnityEngine;
using System.Collections;
using LWG.Core.Fsm;
using TMPro;

// This all spaghetti code. 
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider))]
public class CollectableBirdController : MonoBehaviour {
    public enum States {
        Idle,
        Walking,
        Flying,

        FollowingIdle,
        FollowingWalking,
        FollowingFlying,

        Disabled,
    }

    public enum Triggers {
        Moving,
        NotMoving,
        Grounded,
        Airborne,
        FollowingTarget,

        Disable,
    }

    const float _fadeInTime = 0.5f;

    public BirdChannelAsset BirdChannel => _channel;

    public void Disable() => _fsm.SetTrigger(Triggers.Disable);

    CharacterController _characterController = null;
    DriveChannelByOffset _driveChannelByOffset = null;
    BirdFollower.Handle _handle = null;

    [Header("Bird channel")]
    [SerializeField] BirdChannelAsset _channel = null;

    Animator _animator = null;
    [Header("Literially everything else. ")]
    [SerializeField] BirdChannelRuntimeSet _birdSet = null;
    [SerializeField] CollectableBirdRuntimeSet _collectableSet = null;
    [SerializeField] Collider _triggerCollider = null;
    [SerializeField] GameObject _childDisplay = null;
    [SerializeField] float _speed = 6.0f;
    // [SerializeField] float _jumpSpeed = 8.0f;
    [SerializeField] float _gravity = 20f;

    FiniteStateMachine<States, Triggers> _fsm = new FiniteStateMachine<States, Triggers>(States.Idle);

    private Vector3 _moveDirection = Vector3.zero;
    Vector3 NonVerticalMovement => new Vector3(_moveDirection.x, 0f, _moveDirection.z);

    public bool IsFollowing => _handle != null;


    public void SetFollowing(BirdFollower.Handle handle) {
        _triggerCollider.enabled = false;
        _handle = handle;
        _fsm.SetTrigger(Triggers.FollowingTarget);
    }

    void Awake() {
        // _cam = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _driveChannelByOffset = GetComponent<DriveChannelByOffset>();
        _animator = _childDisplay.GetComponent<Animator>();
        ConstructStateMachine();
    }

    void UpdateAnimatorSpeed() => _animator.speed = NonVerticalMovement.magnitude / _speed;
    void Start() {
        _fsm.Start();
        _collectableSet.Add(this);
    }

    void ConstructStateMachine() {
        _fsm.AddState(States.Idle, new State() {
            enter = () => {
                // get some offset in the animation. 
                _animator.Play("Idle", -1, Random.value);
            },
            update = () => {
                DoMovementStuff();
            },
        });

        _fsm.AddState(States.Walking, new State() {
            enter = () => _animator.Play("Walking"),
            update = () => {
                UpdateAnimatorSpeed();
                DoMovementStuff();
            },
            exit = () => _animator.speed = 1f,
        });

        _fsm.AddState(States.Flying, new State() {
            enter = () => _animator.Play("FlyingWalking"),
            update = () => {
                UpdateAnimatorSpeed();
                DoMovementStuff();
            },
            exit = () => _animator.speed = 1f,
        });

        _fsm.AddState(States.FollowingIdle, new State() {
            enter = () => {
                // Tell the bird set that we've been got!
                _birdSet.Add(_channel);

                // Disable the offset driver!
                _driveChannelByOffset.enabled = false;

                // Tell the fader to fade in all the way. 
                StartCoroutine(_channel.FadeIn(_fadeInTime));

                _animator.Play("Idle");
            },
            update = () => DoFollowStuff(),
        });

        _fsm.AddState(States.FollowingWalking, new State() {
            enter = () => _animator.Play("Walking"),
            update = () => {
                UpdateAnimatorSpeed();
                DoFollowStuff();
            },
            exit = () => _animator.speed = 1f,
        });

        _fsm.AddState(States.FollowingFlying, new State() {
            enter = () => _animator.Play("FlyingWalking"),
            update = () => {
                UpdateAnimatorSpeed();
                DoFollowStuff();
            },
            exit = () => _animator.speed = 1f,
        });

        _fsm.AddState(States.Disabled, new State());
        
        _fsm.AddTransition(States.Idle, States.Walking, Triggers.Moving);
        _fsm.AddTransition(States.Idle, States.Flying, Triggers.Airborne);
        _fsm.AddTransition(States.Idle, States.FollowingIdle, Triggers.FollowingTarget);

        _fsm.AddTransition(States.Walking, States.Idle, Triggers.NotMoving);
        _fsm.AddTransition(States.Walking, States.Flying, Triggers.Airborne);
        _fsm.AddTransition(States.Walking, States.FollowingIdle, Triggers.FollowingTarget);

        _fsm.AddTransition(States.Flying, States.Idle, Triggers.Grounded);
        _fsm.AddTransition(States.Flying, States.FollowingIdle, Triggers.FollowingTarget);

        _fsm.AddTransition(States.FollowingIdle, States.FollowingWalking, Triggers.Moving);
        _fsm.AddTransition(States.FollowingIdle, States.FollowingFlying, Triggers.Airborne);

        _fsm.AddTransition(States.FollowingWalking, States.FollowingIdle, Triggers.NotMoving);
        _fsm.AddTransition(States.FollowingWalking, States.FollowingFlying, Triggers.Airborne);

        _fsm.AddTransition(States.FollowingFlying, States.FollowingIdle, Triggers.Grounded);

        _fsm.AddTransitionAll(States.Disabled, Triggers.Disable, false);
    }

    void DoFollowStuff() {
        var targetPos = _handle.TryGetPosition(out Vector3 pos) ? pos : transform.position;
        var delta = targetPos - transform.position;


        var movementVec = delta.normalized * _speed;
        bool movementLarger = movementVec.sqrMagnitude > delta.sqrMagnitude;

        // Calcaulte move direction. 
        _moveDirection =  movementLarger ? delta : movementVec;
        _moveDirection.y -= _gravity * Time.deltaTime;

        // Drive the heading 
        _childDisplay.transform.forward = delta.normalized;

        // Move the stuff. 
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    void DoMovementStuff() {
        // // Apply _gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // // when the _moveDirection is multiplied by deltaTime). This is because _gravity should be applied
        // // as an acceleration (ms^-2)
        if(_characterController.isGrounded) {
            _moveDirection.y = 0f;
        }
        _moveDirection.y -= _gravity * Time.deltaTime;

        // // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    void Update() {
        if(_characterController.isGrounded) {_fsm.SetTrigger(Triggers.Grounded); }
        else {_fsm.SetTrigger(Triggers.Airborne); }

        var nonVerticalMovemet = NonVerticalMovement;
        if(nonVerticalMovemet.magnitude > 0.01) {_fsm.SetTrigger(Triggers.Moving); }
        else {_fsm.SetTrigger(Triggers.NotMoving); }

        _fsm.Update(Time.deltaTime);
    }

}