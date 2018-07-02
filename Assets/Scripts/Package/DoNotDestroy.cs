using UnityEngine;

/// <summary> It is used to make an object persistent. </summary>
public class DoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<DoNotDestroy>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}