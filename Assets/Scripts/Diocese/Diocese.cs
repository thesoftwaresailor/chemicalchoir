using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diocese : MonoBehaviour
{

    public float fastTimeScale;

    public float slowTimeScale;

    private float currentTimeScale;

    private void Start()
    {
        currentTimeScale = slowTimeScale;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        GameObject gm = collision.gameObject;
        Resource resource = gm.GetComponent<Resource>();
        if (resource)
        {
            Debug.Log("Set timescale in resource");
            resource.Diocese = this;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject gm = collision.gameObject;
        Resource resource = gm.GetComponent<Resource>();
        if (resource)
            resource.Diocese = null;
    }

    public float CurrentTimeScale => currentTimeScale;

    public void FlipTimeScale()
    {
        currentTimeScale = currentTimeScale == fastTimeScale ? slowTimeScale : fastTimeScale;
    }
}
