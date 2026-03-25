using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticleDataSO", menuName = "ScriptableObjects/Particle Effect")]
public class ParticleDataSO : ScriptableObject
{
    public string effectName;
    public ParticleSystem prefab;

    [Header("Optional Overrides")]
    public float lifeTime = 2f;
    public bool useCustomLifetime = false;

    [Header("Randomization")]
    public float sizeVariance = 0f;
    public float speedVariance = 0f;
}
