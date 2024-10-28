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
    /// 공격종료 전달 델리게이트
    /// </summary>
    public Action onAttackEnd;
    /// <summary>
    /// 회피 입력 전달 델이게이트
    /// </summary>
    public Action onDodge;
    /// <summary>
    /// 재장전 델리게이트
    /// </summary>
    public Action onReload;
    /// <summary>
    /// 상호작용 델리게이트
    /// </summary>
    public Action onInteraction;
    /// <summary>
    /// 무기스왑 델리게이트
    /// </summary>
    public Action<int> onSwap;
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
        inputActions.Player.Interaction.performed += OnInteraction;
        inputActions.Player.WeaponSwap.performed += OnSwap;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.Attack.canceled += OnattackEnd;
        inputActions.Player.Reload.performed += OnReload;
    }

    

    private void OnDisable()
    {
        inputActions.Player.Reload.performed -= OnReload;
        inputActions.Player.Attack.canceled -= OnattackEnd;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.WeaponSwap.performed -= OnSwap;
        inputActions.Player.Interaction.performed -= OnInteraction;
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
    /// 무기 스왑함수
    /// </summary>
    /// <param name="context"></param>
    private void OnSwap(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        int select= (int)context.ReadValue<float>();
        onSwap?.Invoke(select);
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
    /// 공격 종료함수
    /// </summary>
    /// <param name="context"></param>
    private void OnattackEnd(InputAction.CallbackContext context)
    {
        onAttackEnd?.Invoke();
    }
    private void OnReload(InputAction.CallbackContext context)
    {
        onReload?.Invoke();
    }
    /// <summary>
    /// 회피 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnDodge(InputAction.CallbackContext context)
    {
        onDodge?.Invoke();
    }

    /// <summary>
    /// 상호작용함수
    /// </summary>
    /// <param name="context"></param>
    private void OnInteraction(InputAction.CallbackContext context)
    {
        onInteraction?.Invoke();
    }

}
