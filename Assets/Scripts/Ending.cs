using System;
using System.Collections.Generic;
using TMPro;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    private enum Endings
    {
        None,
        Ending1,
        Ending2
    };

    [Header("Button objects")]
    [SerializeField] private Button enlistButton;
    [SerializeField] private Button runAwayButton;
    [SerializeField] private Button continueButton;

    [Header("Text objects")]
    [SerializeField] private TextMeshProUGUI endingTextObject;
    [SerializeField] private string firstEndingText;

    [SerializeField] private List<string> ending1Texts;
    [SerializeField] private List<string> ending2Texts;

    int endingLineIndex = 0;
    Endings currentEnding = Endings.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (endingLineIndex != 0) endingLineIndex = 0;
        if (currentEnding != Endings.None) currentEnding = Endings.None;
        if (endingTextObject != null) endingTextObject.text = firstEndingText;
        if (enlistButton != null) enlistButton.gameObject.SetActive(true);
        if (runAwayButton != null) runAwayButton.gameObject.SetActive(true);
        if (continueButton != null) continueButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnlistOption()
    {
        if (endingTextObject != null)
        {
            endingTextObject.text = ending1Texts[endingLineIndex];

            if (enlistButton != null) Destroy(enlistButton.gameObject);
            if (runAwayButton != null) Destroy(runAwayButton.gameObject);
            if (continueButton != null) continueButton.gameObject.SetActive(true);

            currentEnding = Endings.Ending1;
        }
    }

    public void RunAwayOption()
    {
        if (endingTextObject != null)
        {
            endingTextObject.text = ending2Texts[endingLineIndex];

            if (enlistButton != null) Destroy(enlistButton.gameObject);
            if (runAwayButton != null) Destroy(runAwayButton.gameObject);
            if (continueButton != null) continueButton.gameObject.SetActive(true);

            currentEnding = Endings.Ending2;
        }
    }

    public void ContinueEndingLine()
    {
        switch (currentEnding)
        {
            case Endings.Ending1: // Enlist lines

                endingLineIndex++;

                if (endingTextObject != null && ending1Texts[endingLineIndex] != null)
                {
                    endingTextObject.text = ending1Texts[endingLineIndex];
                }

                if (endingLineIndex >= ending1Texts.Count - 1 && continueButton != null)
                {
                    Destroy(continueButton.gameObject);
                }

                break;

                case Endings.Ending2: // Run away lines

                endingLineIndex++;

                if (endingTextObject != null && ending2Texts[endingLineIndex] != null)
                {
                    endingTextObject.text = ending2Texts[endingLineIndex];
                }

                if (endingLineIndex >= ending2Texts.Count - 1 && continueButton != null)
                {
                    Destroy(continueButton.gameObject);
                }

                break;

            default:
                break;
        }
    }
}
