using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectiveCraftings : MonoBehaviour
{
    // Start is called before the first frame update

    public static List<ObjectiveCraftings> objectiveRegister => ObjectiveRegister;
    private static List<ObjectiveCraftings> ObjectiveRegister = new List<ObjectiveCraftings>();

    private static int objectiveCount = 4;

    public string objectiveName;

    private static readonly float MOUSE_FOLLOW_STRENGTH = 50f;
    private int layerMask;
    private int resourceMask;

    [FormerlySerializedAs("rigidbody")]
    public Rigidbody rigidBody;

    private float initialMass;
    private Vector3 targetPoint;

    private bool isBeingDragged;

    void Start()
    {
        bool alreadyCrafted = false;
        foreach(ObjectiveCraftings crafted in ObjectiveRegister)
        {
            if (crafted.objectiveName == objectiveName)
                alreadyCrafted = true;
        }
        if (!alreadyCrafted)
            ObjectiveRegister.Add(this);
        if (ObjectiveRegister.Count == objectiveCount)
            GameOver.instance.DoGameOverStuff();
        layerMask = 1 << LayerMask.NameToLayer("Drop");
        initialMass = rigidBody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingDragged)
        {
            if (CastFromScreenAtMouse(out RaycastHit result))
                targetPoint = result.point;

            Vector3 displacement = targetPoint - transform.position;
            rigidBody.velocity = displacement.normalized * MOUSE_FOLLOW_STRENGTH * Mathf.Clamp(displacement.magnitude, 0f, 1f);

            if(Input.GetMouseButtonUp(0))
            {
                ToggleDragState(false);
                DropPlane.instance.Hide();
            }
        }
    }
    private void OnMouseDown()
    {
        ToggleDragState(true);
        DropPlane.instance.Show();
    }
    private void OnMouseEnter()
    {
        ObjectiveTooltip.instance.ShowTooltip(this);
    }

    private void OnMouseExit()
    {
        ObjectiveTooltip.instance.HideTooltip();
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

}
