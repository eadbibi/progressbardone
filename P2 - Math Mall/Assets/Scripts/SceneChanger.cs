using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // Name of the scene to load, set in the Inspector

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName); // Load the scene with the specified name
    }
}