using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnswersData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject;
    [SerializeField] Image toggle;

    [Header("Sprites")]
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedToggle;

    [Header("References")]
    [SerializeField] GameEvents events;

    private RectTransform _rect;
    public RectTransform rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private int _answerIndex = -1;
    public int answerIndex {  get { return _answerIndex; } }

    private bool Checked = false;

    public void UpdateData (string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState ()
    {
        Checked = !Checked;
        UpdateUI();

        if (events.updateQuestionAnswer != null)
        {
            events.updateQuestionAnswer(this);
        }
    }

    void UpdateUI ()
    {
        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
    }
}
