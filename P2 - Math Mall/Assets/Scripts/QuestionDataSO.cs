using UnityEngine;

public class QuestionDataSO : ScriptableObject
{
	public QuestionType type = QuestionType.NOT_SET;
	public int questionCount;
	public int finishedQuestions;
}