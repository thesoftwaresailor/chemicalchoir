
using System;
using UnityEngine;

class Resource : MonoBehaviour
{
    private bool isMatA = true;

    public Material matA;
    public Material matB;
    public Renderer renderer;
    public Rigidbody rigidbody;

    public DropPlane dropPlane;

    public float microTimer;
    public float macroTimer;
    public float microPhase;
    public float macroPhase;

    private int layerMask;

    private void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    }

    private void FlipMaterial()
    {
        renderer.material = isMatA ? matA : matB;
        isMatA = !isMatA;
    }

    public Diocese Diocese {get;set;}

    private void Update()
    {
        if (Diocese)
        {
            microPhase -= Time.deltaTime * Diocese.CurrentTimeScale;
            if (microPhase <= 0)
            {
                FlipMaterial();
                microPhase = microTimer;
            }
            macroPhase -= Time.deltaTime * Diocese.CurrentTimeScale;
            if (macroPhase <= 0)
            {
                macroPhase = macroTimer;
            }
        }

        if (isBeingDragged)
        {
            RaycastHit result = CastFromScreenAtMouse();
            transform.position = result.point;
        }
    }

    private bool isBeingDragged;

    private void OnMouseDown()
    {
        isBeingDragged = true;
        rigidbody.isKinematic = true;
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        rigidbody.isKinematic = false;
    }

    private RaycastHit CastFromScreenAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool didHit = Physics.Raycast(ray, out RaycastHit hit, 5f, layerMask: layerMask);
        if (didHit == false)
        {
            throw new Exception("Did not hit the drop plane");
        }
        return hit;
    }
}
