using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour, IRestartable
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats playerStats;
    
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private AnimationCurve speedIncrease;
    [SerializeField] private AnimationCurve speedAtMaxSpeed;
    [SerializeField] private float timeToGetFullSpeed = 60f;
    [SerializeField] private float timeOfMaxSpeedLoop = 10f;
    //
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    private static readonly int StumbleTrigger = Animator.StringToHash("StumbleTrigger");
    //
    private readonly float _speed = 15f;
    private float _acceleration;
    private readonly float _gravity = -20.8f;
    private float _currentTime;
    
    private float _yVelocity;
    private bool _isGrounded;
    // Data for restart.
    private Vector3 _startingPosition;
    

    // Obstacles bypassing
    private readonly float _playerHeight = 1.8f;
    private Vector3 _rayOffset;

    // Ratio on how strongly stumble will influence _yVelocity.
    private float _stumbleDistanceToVelocity = 1f;

    private void Update()
    {
        _isGrounded = characterController.isGrounded;
        
        
        if (_isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0;
        }
        
        // Moving forward with given speed and acceleration
        characterController.Move(Vector3.forward * _speed * _acceleration * Time.deltaTime);
        
        Jump();
        
        HandleObstacleBypass();

        VerticalMoving();
        
        HandleAnimator();
    }

    private void Jump()
    {
        if (_isGrounded && Input.GetMouseButtonDown(0))
        {
            _yVelocity += Mathf.Sqrt(jumpHeight * -1 * _gravity);
        }

    }

    private void VerticalMoving()
    {
        
        _yVelocity += _gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, _yVelocity * Time.deltaTime, 0));

    }
    
    private void HandleObstacleBypass()
    {
        if (!_isGrounded)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position + _rayOffset, Vector3.down);
            
            if (Physics.Raycast(ray, out hit, _playerHeight))
            {
                float distance = hit.distance - _playerHeight / 2;
                // Check the hit distance and respond accordingly
                if (Mathf.Abs(distance) < 0.25f * _playerHeight)
                {
                    animator.SetTrigger(StumbleTrigger);
                    
                    playerStats.Health -= 2;
                    _yVelocity += distance * _stumbleDistanceToVelocity;
                }
                else if (Mathf.Abs(distance) < 0.35f * _playerHeight)
                {
                    animator.SetTrigger(StumbleTrigger);
                    
                    playerStats.Health -= 1;
                    _yVelocity += distance * _stumbleDistanceToVelocity;
                }
                else
                {
                    _yVelocity += distance * _stumbleDistanceToVelocity;
                }
                
            }
        }
    }

    private void HandleAnimator()
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

    private void Awake()
    {
        // For restarting purposes
        _startingPosition = transform.position;
        // 0.5 is radius of collider, 0.52 - just slightly larger.
        _rayOffset = new Vector3(0, _playerHeight, 0.52f);
        
        _acceleration = speedIncrease.Evaluate(0);
        
        StartCoroutine(AcceleratingToMaxSpeed());
    }

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
        while (true)
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

    public void Restart()
    {
        transform.position = _startingPosition;
        _acceleration = speedIncrease.Evaluate(0);
        
        StopAllCoroutines();
        StartCoroutine(AcceleratingToMaxSpeed());
    }

    private void OnEnable()
    {
        animator.SetBool("InGame", true);
    }

    private void OnDisable()
    {
        
        animator.SetBool("InGame", false);
    }
}
