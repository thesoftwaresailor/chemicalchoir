

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    private static readonly float MOUSE_FOLLOW_STRENGTH = 50f;

    private bool isMatA = true;

    public static List<Resource> ResourceRegister => _ResourceRegister;
    private static List<Resource> _ResourceRegister = new List<Resource>();

    public SpriteRenderer selectedCircle;

    public Material matA;
    public Material matB;
    [FormerlySerializedAs("renderer")]
    public Renderer resourceRenderer;
    [FormerlySerializedAs("rigidbody")]
    public Rigidbody rigidBody;

    public int macro;
    public int micro;
    [FormerlySerializedAs("name")]
    public string resourceName;

    public float microTimer;
    public float macroTimer;

    private float microPhase;
    private float macroPhase;

    private int layerMask;
    private int resourceMask;

    private float initialMass;
    private Vector3 targetPoint;

    public float MicroPhase => microPhase;
    public float MacroPhase => macroPhase;

    private void Start()
    {
        _ResourceRegister.Add(this);
        layerMask = 1 << LayerMask.NameToLayer("Drop");
        resourceMask = 1 << LayerMask.NameToLayer("Resources");
        initialMass = rigidBody.mass;
    }

    private void FlipMicro()
    {
        resourceRenderer.material = isMatA ? matA : matB;
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
            if(CastFromScreenAtMouse(out RaycastHit result))
            {
                targetPoint = result.point;
            }

            Vector3 displacement = targetPoint - transform.position;
            rigidBody.velocity = displacement.normalized * MOUSE_FOLLOW_STRENGTH * Mathf.Clamp(displacement.magnitude, 0f, 1f);

            if (Input.GetMouseButtonUp(0))
            {
                ToggleDragState(false);
                HideSelectableImage();
                DropPlane.instance.Hide();
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
        ToggleDragState(true);
        DropPlane.instance.Show();
        ShowSelectableImage();
    }

    private void OnMouseEnter()
    {
        ResourceTooltip.instance.ShowTooltip(this);
    }

    private void OnMouseExit()
    {
        ResourceTooltip.instance.HideTooltip();
    }

    private void ToggleDragState(bool isOn)
    {
        isBeingDragged = isOn;
        rigidBody.mass = isOn ? 50f : initialMass;
        rigidBody.useGravity = !isOn;

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

    private void ShowSelectableImage()
    {
        foreach(Resource resource in _ResourceRegister)
        {
            if(resource != this)
            {
                resource.selectedCircle.enabled = true;
            }
        }
    }

    private void HideSelectableImage()
    {
        foreach (Resource resource in _ResourceRegister)
        {
            if (resource != this)
            {
                resource.selectedCircle.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        _ResourceRegister.Remove(this);
    }

}
