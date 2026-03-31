using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    // Ham di chuyen trai phai, len xuong cua chuot
    public float GetHorizontalAxis()
    {
        return Input.GetAxis("Mouse X");
    }
    public float GetVerticalAxis()
    {
        return Input.GetAxis("Mouse Y");
    }

    // Ham kiem tra danh
    public bool IsShootAction()
    {
        return Input.GetKeyDown(KeyCode.S);
    }
    public bool IsExitShootAction()
    {
        return Input.GetKeyUp(KeyCode.S);
    }

    // Ham kiem tra chuyen doi goc nhin
    public bool IsSwitchViewPressed()
    {
        return Input.GetKeyDown(KeyCode.V);
    }
}
