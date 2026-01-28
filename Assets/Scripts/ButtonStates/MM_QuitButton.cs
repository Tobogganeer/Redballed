using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// MM stands for MainMenu

public class MM_QuitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button quitButton;
    [SerializeField] private Image pointingImage;

    // 0 for normal, 1 for hovered, 2 for pressed (pointer down and up)
    [SerializeField] private List<Sprite> quitButtonImages;

    private void Awake()
    {
        // Find the quit button inside the gameobject itself
        quitButton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointingImage.sprite = quitButtonImages[1]; // Change pointing image to its hovered image

        ((IPointerEnterHandler)quitButton).OnPointerEnter(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointingImage.sprite = quitButtonImages[2]; // Change pointing image to its pressed image

        ((IPointerDownHandler)quitButton).OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointingImage.sprite = quitButtonImages[1]; // Change pointing image to its hovered image

        ((IPointerUpHandler)quitButton).OnPointerUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointingImage.sprite = quitButtonImages[0]; // Change pointing image to its normal image

        ((IPointerExitHandler)quitButton).OnPointerExit(eventData);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
