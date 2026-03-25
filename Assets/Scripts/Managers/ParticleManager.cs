using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField] private List<ParticleDataSO> effects;

    private Dictionary<string, ParticleDataSO> lookup = new Dictionary<string, ParticleDataSO>();
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

        foreach (var effect in effects)
            lookup[effect.effectName] = effect;
    }

    public void PlayEffect(string name, Vector3 pos)
    {
        if (!lookup.TryGetValue(name, out ParticleDataSO data))
        {
            Debug.LogWarning("Particle effect not found: " + name);
            return;
        }

        ParticleSystem ps = Instantiate(data.prefab, pos, Quaternion.identity);

        // Randomization
        var main = ps.main;

        if (data.sizeVariance > 0)
            main.startSizeMultiplier += Random.Range(-data.sizeVariance, data.sizeVariance);

        if (data.speedVariance > 0)
            main.startSpeedMultiplier += Random.Range(-data.speedVariance, data.speedVariance);

        // Custom lifetime
        if (data.useCustomLifetime)
            Destroy(ps.gameObject, data.lifeTime);
        else
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax + 0.1f);

        ps.Play();
    }
}
