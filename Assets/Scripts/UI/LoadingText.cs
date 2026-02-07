using System.Collections;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    public CanvasGroup group;

    static readonly WaitForSeconds DelayBeforeStartingFadeIn = new(1f);
    const float FadeInTime = 1f;

    IEnumerator Start()
    {
        group.alpha = 0f;

        // If the next scene loads fast, don't show at all
        yield return DelayBeforeStartingFadeIn;

        float timer = 0f;

        while (timer < FadeInTime)
        {
            timer += Time.deltaTime;
            group.alpha = timer / FadeInTime;
            yield return null;
        }
    }
}
