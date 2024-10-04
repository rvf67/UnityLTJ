using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    CharacterController characterController;
    PlayerInputActions inputActions;
    public float moveSpeed = 4.0f;
    public Vector3 direction = Vector3.zero;
    public Vector3 Direction
    {
        get => direction;
        set
        {
            direction = value;
        }
    }
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }
    private void Update()
    {
        characterController.Move(Time.deltaTime*moveSpeed*direction);
        transform.LookAt(transform.position+direction);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Dodge.performed += OnDodge;
    }


    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input =context.ReadValue<Vector2>();
        direction = new Vector3(input.x,0f,input.y);
    }


    private void OnDodge(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}
