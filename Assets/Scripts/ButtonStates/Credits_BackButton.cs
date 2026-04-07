using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits_BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button backButton;
    [SerializeField] private Image givingHandImage;

    // 0 for normal, 1 for hovered, 2 for pressed (pointer down and up)
    [SerializeField] private List<Sprite> backButtonImages;

    // Anybody can modify the name of the game scene just in case if its name has updated
    //[Scene, SerializeField] private string gameSceneName;

    private void Awake()
    {
        // Find the play button inside the gameobject itself
        backButton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        givingHandImage.sprite = backButtonImages[1]; // Change giving image to its hovered image

        ((IPointerEnterHandler)backButton).OnPointerEnter(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        givingHandImage.sprite = backButtonImages[2]; // Change giving image to its pressed image

        ((IPointerDownHandler)backButton).OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        givingHandImage.sprite = backButtonImages[1]; // Change giving image to its hovered image

        ((IPointerUpHandler)backButton).OnPointerUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        givingHandImage.sprite = backButtonImages[0]; // Change giving image to its normal image

        ((IPointerExitHandler)backButton).OnPointerExit(eventData);
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
