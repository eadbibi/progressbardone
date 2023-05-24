using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene to switch to

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the colliding object is the player
        {
            SceneManager.LoadScene(sceneToLoad); // Switch to the specified scene
        }
    }
}