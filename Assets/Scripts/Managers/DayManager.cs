using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField, Scene] private string baseScene;
    [SerializeField] private List<DayScene> dayScenes;
    [SerializeField] private Day currentDay = Day.DayOne;

    Dictionary<Day, string> days;
    Scene loadedDay;
    Scene loadedBaseScene; // To see if we need to load it

    public Day CurrentDay => currentDay;
    public static event Action<Day> OnDayLoaded;
    public static event Action<Day> BeforeDayUnloaded;
    public static event Action<Day> OnDayEnded;

    public static bool Loading { get; private set; }


    private void Awake()
    {
        days = new Dictionary<Day, string>();
        foreach (DayScene dayScene in dayScenes)
        {
            //Scene scene = SceneManager.GetSceneByName(dayScene.scene);
            //if (!scene.IsValid())
            //    throw new System.NullReferenceException($"Day scene for day {dayScene.day} is null or invalid!");

            // The Scene attribute makes sure the given scene is valid
            days.Add(dayScene.day, dayScene.scene);
        }
    }

    /*
    private void Start()
    {
        StartLoadingDay(currentDay);
    }
    */

    /// <summary>
    /// Unloads the current day
    /// </summary>
    public IEnumerator EndDay()
    {
        Loading = true;

        // Unload the current day
        if (loadedDay.isLoaded)
        {
            BeforeDayUnloaded?.Invoke(currentDay);
            yield return SceneManager.UnloadSceneAsync(loadedDay);
        }

        if (loadedBaseScene.isLoaded)
            yield return SceneManager.UnloadSceneAsync(loadedBaseScene);

        // Reset
        loadedDay = new Scene();
        loadedBaseScene = new Scene();

        Loading = false;

        OnDayEnded?.Invoke(currentDay);
    }

    /// <summary>
    /// Starts loading the <paramref name="day"/>. Calls <see cref="OnDayLoaded"/> when complete.
    /// </summary>
    /// <param name="day">The day to load</param>
    public IEnumerator StartLoadingDay(Day day)
    {
        if (!days.TryGetValue(day, out string scene))
        {
            Debug.LogError($"Could not find day scene for day {day}");
            yield break;
        }

        yield return LoadDayScene(day, scene);
    }

    private IEnumerator LoadDayScene(Day day, string scene)
    {
        Loading = true;

        // Unload the current day
        if (loadedDay.isLoaded)
        {
            BeforeDayUnloaded?.Invoke(currentDay);
            yield return SceneManager.UnloadSceneAsync(loadedDay);
        }

        // Reload the base scene
        if (loadedBaseScene.isLoaded)
            yield return SceneManager.UnloadSceneAsync(loadedBaseScene);

        // Get callback to store base scene once loaded
        SceneManager.sceneLoaded += OnBaseSceneLoaded;
        yield return SceneManager.LoadSceneAsync(baseScene, LoadSceneMode.Single); // Load as main scene
        SceneManager.sceneLoaded -= OnBaseSceneLoaded;

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        //currentlyLoadedDay = scene;
        loadedDay = SceneManager.GetSceneByName(scene);
        currentDay = day;

        Loading = false;

        OnDayLoaded?.Invoke(day);
    }

    private void OnBaseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedBaseScene = scene;
    }

}

[Serializable]
public struct DayScene
{
    public Day day;
    [Scene]
    public string scene;
}

public static class DayExtensions
{
    public static Day GetNextDay(this Day day) => (Day)Mathf.Min((int)day + 1, (int)Day.EndingDay);
    public static Day GetPreviousDay(this Day day) => (Day)Mathf.Max((int)day - 1, (int)Day.DayOne);
}
