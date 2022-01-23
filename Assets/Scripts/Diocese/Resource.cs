using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Resource : MonoBehaviour
{
    private static readonly float MOUSE_FOLLOW_STRENGTH = 18f;
    private static readonly string MATERIAL_EMISSION_NAME = "Color_6bb548e152674a10b20db0483b8b423c";
    private static readonly string MATERIAL_COLOR_NAME = "Color_d9dbbc69cde44ff6ae084f38082421d4";
    private static readonly string MATERIAL_SPECIAL_NAME = "Vector1_c885be47250f45b19ac91272cf04b7e7";

    public static List<Resource> ResourceRegister => _ResourceRegister;
    private static List<Resource> _ResourceRegister = new List<Resource>();

    public AudioSource collisionSound;

    public SpriteRenderer selectedCircle;
    public Color destinationColor = Color.white;

    private float visualTransitionTimer = 0.1f;

    [FormerlySerializedAs("renderer")]
    public Renderer resourceRenderer;
    [FormerlySerializedAs("rigidbody")]
    public Rigidbody rigidBody;

    public int macro;
    public int micro;
    [FormerlySerializedAs("name")]
    public string resourceName;
    public PhaseDescription description;
    public float microTimer;
    public float macroTimer;

    private float microPhase;
    private float macroPhase;

    private float spikeTransitionPhase;
    private float colorTransitionPhase;
    private Color startColor;
    private Color startEmissionColor;

    private int layerMask;
    private int resourceMask;

    private Vector3 targetPoint;

    private bool isBeingDragged;

    public float MicroPhase => microPhase;
    public float MacroPhase => macroPhase;

    private void Start()
    {
        startColor = resourceRenderer.material.GetColor(MATERIAL_COLOR_NAME);
        startEmissionColor = resourceRenderer.material.GetColor(MATERIAL_EMISSION_NAME);

        _ResourceRegister.Add(this);
        layerMask = 1 << LayerMask.NameToLayer("Drop");
        resourceMask = 1 << LayerMask.NameToLayer("Resources");
        microPhase = microTimer;
        macroPhase = macroTimer;
    }

    private void FlipMicro()
    {
        spikeTransitionPhase = visualTransitionTimer;
        micro = micro == 0 ? 1 : 0;
    }

    private void FlipMacro()
    {
        colorTransitionPhase = visualTransitionTimer;
        macro = macro == 0 ? 1 : 0;
    }

    public Diocese Diocese {get;set;}

    private void Update()
    {
        float scaledDelta = Time.deltaTime;

        if (Diocese)
        {
            scaledDelta *= Diocese.CurrentTimeScale;

            microPhase -= scaledDelta;
            if (microPhase <= 0)
            {
                FlipMicro();
                microPhase = microTimer;
            }
            macroPhase -= scaledDelta;
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
            rigidBody.velocity = displacement.normalized * MOUSE_FOLLOW_STRENGTH * Mathf.Clamp(displacement.magnitude, 0f, 0.5f);

            if (Input.GetMouseButtonUp(0))
            {
                ToggleDragState(false);
                HideSelectableImage();
                Crafting.instance.HideResource();
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

        if (colorTransitionPhase > 0)
        {
            colorTransitionPhase -= scaledDelta;
            colorTransitionPhase = Mathf.Max(0f, colorTransitionPhase);

            float amount = (colorTransitionPhase / visualTransitionTimer);
            if (macro == 1)
            {
                amount = 1f - amount;
            }

            Color scaledResult = Color.Lerp(startColor, destinationColor, amount);
            Color scaledResultEmission = Color.Lerp(startEmissionColor, destinationColor, amount);
            resourceRenderer.material.SetColor(MATERIAL_COLOR_NAME, scaledResult);
            resourceRenderer.material.SetColor(MATERIAL_EMISSION_NAME, scaledResultEmission);
        }

        if (spikeTransitionPhase > 0)
        {
            spikeTransitionPhase -= scaledDelta;
            spikeTransitionPhase = Mathf.Max(0f, spikeTransitionPhase);

            float amount = (spikeTransitionPhase / visualTransitionTimer);
            if (micro == 1)
            {
                amount = 1f - amount;
            }

            resourceRenderer.material.SetFloat(MATERIAL_SPECIAL_NAME, amount);
        }
    }

    private void OnMouseDown()
    {
        ToggleDragState(true);
        DropPlane.instance.Show();
        ShowSelectableImage();
        Crafting.instance.DisplayResource(this);
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

    // Sound
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionSound != null && collisionSound.isPlaying == false && IsAResource(collision.gameObject))
        {
            collisionSound.Play();
        }
    }

    private bool IsAResource(GameObject obj)
    {
        return obj.layer == 6 || obj.layer == 7;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source)
        {
            collisionSound = source;
        }
    }
#endif

    private void OnDestroy()
    {
        _ResourceRegister.Remove(this);
    }

}

[System.Serializable]
public struct PhaseDescription
{
    public string[] minorPhase;
    public string[] majorPhase;
}
