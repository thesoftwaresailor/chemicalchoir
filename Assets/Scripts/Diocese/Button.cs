using System;
using UnityEngine;

class Button : MonoBehaviour
{

    public Diocese diocese;

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            diocese.FlipTimeScale();
        }
    }
}

