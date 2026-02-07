using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    Dictionary<Day, BuildIndex> days;
    Scene loadedDay;
    Scene loadedBaseScene; // To see if we need to load it

    public Day CurrentDay => currentDay;
    public static event Action<Day> OnDayLoaded;
    public static event Action<Day> BeforeDayUnloaded;


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
    }

    /// <summary>
    /// Starts loading the <paramref name="day"/>. Calls <see cref="OnDayLoaded"/> when complete.
    /// </summary>
    /// <param name="day">The day to load</param>
    public IEnumerator StartLoadingDay(Day day)
    {
        if (!days.TryGetValue(day, out BuildIndex buildIndex))
        {
            Debug.LogError($"Could not find day scene for day {day}");
            yield break;
        }

        yield return LoadDayScene(day, buildIndex);
    }

    private IEnumerator LoadDayScene(Day day, BuildIndex scene)
    {
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
        loadedDay = SceneManager.GetSceneAt(scene);
        currentDay = day;
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
