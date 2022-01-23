using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{

    private Vector3 offset;

    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.parent.transform.position - transform.position;
        parentTransform = transform.parent.transform;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentTransform)
        {
            transform.LookAt(new Vector3(0, 50, 0));
            transform.position = parentTransform.position - offset;
        } else
        {
            Destroy(gameObject);
        }
    }
}
