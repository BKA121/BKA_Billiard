using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    // Tra ve do lech cua chuot trong khoang (-1, 1) bieu thi di chuyen trai phai
    public float GetHorizontalAxis()
    {
        return Input.GetAxis("Mouse X");
    }
}
