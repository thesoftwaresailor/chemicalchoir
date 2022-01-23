using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diocese : MonoBehaviour
{

    public float fastTimeScale;

    public float slowTimeScale;

    private float currentTimeScale;

    private int spawnType = 0;

    private void Start()
    {
        currentTimeScale = slowTimeScale;
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject gm = collision.gameObject;
        Resource resource = gm.GetComponent<Resource>();
        if (resource)
        {
            resource.Diocese = this;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        GameObject gm = collision.gameObject;
        Resource resource = gm.GetComponent<Resource>();
        if (resource)
            resource.Diocese = null;
    }

    public float CurrentTimeScale => currentTimeScale;

    public void FlipTimeScale()
    {
        currentTimeScale = currentTimeScale == fastTimeScale ? slowTimeScale : fastTimeScale;
    }

    public float spawnTimer;

    public float spawnCounter;

    public Vector3 offset;

    public List<GameObject> spawnables;

    public void Update()
    {
        spawnCounter -= Time.deltaTime;
        if(spawnCounter <= 0)
        {
            spawnCounter = spawnTimer;
            SpawnResource();
        }
    }

    private void SpawnResource()
    {
        Instantiate(spawnables[spawnType], gameObject.transform.position + offset, gameObject.transform.rotation);
        spawnType = spawnType == 3 ? 0 : spawnType + 1;
    }
}
