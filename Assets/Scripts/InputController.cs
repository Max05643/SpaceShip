using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    public Vector2 GetDirectionInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = -Input.GetAxis("Vertical");
        return new Vector2(horizontal, vertical);
    }

    public bool GetActionInput()
    {
        return Input.GetAxis("Fire") > 0;
    }

}
