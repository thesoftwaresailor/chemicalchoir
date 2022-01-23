﻿
using System;
using UnityEngine;

class Resource : MonoBehaviour
{
    private bool isMatA = true;

    public Material matA;
    public Material matB;
    public Renderer renderer;
    public Rigidbody rigidbody;

    public int macro;
    public int micro;
    public string name;

    public float microTimer;
    public float macroTimer;
    public float microPhase;
    public float macroPhase;

    private int layerMask;
    private int resourceMask;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        resourceMask = 1 << LayerMask.NameToLayer("Resources");
    }

    private void FlipMicro()
    {
        renderer.material = isMatA ? matA : matB;
        isMatA = !isMatA;
        micro = micro == 0 ? 1 : 0;
    }

    private void FlipMacro()
    {
        macro = macro == 0 ? 1 : 0;
    }

    public Diocese Diocese {get;set;}

    private void Update()
    {
        if (Diocese)
        {
            microPhase -= Time.deltaTime * Diocese.CurrentTimeScale;
            if (microPhase <= 0)
            {
                FlipMicro();
                microPhase = microTimer;
            }
            macroPhase -= Time.deltaTime * Diocese.CurrentTimeScale;
            if (macroPhase <= 0)
            {
                macroPhase = macroTimer;
                FlipMacro();
            }
        }

        if (isBeingDragged)
        {
            RaycastHit result;
            if(CastFromScreenAtMouse(out result))
                transform.position = result.point;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Mouse up");
                isBeingDragged = false;
                rigidbody.isKinematic = false;
                RaycastHit[] resources = CastFromScreenAtMouseForResource();
                Debug.Log(resources.ToString());
                Debug.Log(resources[0]);
                if (resources.Length >= 2)
                {
                    Collider two = resources[1].collider;
                    if(two == this.gameObject)
                    {
                        two = resources[0].collider;
                    }
                    Crafting.instance.Craft(this, two.gameObject.GetComponent<Resource>());
                }
            }
        }
    }

    private bool isBeingDragged;

    private void OnMouseDown()
    {
        isBeingDragged = true;
        rigidbody.isKinematic = true;
    }

    private bool CastFromScreenAtMouse(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool didHit = Physics.Raycast(ray, out hit, 5f, layerMask: layerMask);
        return didHit;
    }

    private RaycastHit[] CastFromScreenAtMouseForResource()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, 5f, resourceMask);
        return hits;
    }

}
