using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    static LoadingScreen instance;
    private void Awake()
    {
        instance = this;
    }

    [Scene]
    public string loadingScreen;

    Scene scene;

    public static void Enable()
    {
        if (!instance.scene.isLoaded)
        {
            SceneManager.LoadScene(instance.loadingScreen, LoadSceneMode.Additive);
            instance.scene = SceneManager.GetSceneByName(instance.loadingScreen);
        }
    }

    public static AsyncOperation Disable()
    {
        if (instance.scene.isLoaded)
        {
            return SceneManager.UnloadSceneAsync(instance.scene);
        }

        return null;
    }
}
