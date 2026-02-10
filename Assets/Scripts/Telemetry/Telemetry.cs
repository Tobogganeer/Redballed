using System.Linq;
using UnityEngine;

public class Telemetry : MonoBehaviour
{
    public int playerStatesPerSecond = 10;

    float playerStateTimer;

    private void Update()
    {
        playerStateTimer -= Time.deltaTime;
        if (playerStateTimer <= 0f)
        {
            playerStateTimer = 1f / playerStatesPerSecond;

            if (Player.Exists && Player.Movement.Snapshots.Count > 0)
                LogRaw(new PlayerTelemetryEntry("PlayerState", GetLatestSnapshot()));
        }
    }

    public static void Log(string type, string value = "")
    {
        Debug.Log(JsonUtility.ToJson(new TelemetryEntry(type, value)));
    }

    public static void LogRaw(object obj)
    {
        Debug.Log(JsonUtility.ToJson(obj));
    }

    PlayerController.Snapshot GetLatestSnapshot()
    {
        // This is stupid. Why is it a queue?
        //int numSnapshots = Player.Movement.Snapshots.Count;
        return Player.Movement.Snapshots.Last();
    }
}

[System.Serializable]
struct PlayerTelemetryEntry
{
    public float time;
    public string type;
    public PlayerController.Snapshot snapshot;

    public PlayerTelemetryEntry(string type, PlayerController.Snapshot snapshot)
    {
        time = Time.time;
        this.type = type;
        this.snapshot = snapshot;
    }
}

[System.Serializable]
struct TelemetryEntry
{
    public float time;
    public string type;
    public string value;

    public TelemetryEntry(string type, string value = "")
    {
        time = Time.time;
        this.type = type;
        this.value = value;
    }
}