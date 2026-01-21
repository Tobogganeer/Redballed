using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=JQ0Jdfxo7Cg

class App
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        GameObject appObj = Resources.Load<GameObject>("App");
        if (appObj == null)
        {
            Debug.Log("No 'App' object found in the Resources folder.");
            return;
        }

        GameObject app = Object.Instantiate(appObj);
        if (app == null)
            throw new System.ApplicationException();

        Object.DontDestroyOnLoad(app);
    }
}