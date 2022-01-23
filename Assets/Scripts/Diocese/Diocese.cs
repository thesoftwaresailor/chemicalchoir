using System;
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
        spawnScale = 1;
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

    private float spawnScale;

    public float spawnCounter;

    public float offsetXBound;

    public float offsetZBound;

    private Vector3 offset;

    public List<GameObject> spawnables;

    public void Update()
    {
        spawnCounter += Time.deltaTime;
        if(spawnCounter >= spawnTimer * spawnScale)
        {
            spawnCounter = 0;
            SpawnResource();
        }
        ScaleSpawn();
    }

    private void ScaleSpawn()
    {
        spawnScale = (float) Math.Floor(Resource.ResourceRegister.Count / 6.0f) + 1;
    }

    private void SpawnResource()
    {
        Instantiate(spawnables[spawnType], gameObject.transform.position + offset, gameObject.transform.rotation);
        spawnType = spawnType == 3 ? 0 : spawnType + 1;
        offset = new Vector3(UnityEngine.Random.Range(-offsetXBound, offsetXBound), 0.2f, UnityEngine.Random.Range(-offsetZBound, offsetZBound));
    }
}
