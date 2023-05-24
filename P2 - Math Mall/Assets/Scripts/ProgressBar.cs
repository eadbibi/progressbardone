using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    [SerializeField] QuestionType questionType = QuestionType.NOT_SET;

    public QuestionType QuestionType => questionType;

    void OnEnable()
    {
	    QuestionBehavior.onSendQuestionDataList += ReceiveQuestionDataList;
    }

    void OnDisable()
    {
	    QuestionBehavior.onSendQuestionDataList -= ReceiveQuestionDataList;
    }

    void ReceiveQuestionDataList(List<QuestionDataSO> questionDataList)
    {
	    foreach (QuestionDataSO questionData in questionDataList.Where(data => data.type is not QuestionType.NOT_SET))
	    {
		    if (questionData.type != questionType) continue;
		    UpdateProgressBar(questionData.finishedQuestions, questionData.questionCount);
	    }
    }

    void Awake()
    {
        progressBar ??= GetComponent<Slider>();
    }

    public void UpdateProgressBar(float correctAnswers, float questionCount)
    {
        Debug.Log($"Correct answers: {correctAnswers} :: Question count {questionCount}");
        if (questionCount is 0)
        {
            Debug.LogWarning("questionCount var 0!");
            return;
        }
        progressBar.value = correctAnswers / questionCount;
    }
}


public enum QuestionType
{
    NOT_SET = -1,
    GD = 0, 
    Geo = 1,
    Fra = 2,
    KS = 3,
}