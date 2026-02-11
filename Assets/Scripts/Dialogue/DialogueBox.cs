using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public SpriteRenderer background;
    public Vector2 padding = new Vector2(0.3f, 0.3f);
    public Vector2 minSize = new Vector2(0.4f, 0.2f);


    void Update()
    {
        Vector2 size = Vector2.zero;
        if (text.text.Length > 0)
        {
            Vector2 textSize = text.GetRenderedValues();

            // Getting weird, massive size for the first frame - clamp it
            if (textSize.sqrMagnitude < 1000f * 1000f)
                size = textSize;
        }

        Vector2 finalSize = new Vector2(Mathf.Max(size.x, minSize.x), Mathf.Max(size.y, minSize.y));

        background.size = finalSize + padding;
    }
}
