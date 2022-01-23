
using System;
using UnityEngine;

public class Resource : MonoBehaviour
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
        layerMask = 1 << LayerMask.NameToLayer("Drop");
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
                isBeingDragged = false;
                rigidbody.isKinematic = false;
                DropPlane.instance.gameObject.layer = 2;
                RaycastHit[] resources = CastFromScreenAtMouseForResource();
                if (resources.Length >= 2)
                {
                    Collider two = resources[0].collider;
                    int x = 0;
                    while(two.gameObject == this.gameObject && x < resources.Length)
                    {
                        x++;
                        two = resources[x].collider;
                    }
                    if(x < resources.Length)
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
        DropPlane.instance.gameObject.layer = 3;
    }

    private void OnMouseEnter()
    {
        ResourceTooltip.instance.ShowTooltip(this);
    }

    private void OnMouseExit()
    {
        ResourceTooltip.instance.HideTooltip();
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
