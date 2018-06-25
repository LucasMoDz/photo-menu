using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneNavigator : EditorWindow
{
    [MenuItem("Scene Navigator/Main Menu")]
    private static void OpenLoader()
    {
        OpenScene("MainMenu");
    }

    [MenuItem("Scene Navigator/Intro")]
    private static void OpenAspectRatio()
    {
        OpenScene("Intro");
    }

    [MenuItem("Scene Navigator/Game Menu")]
    private static void OpenAuthentication()
    {
        OpenScene("GameMenu");
    }

    [MenuItem("Scene Navigator/Pre Level")]
    private static void OpenPreloading()
    {
        OpenScene("PreLevel");
    }

    [MenuItem("Scene Navigator/Game")]
    private static void OpenInn()
    {
        OpenScene("Game");
    }

    [MenuItem("Scene Navigator/Quiz")]
    private static void OpenDeckBuilder()
    {
        OpenScene("Quiz");
    }

    [MenuItem("Scene Navigator/Survey")]
    private static void OpenInventoryShop()
    {
        OpenScene("Survey");
    }

    private static void OpenScene(string _scene)
    {
        if (Application.isPlaying)
            return;
        
        EditorBuildSettingsScene[] allScenes = EditorBuildSettings.scenes;

        string scenePath = string.Empty;

        for (int i = 0; i < allScenes.Length; i++)
        {
            if (!allScenes[i].path.Contains(_scene))
                continue;

            scenePath = allScenes[i].path;
        }
        
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            Debug.Log(_scene + " has been opened\n");
        }
    }
}