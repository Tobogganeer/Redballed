using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Day
{
    None,
    DayOne,
    DayTwo,
    DayThree,
    EndingDay
}

public class DayManager : MonoBehaviour
{
    [SerializeField] private List<DayScene> dayScenes;
    [SerializeField] private Day currentDay = Day.DayOne;

    public Day CurrentDay => CurrentDay;

    Dictionary<Day, string> days;

    private void Awake()
    {
        days = new Dictionary<Day, string>();
        foreach (DayScene scene in dayScenes)
            days.Add(scene.day, scene.scene);
    }

    private void Start()
    {
        //SceneManager.LoadSceneAsync(0, LoadSceneParameters)
    }

}

[System.Serializable]
public struct DayScene
{
    public Day day;
    [Scene]
    public string scene;
}
