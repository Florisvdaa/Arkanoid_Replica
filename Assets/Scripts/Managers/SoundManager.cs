using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Library")]
    [SerializeField] private List<SoundDataSO> sounds;

    private Dictionary<string, SoundDataSO> soundLookup = new Dictionary<string, SoundDataSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach (var sound in sounds)
            soundLookup[sound.name] = sound;
    }

    public void PlaySFX(string name)
    {
        if(!soundLookup.TryGetValue(name, out SoundDataSO data))
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        AudioSource src = Instantiate(sfxSource, transform);
        src.clip = data.clip;

        float randomPitch = data.pitch + Random.Range(-data.pitchVariance, data.pitchVariance);
        src.pitch = randomPitch;
        src.volume = data.volume;

        src.Play();
        Destroy(src.gameObject, data.clip.length + 0.1f);
    }

    public void PlayMusic(string name)
    {
        if (!soundLookup.TryGetValue(name, out SoundDataSO data))
        {
            Debug.LogWarning("Music not found: " + name);
            return;
        }

        musicSource.clip = data.clip;
        musicSource.volume = data.volume;
        musicSource.loop = true;
        musicSource.Play();
    }
}
