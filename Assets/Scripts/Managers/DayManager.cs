using System;
using System.Collections;
using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using BuildIndex = System.Int32;

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

    Dictionary<Day, BuildIndex> days;
    BuildIndex currentlyLoadedDay;

    public Day CurrentDay => CurrentDay;
    public static event Action<Day> OnDayLoaded;


    private void Awake()
    {
        days = new Dictionary<Day, BuildIndex>();
        foreach (DayScene dayScene in dayScenes)
        {
            Scene scene = SceneManager.GetSceneByName(dayScene.scene);
            if (!scene.IsValid())
                throw new System.NullReferenceException($"Day scene for day {dayScene.day} is null or invalid!");

            days.Add(dayScene.day, scene.buildIndex);
        }
    }

    private void Start()
    {
        StartLoadingDay(currentDay);
    }

    public void StartLoadingDay(Day day)
    {
        if (!days.TryGetValue(day, out BuildIndex buildIndex))
        {
            Debug.LogError($"Could not find day scene for day {day}");
            return;
        }

        LoadDayScene(day, buildIndex);
    }

    private IEnumerator LoadDayScene(Day day, BuildIndex scene)
    {
        if (currentlyLoadedDay > 0)
            yield return SceneManager.UnloadSceneAsync(currentlyLoadedDay);

        yield return SceneManager.LoadSceneAsync(scene);

        currentlyLoadedDay = scene;
        currentDay = day;
        OnDayLoaded?.Invoke(day);
    }

}

[System.Serializable]
public struct DayScene
{
    public Day day;
    [Scene]
    public string scene;
}
