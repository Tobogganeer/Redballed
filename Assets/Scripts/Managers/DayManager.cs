using System;
using System.Collections;
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
    [SerializeField, Scene] private string baseScene;
    [SerializeField, Scene] private string interDayScene; // Upgrades
    [SerializeField] private List<DayScene> dayScenes;
    [SerializeField] private Day currentDay = Day.DayOne;

    Dictionary<Day, string> days;
    Scene loadedDay;
    //Scene loadedBaseScene; // To see if we need to load it
    //Scene loadedInterdayScene;

    public Day CurrentDay => currentDay;
    public static event Action<Day> OnDayLoaded;
    public static event Action<Day> BeforeDayUnloaded;
    public static event Action<Day> OnDayEnded;
    public static event Action OnInterdaySceneLoaded;

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
    /// Unloads the current day and loads the inter-day scene (for upgrades)
    /// </summary>
    public void EndDay()
    {
        StartCoroutine(EndDayAndLoadInterdayScene());
    }

    private IEnumerator EndDayAndLoadInterdayScene()
    {
        OnDayEnded?.Invoke(currentDay);

        Loading = true;
        LoadingScreen.Enable();

        // Unload the current day
        if (loadedDay.isLoaded)
        {
            BeforeDayUnloaded?.Invoke(currentDay);
            //yield return SceneManager.UnloadSceneAsync(loadedDay);
        }

        //if (loadedBaseScene.isLoaded)
        //    yield return SceneManager.UnloadSceneAsync(loadedBaseScene);

        //if (loadedInterdayScene.isLoaded)
        //    yield return SceneManager.UnloadSceneAsync(loadedInterdayScene);

        // Get callback to store inter-day scene once loaded
        //SceneManager.sceneLoaded += InterdaySceneLoaded;
        // This unloads all other scenes
        yield return SceneManager.LoadSceneAsync(interDayScene);
        //SceneManager.sceneLoaded -= InterdaySceneLoaded;

        // Reset
        loadedDay = new Scene();
        //loadedBaseScene = new Scene();

        Loading = false;
        LoadingScreen.Disable();

        OnInterdaySceneLoaded?.Invoke();
    }

    /// <summary>
    /// Starts loading the <paramref name="day"/>. Calls <see cref="OnDayLoaded"/> when complete.
    /// </summary>
    /// <param name="day">The day to load</param>
    public void LoadDay(Day day)
    {
        if (!days.TryGetValue(day, out string scene))
        {
            Debug.LogError($"Could not find day scene for day {day}");
            return;
        }

        StartCoroutine(LoadDayScene(day, scene));
    }

    private IEnumerator LoadDayScene(Day day, string scene)
    {
        Loading = true;
        LoadingScreen.Enable();

        // Unload the current day (in normal gameplay, we won't have a day loaded)
        if (loadedDay.isLoaded)
        {
            BeforeDayUnloaded?.Invoke(currentDay);
            //yield return SceneManager.UnloadSceneAsync(loadedDay);
        }

        // Reload the base scene
        // EDIT: Not needed; loading a 'single' scene unloads all other scenes
        //if (loadedBaseScene.isLoaded)
        //    yield return SceneManager.UnloadSceneAsync(loadedBaseScene);

        // Get callback to store base scene once loaded
        //SceneManager.sceneLoaded += BaseSceneLoaded;
        // Load base scene, which unloads other scenes (including inter-day scene)
        yield return SceneManager.LoadSceneAsync(baseScene, LoadSceneMode.Single); // Load as main scene
        //SceneManager.sceneLoaded -= BaseSceneLoaded;

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        //currentlyLoadedDay = scene;
        loadedDay = SceneManager.GetSceneByName(scene);
        currentDay = day;

        Loading = false;
        LoadingScreen.Disable();

        OnDayLoaded?.Invoke(day);
    }

    /*
    private void BaseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedBaseScene = scene;
    }

    private void InterdaySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadedInterdayScene = scene;
    }
    */
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
