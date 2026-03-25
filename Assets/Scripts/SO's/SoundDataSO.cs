using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "ScriptableObjects/SoundData")]
public class SoundDataSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
    public bool loop = false;

    [Header("Randomization")]
    public float pitchVariance = 0.1f;
}
