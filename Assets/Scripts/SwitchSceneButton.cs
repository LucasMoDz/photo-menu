using UnityEngine;
using UnityEngine.UI;
using Package.CustomLibrary;

public class SwitchSceneButton : MonoBehaviour
{
    public SceneName scene;

    private void Awake()
    {
        Button btn = this.GetComponent<Button>();

        if (btn == null)
            return;

        btn.onClick.AddListener(() => { Utils.LoadScene(scene); });
    }
}