using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName = "Quiz/ new GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Questions questions);
    public UpdateQuestionUICallback updateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswersData pickedAnswers);
    public UpdateQuestionAnswerCallback updateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback displayResolutionScreen;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback scoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore;
    [HideInInspector]
    public int StartupHighscore;
}
