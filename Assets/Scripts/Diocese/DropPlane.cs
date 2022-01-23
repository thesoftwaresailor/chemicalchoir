
using UnityEngine;

class DropPlane : MonoBehaviour
{

    public static DropPlane instance;

    public void Start()
    {
        instance = this;
    }

}

