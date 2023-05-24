using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private GameObject NextLevelPanel;
    public GameManager GameManager;

    void OnEnable()
    {
	    SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
	    SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
    
    void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
	    FindNextLevelPanel();
    }

    void FindNextLevelPanel()
    {
	    if (NextLevelPanel != null) return;
	    
	    Image[] images = FindObjectsOfType<Image>(includeInactive: true);

	    foreach (Image image in images)
	    {
		    if (image.gameObject.name.Contains("Next Level Panel") is false) continue;
		    NextLevelPanel = image.gameObject;
	    }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    


    public void SpawnNextLevelPanel()
    {
        NextLevelPanel.SetActive(true);
    }

    public void ShutDownQuestion()
    {
        GameObject.FindGameObjectWithTag("Question").SetActive(false);
        GameObject.FindGameObjectWithTag("HintButton").SetActive(false);
        GameManager.ActivateJoyStick();
    }
    
    


   
    // Update is called once per frame
    void Update()
    {
        
    }
}
