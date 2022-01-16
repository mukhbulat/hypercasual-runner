using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator _animator;
    
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    
    private float _animationSpeed;
    //
    private readonly float _speed = 7f;
    private readonly float _acceleration = 0.1f;
    private readonly float _jumpHeight = 12f;
    private readonly float _gravity = -20.8f;
    [SerializeField] private AnimationCurve speedIncrease;
    [SerializeField] private AnimationCurve speedAtMaxSpeed;
    
    
    private float _yVelocity;
    private bool _isGrounded;

    private void Update()
    {
        _isGrounded = characterController.isGrounded;
        
        if (_isGrounded && _yVelocity < 0)
        {
            _yVelocity = 0;
        }
        characterController.Move(Vector3.forward * _speed * Time.deltaTime);
        if (_isGrounded && Input.GetMouseButtonDown(0))
        {
            _yVelocity += Mathf.Sqrt(_jumpHeight * -1 * _gravity);
        }

        _yVelocity += _gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, _yVelocity * Time.deltaTime, 0));
        HandleAnimator();
    }

    private void HandleAnimator()
    {
        if (_isGrounded)
        {
            _animator.SetFloat(VerticalVelocity, 0.5f);
        }
        else if (_yVelocity > 0.1f)
        {
            _animator.SetFloat(VerticalVelocity, 1f);
        }
        else
        {
            _animator.SetFloat(VerticalVelocity, 0f);
        }
    }
}
