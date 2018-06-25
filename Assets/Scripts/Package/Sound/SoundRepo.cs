using System;
using UnityEngine;
using Package.CustomLibrary;

public class SoundRepo : MonoBehaviour
{
    public SoundClass music;
    public SoundClass effect;

    [Serializable]
    public struct SoundClass
    {
        public AudioSource source;
        [Reorderable] public AudioClip[] clips;
    }
}