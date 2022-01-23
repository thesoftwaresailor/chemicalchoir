using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceTooltip : MonoBehaviour
{

    public static ResourceTooltip instance;

    public TextMeshProUGUI tooltip;

    public Camera camera;

    private Resource resource;

    public Vector3 offset;

    public void ShowTooltip(Resource resource)
    {
        this.resource = resource;
        tooltip.enabled = true;
    }

    public void HideTooltip()
    {
        tooltip.enabled = false;
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
                tooltip.text = "Resource: \n" + resource.name;
                transform.position = resource.gameObject.transform.position + offset;
                transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
            }
            else
            {
                tooltip.enabled = false;
            }
        }
    }
}
