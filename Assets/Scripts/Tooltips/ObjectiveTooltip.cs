using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTooltip : MonoBehaviour
{

    public static ObjectiveTooltip instance;

    public TextMeshProUGUI resourceName;

    public TextMeshProUGUI resourceDescription;

    public Image tooltip;

    public Camera mainCamera;

    private ObjectiveCraftings objective;

    public Vector3 offset;

    public RectTransform parentTransform;

    public void ShowTooltip(ObjectiveCraftings objective)
    {
        this.objective = objective;
        tooltip.enabled = true;
        resourceName.enabled = true;
        resourceDescription.enabled = true;
    }

    public void HideTooltip()
    {
        tooltip.enabled = false;
        resourceName.enabled = false;
        resourceDescription.enabled = false;
        this.objective = null;
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
            if(objective)
            {
                resourceName.text = objective.objectiveName;
                resourceDescription.text = "Objectives Found: " + ObjectiveCraftings.objectiveRegister.Count;
                parentTransform.position = objective.gameObject.transform.position + offset;
                parentTransform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
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
