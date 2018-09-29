using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [System.Serializable]
    public class AudioClipData
    {
        public string audioName;
        [Range(0, 1)]
        public float defaultVolume;
        public AudioClip clip;
    }

    public AudioClipData[] allAudioclips;
    public List<AudioSource> sources = new List<AudioSource>();
    public AudioMixer mixer;
    public string[] questionMusicNames;
    public AudioClipData chosenRandomMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyVolumes();
    }

    public AudioClipData FindAudioClipByName(string audioName)
    {
        for (int i = 0; i < allAudioclips.Length; i++)
        {
            if (allAudioclips[i].audioName == audioName)
            {
                return allAudioclips[i];
            }
        }

        Debug.LogError("There is no audioClip with the name " + audioName);
        return null;
    }

    public AudioClipData GetRandomMusicClip()
    {
        string audioName = questionMusicNames[Random.Range(0, questionMusicNames.Length)];

        for (int i = 0; i < allAudioclips.Length; i++)
        {
            if (allAudioclips[i].audioName == audioName)
            {
                chosenRandomMusic = allAudioclips[i];
                return allAudioclips[i];
            }
        }


        Debug.LogError("There is no audioClip with the name " + audioName);
        return null;
    }

    public void PlayAudio(AudioClipData audio, bool loop, string audioGroup)
    {
        AudioSource source = FindFreeAudioSource();
        source.clip = audio.clip;
        source.loop = loop;
        source.volume = audio.defaultVolume;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(audioGroup)[0];
        source.Play();
    }

    public bool IsBeingPlayed(AudioClipData audio)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].clip.name == audio.clip.name)
            {
                if (sources[i].isPlaying)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void StopAudio(AudioClipData audio)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].clip.name == audio.clip.name)
            {
                sources[i].Stop();
                return;
            }
        }

        Debug.LogWarning("There is no audio named " + audio.audioName + " playing.");
        return;
    }

    public IEnumerator AudioFadeOut(AudioClipData audio, float fadeTime)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].clip.name == audio.clip.name)
            {
                float startVolume = sources[i].volume;

                while (sources[i].volume > 0)
                {
                    sources[i].volume -= startVolume * Time.deltaTime / fadeTime;

                    yield return null;
                }

                sources[i].Stop();
                yield break;
            }
        }

        Debug.LogWarning("There is no audio named " + audio.audioName + " playing.");
        yield break;
    }

    public IEnumerator AudioFadeIn(AudioClipData audio, float fadeTime, bool loop, string audioGroup)
    {
        AudioSource source = FindFreeAudioSource();
        source.clip = audio.clip;
        source.loop = loop;
        source.volume = 0;
        source.Play();
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(audioGroup)[0];

        float startVolume = source.volume;

        while(source.volume < audio.defaultVolume)
        {
            source.volume += Time.deltaTime / fadeTime;

            yield return null;
        }

        source.volume = audio.defaultVolume;
    }

    public void ApplyVolumes()
    {
        int master = SettingsManager.instance.masterVolume;
        int music = SettingsManager.instance.musicVolume;
        int sfx = SettingsManager.instance.sfxVolume;

        mixer.SetFloat("MasterVolume", master);
        mixer.SetFloat("MusicVolume", music);
        mixer.SetFloat("SFXVolume", sfx);
    }

    private AudioSource FindFreeAudioSource()
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].isPlaying == false)
            {
                return sources[i];
            }
        }

        GameObject newObject = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
        newObject.transform.SetParent(transform);
        int sourceNumber = sources.Count;
        newObject.name = "AudioSource (" + sourceNumber + ")";
        AudioSource newAudioSource = newObject.AddComponent<AudioSource>();
        sources.Add(newAudioSource);

        return newAudioSource;
    }
}
