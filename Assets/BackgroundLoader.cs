using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundLoader : MonoBehaviour
{
    public int backgroundId = 2;

    // Start is called before the first frame update
    void Start()
    {
        LoadBackground();
    }

    void LoadBackground()
    {
        SceneManager.sceneLoaded += OnBackgroundLoaded;
        SceneManager.LoadScene(backgroundId, LoadSceneMode.Additive);
    }

    private void OnBackgroundLoaded(Scene background, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= OnBackgroundLoaded;
        List<Camera> cameras = GameObject.FindObjectsOfType<Camera>().ToList();
        Camera camera = cameras.Find((c) => c.name == "Background Camera");

        if (camera.gameObject.activeSelf)
        {
            camera.gameObject.SetActive(false);
        }
    }
}
