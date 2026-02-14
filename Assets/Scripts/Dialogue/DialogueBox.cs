using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public SpriteRenderer background;
    public Vector2 padding = new Vector2(0.3f, 0.3f);
    public Vector2 minSize = new Vector2(0.4f, 0.2f);

    [Space]
    public float charactersPerSecond = 30f;

    string targetText;
    int currentCharacters;
    float textTimer;
    Vector2 currentTextSize;

    private void Start()
    {
        targetText = string.Empty;
    }

    void Update()
    {
        // Typewriter effect
        if (currentCharacters < targetText.Length)
        {
            textTimer += Time.deltaTime;
            if (textTimer > 1f / charactersPerSecond)
            {
                textTimer = 0f;
                currentCharacters++;
                // Update visible string and cache its size
                text.text = targetText.Substring(0, currentCharacters);
                currentTextSize = text.GetRenderedValues();
            }
        }

        Vector2 size = Vector2.zero;
        if (text.text.Length > 0)
        {
            // Getting weird, massive size for the first frame - clamp it
            if (currentTextSize.sqrMagnitude < 1000f * 1000f)
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

    public void Hide()
    {
        targetText = string.Empty;
        text.text = string.Empty;
        currentCharacters = 0;
        background.enabled = false;
    }
}
