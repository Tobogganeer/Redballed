using System;
using System.Collections;
using System.Collections.Generic;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

[Flags]
public enum Days
{
    None = 0,
    DayOne = 1 << 0,
    DayTwo = 1 << 1,
    DayThree = 1 << 2,
    EndingDay = 1 << 3
}

public class DayManager : MonoBehaviour
{
    [SerializeField, Scene] private string baseScene;
    [SerializeField, Scene] private string interDayScene; // Upgrades
    [SerializeField] private List<DayScene> dayScenes;
    [SerializeField] private Days currentDay = Days.DayOne;

    Dictionary<Days, string> days;
    Scene loadedDay;
    //Scene loadedBaseScene; // To see if we need to load it
    //Scene loadedInterdayScene;

    public Days CurrentDay => currentDay;
    /// <summary>
    /// Called after the passed Day is loaded
    /// </summary>
    public static event Action<Days> OnDayLoaded;
    /// <summary>
    /// Called before the passed Day is unloaded
    /// </summary>
    public static event Action<Days> BeforeDayUnloaded;
    /// <summary>
    /// Called when the passed day is ended, before it is unloaded
    /// </summary>
    public static event Action<Days> OnDayEnded;
    /// <summary>
    /// Called after the interday scene (upgrades) is loaded
    /// </summary>
    public static event Action OnInterdaySceneLoaded;

    public static bool Loading { get; private set; }


    private void Awake()
    {
        days = new Dictionary<Days, string>();
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
    public void LoadDay(Days day)
    {
        if (!days.TryGetValue(day, out string scene))
        {
            Debug.LogError($"Could not find day scene for day {day}");
            return;
        }

        StartCoroutine(LoadDayScene(day, scene));
    }

    private IEnumerator LoadDayScene(Days day, string scene)
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
    public Days day;
    [Scene]
    public string scene;
}

public static class DayExtensions
{
    public static Days GetNextDay(this Days day) => day switch
    {
        Days.None => Days.DayOne,
        Days.DayOne => Days.DayTwo,
        Days.DayTwo => Days.DayThree,
        _ => Days.EndingDay,
    };
    public static Days GetPreviousDay(this Days day) => day switch
    {
        Days.EndingDay => Days.DayThree,
        Days.DayThree => Days.DayTwo,
        _ => Days.DayOne,
    };

    public static string GetDayString(this Days day) => day switch
    {
        Days.DayOne => "Day 1",
        Days.DayTwo => "Day 2",
        Days.DayThree => "Day 3",
        Days.EndingDay => "Day 4",
        _ => "Invalid Day",
    };
}
