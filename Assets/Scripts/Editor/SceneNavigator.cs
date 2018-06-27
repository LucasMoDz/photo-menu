using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

/// <summary> It is used to switch scene quickly with a menu item. </summary>
public class SceneNavigator : EditorWindow
{
    [MenuItem("Scene Navigator/Authentication")]
    private static void OpenAspectRatio()
    {
        OpenScene("Authentication");
    }

    [MenuItem("Scene Navigator/Main Menu")]
    private static void OpenMainMenu()
    {
        OpenScene("MainMenu");
    }

    private static void OpenScene(string _scene)
    {
        if (Application.isPlaying)
        {
            Debug.LogError("Cannot switch scene in Play Mode\n");
            return;
        }
        
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