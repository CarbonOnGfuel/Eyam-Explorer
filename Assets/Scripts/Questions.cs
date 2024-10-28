using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct Answer
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }
    [SerializeField] private bool isCorrect;
    public bool IsCorrect { get {  return isCorrect; } }
}
[CreateAssetMenu(fileName = "new Questions", menuName = "Quiz/ new Questions")]
public class Questions : ScriptableObject
{
    public enum AnswerType { Multi, Single}

    [SerializeField] private string _info = string.Empty;
    public string Info { get { return _info; } }
    [SerializeField] Answer[] _answers = null;
    public Answer[] Answers { get {  return _answers; } }

    //parameters

    [SerializeField] private bool _useTimer = false;
    public bool UseTimer { get { return _useTimer; } }

    [SerializeField] private int _time = 0;
    public int Time { get { return _time; } }

    [SerializeField] private AnswerType _answerType = AnswerType.Multi;
    public AnswerType GetAnswerType { get { return _answerType; } }

    [SerializeField] private int _addScore = 10;
    public int AddScore { get { return _addScore;} }

    public List<int> CorrectAnswersList()
    {
        List<int> CorrectAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }
}
