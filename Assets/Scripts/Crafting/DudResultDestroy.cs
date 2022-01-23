using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudResultDestroy : MonoBehaviour
{

    private float lifetime = 3;
    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
    }
}
