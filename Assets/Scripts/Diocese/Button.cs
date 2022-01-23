using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("References")]
    public Diocese diocese;
    public MeshRenderer rend;

    [Header("Adjustables")]
    public Vector3 selectedIndent;
    public float positionLerpStrength = 10f;
    public float emissionStrengthIncrease = 5f;
    public float emissionLerpStrength = 10f;

    private bool selected;
    private Vector3 destinationPoint = new Vector3(0, -0.1f, 0);
    private float currentEmission = 1f;
    private float destinationEmission = 1f;
    private Color originalColor;

    private void Start()
    {
        destinationPoint = transform.position;
        originalColor = rend.material.GetColor("_EmissionColor");
    }

    private void Update()
    {
        transform.position += (destinationPoint - transform.position) * positionLerpStrength * Time.deltaTime;

        currentEmission += (destinationEmission - currentEmission) * emissionLerpStrength * Time.deltaTime;

        if (Mathf.Abs(currentEmission - destinationEmission) > 0.1f)
            rend.material.SetColor("_EmissionColor", originalColor * currentEmission);

        if (selected && Input.GetMouseButtonUp(0))
        {
            SelectButton();
        }
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectButton();
        }
    }

    private void SelectButton()
    {
        diocese.FlipTimeScale();

        selected = !selected;
        destinationPoint += selectedIndent * (selected ? -1 : 1);
        destinationEmission += emissionStrengthIncrease * (selected ? 1 : -1);
    }
}

