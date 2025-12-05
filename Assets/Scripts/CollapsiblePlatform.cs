using UnityEngine;

public class CollapsiblePlatform : MonoBehaviour, IStompable
{
    public float lifeTime = 2f;

    bool hasPlayer;
    float currentPlayerTime;

    private void Update()
    {
        if (hasPlayer)
            currentPlayerTime += Time.deltaTime;
        else
            currentPlayerTime -= Time.deltaTime;

        // Make sure it never goes below 0
        currentPlayerTime = Mathf.Max(currentPlayerTime, 0f);

        if (currentPlayerTime > lifeTime)
            Destroy(gameObject);
    }


    public void PlayerLanded(PlayerController player)
    {
        hasPlayer = true;
    }

    public void PlayerLeft(PlayerController player)
    {
        hasPlayer = false;
    }
}
