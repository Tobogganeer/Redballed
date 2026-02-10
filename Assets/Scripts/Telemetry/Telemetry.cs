using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Telemetry : MonoBehaviour
{
    public bool log = true;
    public bool logInEditor = false;
    public int playerStatesPerSecond = 10;

    static List<string> outputBuffer = new List<string>();
    float outputFlushInterval = 5f;
    float flushTimer;
    string outputPath;
    int sessionID;

    float playerStateTimer;

    private void Start()
    {
        if (!log || (!logInEditor && Application.isEditor))
        {
            Destroy(this);
            return;
        }

        sessionID = Random.Range(10000, 99999);
        outputPath = Path.Combine(Application.persistentDataPath, $"telemetry_{sessionID}.txt");

        outputBuffer.Clear();

        Log("SessionID", sessionID.ToString());
        Log("Date", System.DateTime.Now.ToString());
    }

    private void Update()
    {
        playerStateTimer -= Time.deltaTime;
        if (playerStateTimer <= 0f)
        {
            playerStateTimer = 1f / playerStatesPerSecond;

            if (Player.Exists && Player.Movement.Snapshots.Count > 0)
                LogRaw(new PlayerTelemetryEntry("PlayerState", GetLatestSnapshot()));
        }

        // This one counts up so we don't flush immediately
        flushTimer += Time.deltaTime;
        if (flushTimer > outputFlushInterval)
        {
            flushTimer = 0f;
            FlushOutput();
        }
    }

    void FlushOutput()
    {
        if (outputBuffer.Count == 0)
            return;

        //File.AppendAllLinesAsync(outputPath, outputBuffer);
        File.AppendAllLines(outputPath, outputBuffer);
        outputBuffer.Clear();
    }

    void OnApplicationQuit()
    {
        FlushOutput();
    }

    public static void Log(string type, string value = "")
    {
        outputBuffer.Add(JsonUtility.ToJson(new TelemetryEntry(type, value)));
    }

    public static void LogRaw(object obj)
    {
        outputBuffer.Add(JsonUtility.ToJson(obj));
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
