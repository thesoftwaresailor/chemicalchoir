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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
