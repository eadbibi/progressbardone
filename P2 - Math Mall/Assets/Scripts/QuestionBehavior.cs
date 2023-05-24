using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class QuestionBehavior : MonoBehaviour
{
    public TMP_InputField MainInputField;
    [SerializeField] public TMP_InputField[] MainInputFields;
    [SerializeField] private string myText;
    public string RightAnswer;
    public string[] RightAnswers;
    public GameObject[] GDQuestionArray;
    public GameObject[] GDFinishedQuestions = new GameObject[10];
    [SerializeField]
    public GameObject[] GeoQuestionArray;
    public GameObject[] GeoFinishedQuestions = new GameObject[10];

    public GameObject[] KSQuestionArray;
    public GameObject[] KSFinishedQuestions = new GameObject[10];

    public GameObject[] FraQuestionArray;
    public GameObject[] FraFinishedQuestions = new GameObject[10];

    [SerializeField] private GameObject[] HintButtons;
    [SerializeField] private GameObject[] HintPanels;

    [SerializeField] List<QuestionDataSO> questionDataSOs = new();
    public static Action<List<QuestionDataSO>> onSendQuestionDataList = delegate { };
	
    public GameManager GameManager;

    void OnEnable()
    {
	    SceneManager.activeSceneChanged += OnActiveSceneChange;
    }

    void OnDisable()
    {
	    SceneManager.activeSceneChanged -= OnActiveSceneChange;
    }

    void Awake()
    {
	    if (questionDataSOs.Count is not 0)
	    {
		    questionDataSOs.Clear();
		    Debug.Log("Clear questionDataSOs");
	    }
	    
	    foreach (object item in Enum.GetValues(typeof(QuestionType)))
	    {
		    if (item is QuestionType.NOT_SET) continue;
		    var questionDataSO = ScriptableObject.CreateInstance<QuestionDataSO>();
		    questionDataSO.type = (QuestionType)item;
		    questionDataSOs.Add(questionDataSO);
	    }
    }
    
    void OnActiveSceneChange(Scene arg0, Scene arg1)
    {
	    LocateQuestionGameObjects();
	    TrySendQuestionData();
    }

    void LocateQuestionGameObjects()
    {
	    const int minus_one = -1;
	    const int zero = 0;

	    Question[] questions = FindObjectsOfType<Question>(includeInactive: true);
	    Debug.Log("Questions " + questions.Length);
	    
	    int gdQuestions = questions.Where(question => question.type is QuestionType.GD).ToArray().Length;
	    int geoQuestions = questions.Where(question => question.type is QuestionType.Geo).ToArray().Length;
	    int ksQuestions = questions.Where(question => question.type is QuestionType.KS).ToArray().Length;
	    int fraQuestions = questions.Where(question => question.type is QuestionType.Fra).ToArray().Length;

	    var gdArray = new GameObject[gdQuestions];
	    var geoArray = new GameObject[geoQuestions];
	    var ksArray = new GameObject[ksQuestions];
	    var fraArray = new GameObject[fraQuestions];
	    var inputFieldArray = new TMP_InputField[questions.Length];

	    for (var i = 0; i < questions.Length; i++)
	    {
		    var question = questions[i];
		    
		    foreach (object item in Enum.GetValues(typeof(QuestionType)))
		    {
			    if (question.type == (QuestionType)item)
			    {
				    int questionIndex = question.typeSpecificIndex;
				    int correctListIndex = questionIndex is zero ? zero : questionIndex + minus_one;

				    switch ((QuestionType)item)
				    {
					    case QuestionType.GD:
						    gdArray[correctListIndex] = question.gameObject;
						    inputFieldArray[correctListIndex] = question.GetInputField;
						    break;
					    case QuestionType.Geo:
						    geoArray[correctListIndex] = question.gameObject;
						    inputFieldArray[10 + correctListIndex] = question.GetInputField;
						    break;
					    case QuestionType.KS:
						    ksArray[correctListIndex] = question.gameObject;
						    inputFieldArray[20 + correctListIndex] = question.GetInputField;
						    break;
					    case QuestionType.Fra:
						    fraArray[correctListIndex] = question.gameObject;
						    inputFieldArray[30 + correctListIndex] = question.GetInputField;
						    break;
					    case QuestionType.NOT_SET: break;
					    default: throw new ArgumentOutOfRangeException();
				    }
			    }
		    }
	    }
	    
	    //for (int i = 0; i < inputFieldList.Count; i++)
	    //{
		//    for (int j = 0; j < inputFieldList.Count; j++)
		//    {
		//	    if (inputFieldList[j].gameObject.name.EndsWith($"Field{i}") is false) continue;
		//	    inputFieldList.Insert(i, inputFieldList[j]);
		//    }
	    //}

	    GDQuestionArray = gdArray;
	    GeoQuestionArray = geoArray;
	    KSQuestionArray = ksArray;
	    FraQuestionArray = fraArray;
	    MainInputFields = inputFieldArray.ToArray();
    }
    
    void TrySendQuestionData()
    {
	    if (questionDataSOs.Count is 0) return;
	    onSendQuestionDataList?.Invoke(questionDataSOs); // Opfanges af ProgressBar
    }

    public void CheckRightAnswer(int answerArray)
    {
        myText = MainInputField.text;
        RightAnswer = RightAnswers[answerArray];
        

        if (myText==RightAnswer)
        {
            Debug.Log("Correct");

            GameObject answerGameObject = GameObject.FindWithTag("Answer");

            if (answerGameObject.name.Contains("GD"))
            {
	            foreach (var questionData in questionDataSOs.Where(questionData => questionData.type is QuestionType.GD))
	            {
		            questionData.finishedQuestions = GDFinishedQuestions.Where(question => question != null).ToArray().Length;
	            }
            }
            else if (answerGameObject.name.Contains("GEO"))
            {
	            foreach (var questionData in questionDataSOs.Where(questionData => questionData.type is QuestionType.Geo))
	            {
		            questionData.finishedQuestions = GeoFinishedQuestions.Where(question => question != null).ToArray().Length;
	            }
            }
            else if (answerGameObject.name.Contains("KS"))
            {
	            foreach (var questionData in questionDataSOs.Where(questionData => questionData.type is QuestionType.KS))
	            {
		            questionData.finishedQuestions = KSFinishedQuestions.Where(question => question != null).ToArray().Length;
	            }
            }
            else if (answerGameObject.name.Contains("FRA"))
            {
	            foreach (var questionData in questionDataSOs.Where(questionData => questionData.type is QuestionType.Fra))
	            {
		            questionData.finishedQuestions = FraFinishedQuestions.Where(question => question != null).ToArray().Length;
	            }
            }
            
            Destroy(answerGameObject);
            GameManager.AnswerCorrectly();
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }

    public void SpawnGDQuestion()
    {
        SpawnTheQuestion(QuestionType.GD, GDQuestionArray, GDFinishedQuestions);
        SpawnHintButton(0);
    }

    public void SpawnGeoQuestion()
    {
        SpawnTheQuestion(QuestionType.Geo, GeoQuestionArray,GeoFinishedQuestions);
        SpawnHintButton(1);
    }

    public void SpawnKSQuestion()
    {
        SpawnTheQuestion(QuestionType.KS, KSQuestionArray, KSFinishedQuestions);
        SpawnHintButton(2);
    }

    public void SpawnFraQuestion()
    {
        SpawnTheQuestion(QuestionType.Fra, FraQuestionArray, FraFinishedQuestions);
        SpawnHintButton(3);
    }


    private void SpawnTheQuestion(QuestionType questionType, GameObject[] array, GameObject[] finishedQuestions)
    {

        var questionIndex = Random.Range(0, array.Length);
        var answerIndex = questionIndex;
        
        
        while (finishedQuestions[questionIndex] != null)
        {
            questionIndex = Random.Range(0, array.Length);
        }
        array[questionIndex].SetActive(true);
        finishedQuestions[questionIndex] = array[questionIndex];

        foreach (var questionData in questionDataSOs.Where(questionData => questionData.type == questionType))
        {
	        questionData.questionCount = array.Length;
	        questionData.finishedQuestions = finishedQuestions.Where(question => question != null).ToArray().Length;
        }
    }


    public void SelectAnInputField(int fieldIndex)
    {
        MainInputField = MainInputFields[fieldIndex];
    }
    
    private void SpawnHintButton(int index)
    {
	    GameObject hintButtonAtIndex = HintButtons[index];
	    bool hintButtonIsNull = hintButtonAtIndex == null;

	    if (hintButtonIsNull is false)
	    {
		    hintButtonAtIndex.SetActive(true);
		    return;
	    }
	    
		GameObject[] foundHintButtons = FindObjectsOfType<GameObject>(includeInactive: true);
		foreach (GameObject hintButtonGO in foundHintButtons)
		{
		    if (hintButtonGO.TryGetComponent(out HintButton foundHintButton) is false) continue;
		    
		    int questionTypeIntValue = (int)foundHintButton.questionType;
		    if (questionTypeIntValue != index) continue;
		    
		    hintButtonAtIndex = hintButtonGO;
		    hintButtonIsNull = hintButtonAtIndex == null;
		    if (hintButtonIsNull) return;

		    hintButtonAtIndex.SetActive(true);
		    HintButtons[index] = hintButtonAtIndex;
		}
    }
    
    public void SpawnHintPanel(int index)
    {
        if (!HintPanels[index].activeInHierarchy)
        {
            HintPanels[index].SetActive(true);
        }
        else
        {
            HintPanels[index].SetActive(false);
        }
    }

    
    public void ActivateQuestion(int index)
    {
        if (!GDQuestionArray[index].activeInHierarchy || !GeoQuestionArray[index].activeInHierarchy || !FraQuestionArray[index].activeInHierarchy || !KSQuestionArray[index].activeInHierarchy)
        {
            GameObject.FindGameObjectWithTag("Question").SetActive(true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Question").SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
