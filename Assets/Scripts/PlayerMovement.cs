using System.Collections;
using Game;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IRestartable
{
    #region General
    
    // Components
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats playerStats;
    
    // Animator Parameters
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    private static readonly int StumbleTrigger = Animator.StringToHash("StumbleTrigger");
    private static readonly int InGame = Animator.StringToHash("InGame");
    private static readonly int ClimbTrigger = Animator.StringToHash("ClimbTrigger");
    private static readonly int HitHeadTrigger = Animator.StringToHash("HitHeadTrigger");
    private static readonly int GotDownTrigger = Animator.StringToHash("GotDownTrigger");
    private static readonly int CrouchTrigger = Animator.StringToHash("CrouchTrigger");
    // Enable/Disable movement.
    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            animator.SetBool(InGame, value);
        }
    }
    
    // Data for restart.
    private Vector3 _startingPosition;
    
    
    private void Awake()
    {
        // For restarting purposes.
        _startingPosition = transform.position;

        // For obstacle bypassing.
        _playerHeight = characterController.height;
        _playerRadius = characterController.radius;
        _rayOffset = new Vector3(0, _playerHeight - _delta, _playerRadius + _delta);
        
        // For crouching
        _originalHeight = characterController.height;

        _crouchHeight = _originalHeight / 2;
        
        _acceleration = speedIncrease.Evaluate(0);
        
        StartCoroutine(AcceleratingToMaxSpeed());
        
    }
    
    private void Update()
    {
        if (IsEnabled)
        {
            Movement();
            
            HandleObstacleBypass();
            
            HandleAnimatorTree();
        }
    }
    
    // Two coroutines for acceleration.
    private IEnumerator AcceleratingToMaxSpeed()
    {
        while (_currentTime < timeToGetFullSpeed)
        {
            _currentTime += Time.deltaTime;
            _acceleration = speedIncrease.Evaluate(_currentTime / timeToGetFullSpeed);
            yield return null;
        }

        _currentTime = 0;
        StartCoroutine(SpeedChangeOnMaxSpeed());
    }

    private IEnumerator SpeedChangeOnMaxSpeed()
    {
        while (IsEnabled)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > timeOfMaxSpeedLoop)
            {
                _currentTime -= timeOfMaxSpeedLoop;
            }

            _acceleration = speedAtMaxSpeed.Evaluate(_currentTime / timeOfMaxSpeedLoop);

            yield return null;
        }
    }

    
    private void HandleAnimatorTree()
    {
        if (_isGrounded)
        {
            animator.SetFloat(VerticalVelocity, 0.5f);
        }
        else if (_yVelocity > 0.1f)
        {
            animator.SetFloat(VerticalVelocity, 1f);
        }
        else
        {
            animator.SetFloat(VerticalVelocity, 0f);
        }
    }
    
    private IEnumerator LoseCoroutine()
    {
        yield return new WaitForSeconds(3);
        animator.SetBool(GotDownTrigger, false);
        playerStats.Health = 0;
    }

    public void Restart()
    {
        transform.position = _startingPosition;
        animator.SetBool(InGame, true);
        _acceleration = speedIncrease.Evaluate(0);
        
        
        _isCrouching = false;
        characterController.height = _originalHeight;
        
        StopAllCoroutines();
        StartCoroutine(AcceleratingToMaxSpeed());
    }
    
    #endregion

    #region Movement

    
    // Values to tweak.
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private AnimationCurve speedIncrease;
    [SerializeField] private AnimationCurve speedAtMaxSpeed;
    [SerializeField] private float timeToGetFullSpeed = 60f;
    [SerializeField] private float timeOfMaxSpeedLoop = 10f;
    
    // Changing values.
    private readonly float _speed = 15f;
    private float _acceleration;
    private readonly float _gravity = -20.8f;
    private float _currentTime;
    
    private float _yVelocity;
    private bool _isGrounded;
    
    
    // Crouch
    private float _crouchTime = 0.5f;
    private bool _isCrouching = false;

    private float _originalHeight;
    private float _crouchHeight;
    
    private void Movement()
    {
        _isGrounded = characterController.isGrounded;

        if (_isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0;
        }
        
        
        characterController.Move(Vector3.forward * _speed * _acceleration * Time.deltaTime);
        
        Jump();

        Crouch();

        VerticalMoving();
    }


    private void Jump()
    {
        if (_isGrounded && Input.GetMouseButtonDown(0))
        {
            _yVelocity += Mathf.Sqrt(jumpHeight * -1 * _gravity);
        }
    }
    
    private void Crouch()
    {
        if (_isCrouching)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(CrouchCoroutine());
        }
    }

    private IEnumerator CrouchCoroutine()
    {
        _isCrouching = true;
        while (!_isGrounded)
        {
            _yVelocity = _gravity;
            yield return null;
        }

        if (IsEnabled)
        {
            characterController.height = _crouchHeight;
            
            animator.SetTrigger(CrouchTrigger);
        }

        yield return new WaitForSeconds(_crouchTime);

        _isCrouching = false;
        characterController.height = _originalHeight;
    }

    private void VerticalMoving()
    {
        
        _yVelocity += _gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, _yVelocity * Time.deltaTime, 0));

    }

    #endregion

    #region Barriers and obstacles

    
    // Obstacles bypassing
    private float _playerHeight;
    private Vector3 _rayOffset;

    private float _timeSinceLastObstacle = 0;
    private float _timeToObstacleWork = 0.2f;

    private float _playerRadius;
    private int _platformsMask = 1 << 6;
    
    // Just a little value for ray offset.
    private float _delta = 0.05f;

    // Ratio on how strongly stumble will influence _yVelocity.
    private float _bypassDistanceToVelocityRatio = 1.4f;
    
    
    private void HandleObstacleBypass()
    {
        // Disgusting but easy solution.
        if (_isCrouching) return;
        
        if (!_isGrounded)
        {
            if (_timeSinceLastObstacle < _timeToObstacleWork)
            {
                _timeSinceLastObstacle += Time.deltaTime;
                return;
            }
            RaycastHit hit;
            Ray ray = new Ray(transform.position + _rayOffset, Vector3.down);
            
            if (Physics.Raycast(ray, out hit, _playerHeight - _delta, _platformsMask))
            {
                float distance = hit.distance - (_playerHeight - _delta * 2) / 2;
                // Check the hit distance and respond accordingly
                int animatorHash = distance < 0 ? HitHeadTrigger : StumbleTrigger;
                
                if (Mathf.Abs(distance) < 0.25f * _playerHeight)
                {
                    animator.SetTrigger(animatorHash);
                    
                    _yVelocity += distance * _bypassDistanceToVelocityRatio;
                    _timeSinceLastObstacle = 0;
                }
                else if (Mathf.Abs(distance) < 0.35f * _playerHeight)
                {
                    animator.SetTrigger(animatorHash);

                    _yVelocity += distance * _bypassDistanceToVelocityRatio;
                    _timeSinceLastObstacle = 0;
                }
                else
                {
                    _yVelocity += distance * _bypassDistanceToVelocityRatio;
                }
                
            }
        }
    }

    public void BarrierHit()
    {
        _isEnabled = false;
        animator.SetTrigger(GotDownTrigger);
        
        characterController.height = _crouchHeight;
        _yVelocity = _gravity;
        VerticalMoving();
        StartCoroutine(LoseCoroutine());
    }

    #endregion
}