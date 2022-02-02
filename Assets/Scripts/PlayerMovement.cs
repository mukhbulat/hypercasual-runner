using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private AnimationCurve speedIncrease;
    [SerializeField] private AnimationCurve speedAtMaxSpeed;
    [SerializeField] private float timeToGetFullSpeed = 60f;
    [SerializeField] private float timeOfMaxSpeedLoop = 10f;
    //
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    
    private readonly float _speed = 15f;
    private float _acceleration;
    private readonly float _gravity = -20.8f;
    private float _currentTime;
    
    private float _yVelocity;
    private bool _isGrounded;

    private void Update()
    {
        _isGrounded = characterController.isGrounded;
        
        if (_isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0;
        }
        characterController.Move(Vector3.forward * _speed * _acceleration * Time.deltaTime);
        if (_isGrounded && Input.GetMouseButtonDown(0))
        {
            _yVelocity += Mathf.Sqrt(jumpHeight * -1 * _gravity);
        }

        _yVelocity += _gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, _yVelocity * Time.deltaTime, 0));
        HandleAnimator();
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
}
