using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ResourceTooltip : MonoBehaviour
{

    public static ResourceTooltip instance;

    public TextMeshProUGUI resourceName;

    public TextMeshProUGUI resourceDescription;

    public Image tooltip;

    public Camera camera;

    private Resource resource;

    public Vector3 offset;

    public RectTransform parentTransform;

    public void ShowTooltip(Resource resource)
    {
        this.resource = resource;
        tooltip.enabled = true;
        resourceName.enabled = true;
        resourceDescription.enabled = true;
    }

    public void HideTooltip()
    {
        tooltip.enabled = false;
        resourceName.enabled = false;
        resourceDescription.enabled = false;
        this.resource = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (tooltip.enabled)
        {
            if (resource)
            {
                resourceName.text = resource.resourceName;
                double micro = Math.Round(resource.MicroPhase, 1);
                double macro = Math.Round(resource.MacroPhase, 1);
                resourceDescription.text = "Minor Phase: " + micro + "\nMajor Phase: " + macro;
                parentTransform.position = resource.gameObject.transform.position + offset;
                parentTransform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
            }
            else
            {
                tooltip.enabled = false;
                resourceName.enabled = false;
                resourceDescription.enabled = false;
            }
        }
    }
}
