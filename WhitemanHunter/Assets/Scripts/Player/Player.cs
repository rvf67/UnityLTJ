using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    // ÄÄÆ÷³ÍÆ® µé
    CharacterController characterController;

    PlayerInputController inputController;
    PlayerMovement movement;
    PlayerAttack attack;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        inputController = GetComponent<PlayerInputController>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        inputController.onMove += movement.SetDirection;
        inputController.onMoveModeChange += movement.ToggleMoveMode;
        inputController.onDodge += movement.Dodge;
        inputController.onAttack += attack.OnAttackInput;
    }
}
