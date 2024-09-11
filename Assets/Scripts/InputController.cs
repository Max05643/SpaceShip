using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    /// <summary>
    /// Returns the direction input from the player
    /// </summary>
    public Vector2 GetDirectionInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = -Input.GetAxis("Vertical");
        return new Vector2(horizontal, vertical);
    }

    /// <summary>
    /// Returns the action input from the player
    /// </summary>

    public bool GetActionInput()
    {
        return Input.GetAxis("Fire") > 0;
    }

}
