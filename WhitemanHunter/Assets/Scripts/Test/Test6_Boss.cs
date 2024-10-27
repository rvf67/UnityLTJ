using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test6_Boss : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetBossRock(transform.position,transform.forward);
    }
}
