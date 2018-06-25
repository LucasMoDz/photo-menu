using UnityEngine;
using System.Collections;
using Package.CustomLibrary;
using Package.EventManager;

/// <summary> All events that can be invoked on Sound Manager </summary>
public enum SoundManagerTopics
{
    /// <summary> 0 Results | 1 Parameters = AudioClipName. </summary>
    PlayEffect = 0,

    /// <summary> 0 Results | 1 Parameters = AudioClipName. </summary>
    PlayMusic = 1,

    /// <summary> 1 Results = Coroutine | 1 Parameters = AudioClipName. </summary>
    SwitchMusic = 2,

    /// <summary> 0 Results | 0 Parameters. </summary>
    Mute = 3,

    /// <summary> 0 Results | 0 Parameters. </summary>
    Unmute = 4,

    /// <summary> 1 Results = Coroutine | 0 Parameters. </summary>
    FadeOutMusicRequest = 5,

    /// <summary> 1 Results = Coroutine | 1 Parameters = AudioClipName. </summary>
    FadeInMusicRequest = 6,

    /// <summary> 1 Results = Coroutine | 2 Parameters = AudioClipName, AudioClipName. </summary>
    PlayIntroAndLoopRequest = 7,

    /// <summary> 1 Results = AudioSource | 0 Parameters </summary>
    GetMusicSourceRequest = 8,

    /// <summary> 1 Results = AudioSource | 0 Parameters </summary>
    GetEffectSourceRequest = 9
}

/// <summary> Abstract sound manager at events. </summary>
public class SoundManager : BaseMonoBehaviour
{
    private float sourceMusicVolume;
    private enum SoundType { Music = 0, Effect = 1 }

    /// <summary> Clips database. </summary>
    private SoundRepo repo;
    
    protected override void PreInitialization()
    {
        base.PreInitialization();

        repo = this.GetComponent<SoundRepo>();

        if (repo.effect.source == null)
        {
            Debug.LogError("Error: Sound Manager has no effect audio source\n");
            return;
        }

        // Initialize effects source component
        repo.effect.source.loop = false;
        repo.effect.source.playOnAwake = false;

        if (repo.music.source == null)
        {
            Debug.LogError("Error: Sound Manager has no music audio source\n");
            return;
        }

        sourceMusicVolume = repo.music.source.volume;

        // Initialize musics source component
        //repo.music.source.loop = true;
        //repo.music.source.playOnAwake = true;
    }

    protected override void AddEvents()
    {
        base.AddEvents();

        ServerPattern.AddEvent<AudioClipName>(SoundManagerTopics.PlayEffect, TopicType.Event, true);
        ServerPattern.AddEvent<AudioClipName>(SoundManagerTopics.PlayMusic, TopicType.Event, true);
        ServerPattern.AddEvent<Coroutine, AudioClipName>(SoundManagerTopics.SwitchMusic, TopicType.Function, true);
        ServerPattern.AddEvent(SoundManagerTopics.Mute, TopicType.Event, true);
        ServerPattern.AddEvent(SoundManagerTopics.Unmute, TopicType.Event, true);
        ServerPattern.AddEvent<Coroutine>(SoundManagerTopics.FadeOutMusicRequest, TopicType.Function, true);
        ServerPattern.AddEvent<Coroutine, AudioClipName>(SoundManagerTopics.FadeInMusicRequest, TopicType.Function, true);
        ServerPattern.AddEvent<Coroutine, AudioClipName, AudioClipName>(SoundManagerTopics.PlayIntroAndLoopRequest, TopicType.Function, true);
        ServerPattern.AddEvent<AudioSource>(SoundManagerTopics.GetMusicSourceRequest, TopicType.Function, true);
        ServerPattern.AddEvent<AudioSource>(SoundManagerTopics.GetEffectSourceRequest, TopicType.Function, true);
    }
    
    protected override void AddListeners()
    {
        base.AddListeners();

        ServerPattern.AddListener<AudioClipName>(SoundManagerTopics.PlayEffect, _clipName => { PlayEffect(_clipName); });
        ServerPattern.AddListener<AudioClipName>(SoundManagerTopics.PlayMusic, _clipName => { PlayMusic(_clipName); });
        ServerPattern.AddListener<Coroutine, AudioClipName>(SoundManagerTopics.SwitchMusic, _clipName => StartCoroutine(SwitchMusicCO(_clipName)));
        ServerPattern.AddListener(SoundManagerTopics.Mute, ()=> { Mute(true); });
        ServerPattern.AddListener(SoundManagerTopics.Unmute, () => { Mute(false); });
        ServerPattern.AddListener(SoundManagerTopics.FadeOutMusicRequest, ()=>
        {
            StopAllCoroutines(); // ToDo: Da verificare tutte le casistiche
            return StartCoroutine(FadeOutMusicCO());
        });
        ServerPattern.AddListener<Coroutine, AudioClipName>(SoundManagerTopics.FadeInMusicRequest, _clipName => StartCoroutine(FadeInMusicCO(_clipName)));

        ServerPattern.AddListener<Coroutine, AudioClipName, AudioClipName>(SoundManagerTopics.PlayIntroAndLoopRequest, (_introClipName, _loopClipName) => StartCoroutine(PlayIntroWithFadeAndThenLoopCO(_introClipName, _loopClipName)));
        ServerPattern.AddListener(SoundManagerTopics.GetMusicSourceRequest, ()=> repo.music.source);
        ServerPattern.AddListener(SoundManagerTopics.GetEffectSourceRequest, ()=> repo.effect.source);
    }
    
    /// <summary> Play one shot an audio clip effect. </summary>
    /// <param name="_clipName"> Name of audio clip to play. </param>
    /// <param name="_volume"> Optional volume of audio clip. </param>
    private void PlayEffect(AudioClipName _clipName, float _volume = 0.0f)
    {
        if (_clipName.Equals(AudioClipName.None) || repo.effect.source.mute)
            return;

        AudioSource targetSource = repo.effect.source.clip == null ? repo.effect.source : repo.effect.source.gameObject.AddComponent<AudioSource>();
        targetSource.PlayOneShot(GetClipToEnum(_clipName, SoundType.Effect), 1);

        return;

        float defaultPitch = targetSource.pitch;
        targetSource.pitch = Random.Range(.925f, 1.075f);
        targetSource.volume = Mathf.Approximately(0.0f, _volume) ? repo.effect.source.volume : _volume;
        targetSource.clip = GetClipToEnum(_clipName, SoundType.Effect);
        targetSource.Play();
        
        if (targetSource != repo.effect.source)
        {
            UtilitiesGen.CallMethod(targetSource.clip.length + 0.5f, ()=> { Destroy(targetSource); });
        }
        else
        {
            targetSource.pitch = defaultPitch;
        }
    }

    /// <summary> Play background music without fade in. </summary>
    /// <param name="_clipName"> Name of audio clip to play on loop. </param>
    /// <param name="_volume"> Optional volume of audio clip. </param>
    private void PlayMusic(AudioClipName _clipName, float _volume = 0.0f)
    {
        repo.music.source.clip = GetClipToEnum(_clipName, SoundType.Music);

        if (!Mathf.Approximately(0.0f, _volume))
        {
            repo.music.source.volume = _volume;
        }

        repo.music.source.Play();
    }

    /// <summary> Switch music with fade out and fade in. </summary>
    /// <param name="_clipName"> Name of audio clip to be replaced by the current one. </param>
    /// <param name="_volume"> Optional volume of audio clip. </param>
    private IEnumerator SwitchMusicCO(AudioClipName _clipName, float _volume = 0.0f)
    {
        yield return StartCoroutine(FadeOutMusicCO());
        yield return StartCoroutine(FadeInMusicCO(_clipName, _volume));
    }

    /// <summary> Fade out music. </summary>
    private IEnumerator FadeOutMusicCO()
    {
        float step = 0;

        while (step < 1)
        {
            step += Time.deltaTime / ConstantValues.FADEOUTTIME * sourceMusicVolume;
            repo.music.source.volume = Mathf.Lerp(sourceMusicVolume, 0, step);
            yield return new WaitForEndOfFrame();
        }

        repo.music.source.volume = 0;
        repo.music.source.clip = null;
    }

    /// <summary> Fade in new music. </summary>
    /// <param name="_clipName"> Name of new audio clip. </param>
    /// <param name="_volume"> Optional volume of audio clip. </param>
    private IEnumerator FadeInMusicCO(AudioClipName _clipName, float _volume = 0.0f)
    {
        repo.music.source.clip = null;

        repo.music.source.volume = 0;
        repo.music.source.clip = GetClipToEnum(_clipName, SoundType.Music);
        repo.music.source.Play();
        
        float targetVolume = Mathf.Approximately(0.0f, _volume) ? sourceMusicVolume : _volume;
        float step = 0;

        while (step < 1)
        {
            step += Time.deltaTime / ConstantValues.FADEOUTTIME * sourceMusicVolume;
            repo.music.source.volume = Mathf.Lerp(0, targetVolume, step);
            yield return new WaitForEndOfFrame();
        }

        repo.music.source.volume = targetVolume;
    }

    /// <summary> Fade in of intro music and at its end play other music that will be on loop. </summary>
    /// <param name="_introClipName"> Intro clip name. </param>
    /// <param name="_loopClipName"> Loop clip name. </param>
    /// <param name="_volume"> Optional volume of audio clip. </param>
    private IEnumerator PlayIntroWithFadeAndThenLoopCO(AudioClipName _introClipName, AudioClipName _loopClipName, float _volume = 0.0f)
    {
        repo.music.source.loop = false;
        yield return StartCoroutine(FadeInMusicCO(_introClipName, _volume));
        yield return new WaitWhile(()=> repo.music.source.isPlaying);
        PlayMusic(_loopClipName, _volume);
        repo.music.source.loop = true;
    }

    /// <summary> Mute or unmute. </summary>
    /// <param name="_value"> True: Mute | False: Unmute </param>
    public void Mute(bool _value)
    {
        repo.music.source.mute = _value;
        repo.effect.source.mute = _value;
    }
    
    /// <summary> Search an audio clip inside a specific array. </summary>
    /// <param name="_clipName"> Name of audio clip. </param>
    /// <param name="_type"> Select if clip name parameter is music or effect. </param>
    /// <returns> Return audio clip if exists, otherwise null. </returns>
    private AudioClip GetClipToEnum(AudioClipName _clipName, SoundType _type)
    {
        AudioClip[] pointerClips = _type.Equals(SoundType.Music) ? repo.music.clips : repo.effect.clips;

        for (int i = 0; i < pointerClips.Length; i++)
        {
            if (!pointerClips[i].name.Equals(_clipName.ToString()))
                continue;

            return pointerClips[i];
        }

        Debug.LogError("Error: " + _clipName + " was not found, check that enum and clip name is the same\n");
        return null;
    }
}

/// <summary> Name of clips, must be equal to audio clips name of SoundRepo. </summary>
public enum AudioClipName
{
    None = -1,
    Gameplay = 0,
    IntroMenu = 1,
    Quiz = 2,
    Screenshot = 3,
    Button = 4,
    CorrectAnswer = 5,
    Tap = 6,
    Time = 7,
    WrongAnswer = 8
}