using System;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int RemainingQuestions = 20;
    public TextMeshProUGUI RemainingQuestionsText;
    [SerializeField] private GameObject NextLevelButton;
    [SerializeField] private GameObject _stick;

    static GameManager instance;

    void OnEnable()
    {
	    SceneManager.activeSceneChanged += OnActiveSceneChange;
    }
    void OnDisable()
    {
	    SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }

    void Start()
    {
	    FindRemainingQuestionsText();
    }
    
    void OnActiveSceneChange(Scene arg0, Scene arg1)
    {
	    CheckForMultipleGameManagers();
	    FindRemainingQuestionsText();
	    FindNextLevelObjects();
	    FindJoyStick();
    }

    void FindNextLevelObjects()
    {
	    if (NextLevelButton != null) return;
	    
		Button[] buttons = FindObjectsOfType<Button>(includeInactive: true);

		foreach (Button button in buttons)
		{
		    if (button.gameObject.name.Contains("Next Level Button") is false) continue;
		    NextLevelButton = button.gameObject;
		    break;
		}
    }

    void FindRemainingQuestionsText()
    {
	    if (RemainingQuestionsText != null)
		    RemainingQuestionsText.text = "Remaining Questions: " + RemainingQuestions;
	    else
	    {
		    TextMeshProUGUI[] tmpTexts = FindObjectsOfType<TextMeshProUGUI>(includeInactive: true);

		    foreach (TextMeshProUGUI tmpText in tmpTexts)
		    {
			    if (tmpText.gameObject.name.Contains("Remaining Questions") is false) continue;
			    RemainingQuestionsText = tmpText;
			    break;
		    }
	    }
    }
    
    void FindJoyStick()
    {
	    if (_stick != null) return;

	    OnScreenStick[] sticks = FindObjectsOfType<OnScreenStick>(includeInactive: true);

	    foreach (OnScreenStick stick in sticks)
	    {
		    if (stick.gameObject.name.Contains("Stick") is false) continue;
		    _stick = stick.gameObject;
		    break;
	    }
    }

    void CheckForMultipleGameManagers()
    {
	    const int one_instance = 1;
	    GameManager[] gameManagers = FindObjectsOfType<GameManager>(includeInactive: true);

	    if (instance == null)
	    {
		    instance = this;
		    DontDestroyOnLoad(gameObject);
	    }
	    if (gameManagers.Length == one_instance) return;

	    bool thisIsTheInstance = instance == this;
	    
	    if (instance != null && thisIsTheInstance is false && gameManagers.Contains(instance))
	    {
		    Destroy(gameObject);
		    return;
	    }
	    if (thisIsTheInstance is false) return;
	    
	    foreach (var foundGameManager in gameManagers.Where(gameManager => gameManager != instance))
	    {
		    Debug.Log($"There's more than one GameManager in the scene. Unneeded {foundGameManager} destroyed");
		    Destroy(foundGameManager.gameObject);
	    }
    }

    public void AnswerCorrectly()
    {
        RemainingQuestions--;
        RemainingQuestionsText.text = "Remaining Questions: " + RemainingQuestions;

        if (RemainingQuestions == 0)
        {
            SpawnNextLevelButton();
        }
        
    }

    public void SpawnNextLevelButton()
    {
        NextLevelButton.SetActive(true);
    }



    public void ActivateJoyStick()
    {
        _stick.SetActive(true);
    }
    
    

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
