using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

        if (!File.Exists(outputPath))
            File.WriteAllText(outputPath, "Time, Type, Value\n");

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
                LogRaw("PlayerState", GetLatestSnapshot());
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
        // e.g. "{value:"hi"}" - json in quotes (to go inside CSV)
        outputBuffer.Add($"{Time.time}, {type}, \"{{value:\"{value}\"}}\"");
    }

    public static void LogRaw(string type, object obj)
    {
        outputBuffer.Add($"{Time.time}, {type}, \"{JsonUtility.ToJson(obj)}\"");
    }

    PlayerController.Snapshot GetLatestSnapshot()
    {
        // This is stupid. Why is it a queue?
        //int numSnapshots = Player.Movement.Snapshots.Count;
        return Player.Movement.Snapshots.Last();
    }
}
