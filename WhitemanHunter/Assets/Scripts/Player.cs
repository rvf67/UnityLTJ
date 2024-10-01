using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector3 direction { get; private set; }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input =context.ReadValue<Vector2>();
        direction = new Vector3(input.x,0f,input.y);
    }
}
