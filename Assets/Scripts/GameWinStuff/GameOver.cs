using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public Diocese diocese;

    public Canvas winText;

    public static GameOver instance;

    public void Start()
    {
        instance = this;
    }

    public void DoGameOverStuff()
    {
        diocese.spawnTimer = 0.2f;
        winText.enabled = true;
    }

}
