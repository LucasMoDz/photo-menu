using UnityEngine;
using UnityEngine.UI;
using Package.EventManager;

public class SoundButton : BaseMonoBehaviour
{
    [SerializeField] private AudioClipName clip;
    [SerializeField] private bool isOverlappable = true;

    protected override void Initialization()
    {
        base.PreInitialization();

        Button button = this.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("There's no button on '" + this.gameObject.name + "'\n");
            return;
        }

        if (isOverlappable)
        {
            button.onClick.AddListener(() => { ServerPattern.Invoke(SoundManagerTopics.PlayEffect, clip); });
        }
        else
        {
            button.onClick.AddListener(() =>
            {
                if (ServerPattern.Invoke<AudioSource>(SoundManagerTopics.GetEffectSourceRequest).isPlaying)
                    return;

                ServerPattern.Invoke(SoundManagerTopics.PlayEffect, clip);
            });
        }
    }
}