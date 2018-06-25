using UnityEngine;

/// <summary> It is used to make an object persistent. </summary>
public class DoNotDestroy : MonoBehaviour
{
    private void Awake()
    {
       DontDestroyOnLoad(this.gameObject);
    }
}