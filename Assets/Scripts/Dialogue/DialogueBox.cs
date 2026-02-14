using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public SpriteRenderer background;
    public Vector2 padding = new Vector2(0.3f, 0.3f);
    public Vector2 minSize = new Vector2(0.4f, 0.2f);

    [Space]
    public float charactersPerSecond = 30f;
    public GameObject nextKeyReminder;
    public float timeAfterFinishedToShowNextReminder = 2f;

    string targetText;
    int currentCharacters;
    float textTimer;
    Vector2 currentTextSize;

    float timeSinceFinished;

    /// <summary>
    /// Have we written out the entire line yet?
    /// </summary>
    public bool IsFinished => currentCharacters >= targetText.Length;

    private void Start()
    {
        Hide();
    }

    void Update()
    {
        // Typewriter effect
        if (!IsFinished)
        {
            textTimer += Time.deltaTime;
            if (textTimer > 1f / charactersPerSecond)
            {
                textTimer = 0f;
                currentCharacters++;
                // Update visible string and cache its size
                text.text = targetText.Substring(0, currentCharacters);
                text.ForceMeshUpdate();
                currentTextSize = text.GetRenderedValues();
            }

            SetNextKeyActive(false);
            timeSinceFinished = 0f;
        }
        else
        {
            // Remind player that they can click a key to continue conversation
            timeSinceFinished += Time.deltaTime;

            if (timeSinceFinished >= timeAfterFinishedToShowNextReminder && text.text.Length > 0)
                SetNextKeyActive(true);
        }

            Vector2 size = Vector2.zero;
        if (text.text.Length > 0)
        {
            // Getting weird, massive size for the first frame - clamp it
            //if (currentTextSize.sqrMagnitude < 100f * 100f)
            //    size = currentTextSize;
            size = currentTextSize;
        }

        Vector2 finalSize = new Vector2(Mathf.Max(size.x, minSize.x), Mathf.Max(size.y, minSize.y));

        background.size = finalSize + padding;
    }

    public void SetText(string text, bool updateInstantly = false)
    {
        background.enabled = true;
        targetText = text;
        currentCharacters = updateInstantly ? targetText.Length : 0;
        textTimer = 0f;


    }

    /// <summary>
    /// Instantly fills in the rest of the characters
    /// </summary>
    public void Finish()
    {
        currentCharacters = targetText.Length;
        text.text = targetText;
        text.ForceMeshUpdate();
        currentTextSize = text.GetRenderedValues();
        SetNextKeyActive(false);
    }

    public void Hide()
    {
        targetText = string.Empty;
        text.text = string.Empty;
        currentCharacters = 0;

        // Make sure we don't access while closing scene/game
        if (background != null)
            background.enabled = false;

        SetNextKeyActive(false);
    }

    void SetNextKeyActive(bool active)
    {
        if (nextKeyReminder != null)
            nextKeyReminder.SetActive(active);
    }
}
