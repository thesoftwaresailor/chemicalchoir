
using UnityEngine;

class DropPlane : MonoBehaviour
{

    public static DropPlane instance;

    public void Start()
    {
        instance = this;
    }

    public void Show()
    {
        gameObject.layer = 3;
    }

    public void Hide()
    {
        gameObject.layer = 2;
    }

}

