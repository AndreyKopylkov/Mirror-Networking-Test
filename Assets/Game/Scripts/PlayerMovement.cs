using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;

    private float _moveHorizontal;
    private float _moveVertical;
    private Vector3 _movementDirection;
    private bool _canMove;

    public bool CanMove { get => _canMove; set => _canMove = value; }

    private void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        DashInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Initialize()
    {
        _canMove = true;
    }

    private void HandleMovement()
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;
        Quaternion rotation = Quaternion.LookRotation(cameraForward);
            
        _movementDirection = (rotation * _playerInput.PrimaryMovementDirection).normalized;
    }

    private void Move()
    {
        if(!_canMove) return;
        
        // transform.position = transform.position + _movementVector3 * _moveSpeed;
        // _rigidbody.MovePosition(transform.position + _movementVector3 * _moveSpeed);
        _characterController.Move(_movementDirection * _moveSpeed);
    }

    private void DashInput()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
    }
    
    //Вызывается на клиенте - работает на сервере
    [Command]
    private void Dash()
    {
        Debug.Log("Sending this command to Server");
    }

    //Вызывается на сервере - работает на всех клиентах
    [ClientRpc]
    private void WorkInClients()
    {
        
    }
    
    //Вызывается на сервере - работает на клиенте
    [TargetRpc]
    private void WorkInTargetClient()
    {
        
    }
}