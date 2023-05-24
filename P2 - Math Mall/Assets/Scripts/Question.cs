using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Question : MonoBehaviour
{
	[TextArea, SerializeField] string info = INFO_STRING;
	
	[SerializeField] public QuestionType type = QuestionType.NOT_SET;
	[SerializeField] public int typeSpecificIndex = INVALID_INDEX;
	[SerializeField] public int globalQuestionIndex = INVALID_INDEX;
	[SerializeField] public TMP_InputField inputField;
	
	[SerializeField] Button answerButton;
	[SerializeField] public Button closeButton;

	public TMP_InputField GetInputField => inputField ??= GetComponentInChildren<TMP_InputField>();
	
	const string ANSWER_TAG = "Answer";
	const string CLOSE_BUTTON_NAME = "Close Button";
	const string INPUT_FIELD_GO_NAME = "InputField";

	void Awake()
	{
		Button[] buttons = GetComponentsInChildren<Button>();
		foreach (Button button in buttons)
		{
			if (button.tag is ANSWER_TAG)
			{
				answerButton = button;
			}

			if (button.gameObject.name is CLOSE_BUTTON_NAME)
			{
				closeButton = button;
			}
			
		}

		if (answerButton != null)
			answerButton.onClick.AddListener(OnAnswerButtonClick);
		
		if (closeButton != null)
			closeButton.onClick.AddListener(OnCloseButtonClick);

		inputField ??=  GetComponentInChildren<TMP_InputField>();
	}

	void OnDisable()
	{
		answerButton.onClick.RemoveListener(OnAnswerButtonClick);
		closeButton.onClick.RemoveListener(OnCloseButtonClick);
	}

	void OnAnswerButtonClick()
	{
		var questionBehaviour = FindObjectOfType<QuestionBehavior>();
		int questionOriginalIndex = typeSpecificIndex - 1;
		questionBehaviour.SelectAnInputField(questionOriginalIndex);
		questionBehaviour.CheckRightAnswer(questionOriginalIndex);
	}
	
	void OnCloseButtonClick()
	{
		var buttonBehaviour = FindObjectOfType<ButtonBehavior>();
		buttonBehaviour.ShutDownQuestion();
	}


#if UNITY_EDITOR
	
	const int INVALID_INDEX = -1;
	const string INFO_STRING = "Can only be changed programatically";
	
	void OnValidate()
	{
		HandleIndexAndTypeSetting();
	}
	
	void HandleIndexAndTypeSetting()
	{
		if (!string.Equals(info, INFO_STRING, StringComparison.Ordinal))
			info = INFO_STRING;
		
		string gameObjName = gameObject.name;
		bool hasTypeInName = SetQuestionTypeValueBasedOnGameObjectName(gameObjName, out QuestionType foundQuestionType);

		if (hasTypeInName && foundQuestionType is not QuestionType.NOT_SET)
			type = foundQuestionType;
		
		SetIndexValueBasedOnTypeInName();
	}

	void SetIndexValueBasedOnTypeInName()
	{
		const int correct_off_by_one = 1;
		const char space = ' ';
		const string name_end_1 = " 1";

		Question[] allQuestionsInScene = FindObjectsOfType<Question>(includeInactive: true);
		Question[] gdQuestions = allQuestionsInScene.Where(question => question.type is QuestionType.GD).ToArray();
		Question[] geoQuestions = allQuestionsInScene.Where(question => question.type is QuestionType.Geo).ToArray();
		Question[] ksQuestions = allQuestionsInScene.Where(question => question.type is QuestionType.KS).ToArray();
		Question[] fraQuestions = allQuestionsInScene.Where(question => question.type is QuestionType.Fra).ToArray();

		Question[][] questionArrays = { gdQuestions, geoQuestions, ksQuestions, fraQuestions };

		for (int i = 0; i < questionArrays.Length; i++)
		{
			Question[] currentArray = questionArrays[i];

			for (int j = 0; j < currentArray.Length; j++)
			{
				int iIterationCorrected = i + 1;
				int jIterationCorrected = j + 1;
				int questionIndex = iIterationCorrected * jIterationCorrected - 1;
				
				currentArray[i].globalQuestionIndex = questionIndex;
			}
		}
		
		Question[] questionsOfType = FindObjectsOfType<Question>(includeInactive: true)
			.Where(question => question.type == type)
			.ToArray();
		
		for (int i = 0; i < questionsOfType.Length; i++)
		{
			int correctedIndex = i + correct_off_by_one;
			string correctedIndexString = $"{space}{correctedIndex}";

			if (!gameObject.name.EndsWith(correctedIndexString)) continue;
			
			typeSpecificIndex = correctedIndex;
			return;
		}
	}

	bool SetQuestionTypeValueBasedOnGameObjectName(string objectName, out QuestionType foundQuestionType)
	{
		foundQuestionType = QuestionType.NOT_SET;
		
		foreach (QuestionType value in (QuestionType[])Enum.GetValues(typeof(QuestionType)))
		{
			string valueName = value.HumanName();
			bool gameObjNameIncludesType = objectName.Contains($"{valueName}");
			bool typeIsAlreadySet = value is not QuestionType.NOT_SET && type == value;
			
			switch (gameObjNameIncludesType)
			{
				case false: continue;
				case true when typeIsAlreadySet: break;
				case true:
					foundQuestionType = value;
					break;
			}
		}
		return foundQuestionType is not QuestionType.NOT_SET;
	}
#endif
}