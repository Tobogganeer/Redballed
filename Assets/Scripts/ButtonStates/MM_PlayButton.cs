using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// MM stands for MainMenu

public class MM_PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button playButton;
    [SerializeField] private Image givingHandImage;

    // 0 for normal, 1 for hovered, 2 for pressed (pointer down and up)
    [SerializeField] private List<Sprite> playButtonImages;

    // Anybody can modify the name of the game scene just in case if its name has updated
    [Scene, SerializeField] private string gameSceneName;

    private void Awake()
    {
        // Find the play button inside the gameobject itself
        playButton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        givingHandImage.sprite = playButtonImages[1]; // Change giving image to its hovered image

        ((IPointerEnterHandler)playButton).OnPointerEnter(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        givingHandImage.sprite = playButtonImages[2]; // Change giving image to its pressed image

        ((IPointerDownHandler)playButton).OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        givingHandImage.sprite = playButtonImages[1]; // Change giving image to its hovered image

        ((IPointerUpHandler)playButton).OnPointerUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        givingHandImage.sprite = playButtonImages[0]; // Change giving image to its normal image

        ((IPointerExitHandler)playButton).OnPointerExit(eventData);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
