using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    Questions[] _questions = null;
    public Questions[] Question { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    private List<AnswersData> PickedAnswers = new List<AnswersData>();
    private List<int> FinishedQuestions = new List<int>();
    private int CurrentQuestion = 0;

    private IEnumerator IE_WaitTillNextRound = null;

    private bool isFinished
    {
        get
        {
            return (FinishedQuestions.Count < Question.Length) ? false : true;
        }
    }

    void OnEnable()
    {
        events.updateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable()
    {
        events.updateQuestionAnswer -= UpdateAnswers;
    }

    void Start()
    {
        LoadQuestions();

        events.CurrentFinalScore = 0;

        var seed = UnityEngine.Random.Range( int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    public void UpdateAnswers(AnswersData newAnswer)
    {
        if (Question[CurrentQuestion].GetAnswerType == Questions.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
                PickedAnswers.Clear();
                PickedAnswers.Add(newAnswer);
            }
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x ==  newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void EraseAnswers ()
    {
        PickedAnswers = new List<AnswersData> ();
    }

    void Display()
    {
        EraseAnswers ();
        var question = GetRandomQuestion ();

        if (events.updateQuestionUI != null)
        {
            events.updateQuestionUI (question);
        } else { Debug.LogWarning("Oops, Something went wrong here!"); }
    }

    public void Accept()
    {
        Debug.Log("Accepted");
        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(CurrentQuestion);

        UpdateScore((isCorrect) ? Question[CurrentQuestion].AddScore : -Question[CurrentQuestion].AddScore);
        var type = (isFinished) ? UIManager.ResolutionScreenType.Finished : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;

        if (events.displayResolutionScreen != null)
        {
            events.displayResolutionScreen(type, Question[CurrentQuestion].AddScore);
        }

        if (type != UIManager.ResolutionScreenType.Finished)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    Questions GetRandomQuestion ()
    {
        var randomIndex = GetRandomQuestionIndex ();
        CurrentQuestion = randomIndex;

        return Question[CurrentQuestion];
    }
    int GetRandomQuestionIndex ()
    {
        var random = 0;
        if (FinishedQuestions.Count < Question.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, Question.Length);
            } while (FinishedQuestions.Contains(random) || random == CurrentQuestion);
        }
        return random;
    }

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }
    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> c = Question[CurrentQuestion].CorrectAnswersList();
            List<int> p = PickedAnswers.Select(x => x.answerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }

    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Questions));
        _questions = new Questions[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Questions)objs[i];
        }
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.scoreUpdated != null)
        {
            events.scoreUpdated();
        }
    }
}
