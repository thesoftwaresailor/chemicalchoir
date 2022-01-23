using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Diocese diocese;

    public GameObject winText;

    public static GameOver instance;

    public void Start()
    {
        instance = this;
    }

    public void DoGameOverStuff()
    {
        diocese.spawnTimer = 0.2f;
        winText.SetActive(true);
    }
}
