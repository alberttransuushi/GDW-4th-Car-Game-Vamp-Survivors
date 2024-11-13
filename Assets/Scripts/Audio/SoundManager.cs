using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : PersistentSingleton<SoundManager>
{
    IObjectPool<SoundEmitter> soundEmitterPool;

    readonly List<SoundEmitter> activeSoundEmitters = new();

    public readonly Dictionary<SoundData, int> counts = new();

    [SerializeField] SoundEmitter soundEmitterPrefab;
    [SerializeField] bool collectionCheck = true;
    [SerializeField] int defaultCap = 10;
    [SerializeField] int maxPoolSize = 100;
    [SerializeField] int maxSoundInstances = 30;

    void Start()
    {
        InitializePool();
    }

    public SoundBuilder CreateSound() => new SoundBuilder(this);

    public bool CanPlaySound(SoundData soundData)
    {
        if (counts.TryGetValue(soundData, out var count))
        {
            if (count >= maxSoundInstances)
            {
                return false;
            }
        }
        return true;
    }

    public SoundEmitter Get()
    {
        return soundEmitterPool.Get();
    }

    public void ReturnToPool(SoundEmitter soundEmitter)
    {
        soundEmitterPool.Release(soundEmitter);
    }

    void OnDestroyPoolObject(SoundEmitter soundEmitter)
    {
        Destroy(soundEmitter.gameObject);
    }

    SoundEmitter CreateSoundEmitter()
    {
        var soundEmitter = Instantiate(soundEmitterPrefab);
        soundEmitter.gameObject.SetActive(false);
        return soundEmitter;
    }

    void OnTakeFromPool(SoundEmitter soundEmitter)
    {
        soundEmitter.gameObject.SetActive(true);
        activeSoundEmitters.Add(soundEmitter);
    }

    void OnReturnedToPool(SoundEmitter soundEmitter)
    {
        if (counts.TryGetValue(soundEmitter.soundData, out var count))
        {
            counts[soundEmitter.soundData] -= count > 0 ? 1 : 0;
        }
        soundEmitter.gameObject.SetActive(false );
        activeSoundEmitters.Remove(soundEmitter);
    }

    void InitializePool()
    {
        soundEmitterPool = new ObjectPool<SoundEmitter>(
            CreateSoundEmitter,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            collectionCheck,
            defaultCap,
            maxPoolSize);
    }
}
