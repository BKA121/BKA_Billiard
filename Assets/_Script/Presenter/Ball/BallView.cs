using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private int ballID; 
    public int BallID => ballID;

    public void Render(Vector3 localPos, bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive)
        {
            transform.localPosition = localPos;
        }
    }
}
