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

    // Ham kiem tra yeu cau dat bi
    public bool IsBallInHandAction()
    {
        return Input.GetKeyDown(KeyCode.M);
    }

    // Ham kiem tra yeu cau thoat van dau
    public bool IsExitMatch()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    // Ham kiem tra bat tat controls
    public bool IsShowControls()
    {
        return Input.GetKeyDown(KeyCode.T);
    }
    public bool IsHideControls()
    {
        return Input.GetKeyUp(KeyCode.T);
    }

    // Ham them xoay bi
    public bool IsAddSpinAction()
    {
        return Input.GetKey(KeyCode.E);
    }

    public bool IsZoomActionInOverView()
    {
        return Input.GetMouseButton(0);
    }
}
