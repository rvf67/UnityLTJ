using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 이동 입력을 전달하는 델리게이트(Vector2:이동방향, bool 누른 상황(true면 눌렀다))
    /// </summary>
    public Action<Vector2, bool> onMove;

    /// <summary>
    /// 이동 모드 변경 입력을 전달하는 델리게이트
    /// </summary>
    public Action onMoveModeChange;
    /// <summary>
    /// 공격입력 전달 델리게이트
    /// </summary>
    public Action onAttack;
    /// <summary>
    /// 회피 입력 전달 델이게이트
    /// </summary>
    public Action onDodge;

    // 인풋 액션 에셋
    PlayerInputActions inputActions;
    /// <summary>
    /// 공격 컴포넌트
    /// </summary>
    PlayerAttack attack;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Dodge.performed += OnDodge;
        //inputActions.Player.Attack.performed += OnAttack;

    }


    private void OnDisable()
    {
        //inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Dodge.performed -= OnDodge;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// 이동 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        onMove?.Invoke(input, !context.canceled);
    }

    /// <summary>
    /// 달리기 토글함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveModeChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        onMoveModeChange?.Invoke();
    }
    /// <summary>
    /// 공격함수
    /// </summary>
    /// <param name="context"></param>
    private void OnAttack(InputAction.CallbackContext context)
    {
        onAttack?.Invoke();
    }
    /// <summary>
    /// 회피 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnDodge(InputAction.CallbackContext context)
    {
        onDodge?.Invoke();
    }
}
