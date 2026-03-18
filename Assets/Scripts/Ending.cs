using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private TextMeshProUGUI endingTextObject;
    [SerializeField] private List<string> endingTexts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        endingTextObject = GetComponent<TextMeshProUGUI>();

        if (endingTextObject != null && endingTexts[0] != null && 
            DayManager.GetCurrentEnding() == Endings.Ending1)
        {
            endingTextObject.text = endingTexts[0];
        }

        else if (endingTextObject != null && endingTexts[1] != null &&
            DayManager.GetCurrentEnding() == Endings.Ending2)
        {
            endingTextObject.text = endingTexts[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
