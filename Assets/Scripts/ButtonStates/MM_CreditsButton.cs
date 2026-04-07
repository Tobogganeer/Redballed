using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MM_CreditsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button creditsButton;
    [SerializeField] private Image givingHandImage;

    // 0 for normal, 1 for hovered, 2 for pressed (pointer down and up)
    [SerializeField] private List<Sprite> creditButtonImages;

    // Anybody can modify the name of the game scene just in case if its name has updated
    //[Scene, SerializeField] private string gameSceneName;

    private void Awake()
    {
        // Find the play button inside the gameobject itself
        creditsButton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        givingHandImage.sprite = creditButtonImages[1]; // Change giving image to its hovered image

        ((IPointerEnterHandler)creditsButton).OnPointerEnter(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        givingHandImage.sprite = creditButtonImages[2]; // Change giving image to its pressed image

        ((IPointerDownHandler)creditsButton).OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        givingHandImage.sprite = creditButtonImages[1]; // Change giving image to its hovered image

        ((IPointerUpHandler)creditsButton).OnPointerUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        givingHandImage.sprite = creditButtonImages[0]; // Change giving image to its normal image

        ((IPointerExitHandler)creditsButton).OnPointerExit(eventData);
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditScene");
    }
}
