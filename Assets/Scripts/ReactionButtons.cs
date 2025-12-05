using UnityEngine;
using UnityEngine.UI;

public class ReactionButtons : MonoBehaviour
{
    [Header("Buttons")]
    public Button likeButton;
    public Button dislikeButton;
    public Button commentButton;

    [Header("Sprites")]
    public Sprite likeDefault;
    public Sprite likeActive;
    public Sprite dislikeDefault;
    public Sprite dislikeActive;

    private bool isLiked = false;
    private bool isDisliked = false;

    void Start()
    {
        likeButton.onClick.AddListener(ToggleLike);
        dislikeButton.onClick.AddListener(ToggleDislike);
        commentButton.onClick.AddListener(OpenComment);
    }

    void ToggleLike()
    {
        isLiked = !isLiked;
        isDisliked = false; // reset dislike if like is pressed
        UpdateSprites();
    }

    void ToggleDislike()
    {
        isDisliked = !isDisliked;
        isLiked = false; // reset like if dislike is pressed
        UpdateSprites();
    }

    void UpdateSprites()
    {
        likeButton.image.sprite = isLiked ? likeActive : likeDefault;
        dislikeButton.image.sprite = isDisliked ? dislikeActive : dislikeDefault;
    }

    void OpenComment()
    {
        Debug.Log("Comment button pressed! Open comment UI.");
    }
}