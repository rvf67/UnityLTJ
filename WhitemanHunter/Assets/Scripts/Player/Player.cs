using UnityEngine;

[RequireComponent(typeof(PlayerInputController), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    // ������Ʈ ��
    CharacterController characterController;
    PlayerInputController inputController;
    PlayerMovement movement;
    PlayerAttack attack;
    PlayerInteraction interaction;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        inputController = GetComponent<PlayerInputController>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        interaction = GetComponent<PlayerInteraction>();
        inputController.onMove += movement.SetDirection;
        inputController.onMoveModeChange += movement.ToggleMoveMode;
        inputController.onDodge += movement.Dodge;
        inputController.onAttack += attack.OnAttackInput;
        inputController.onAttackEnd += attack.OnAttackEndInput;
        inputController.onInteraction += interaction.Interact;
        inputController.onSwap += interaction.Swap;
        inputController.onReload += movement.Reload;
    }
}
