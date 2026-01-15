using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PM_ResumeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button resumeButton;
    [SerializeField] private List<Image> starImages;

    private void Awake()
    {
        // Find the resume button inside the gameobject itself
        resumeButton = GetComponent<Button>();

        // Don't show the star images at the beginning
        for (int i = 0; i < starImages.Count; i++)
        {
            starImages[i].gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show star images when pointer is hovered on the button
        for (int i = 0; i < starImages.Count; i++)
        {
            starImages[i].gameObject.SetActive(true);
        }

        ((IPointerEnterHandler)resumeButton).OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide star images when pointer is not hovered on the button anymore
        for (int i = 0; i < starImages.Count; i++)
        {
            starImages[i].gameObject.SetActive(false);
        }

        ((IPointerExitHandler)resumeButton).OnPointerExit(eventData);
    }
}
