using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceService : MonoBehaviour
{
    public static ResourceService Instance { get; private set; }

    [SerializeField] private ResourcesLoader _resourcesLoader; // Прямая ссылка на конкретный загрузчик

    private IResourceLoader _loader;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Просто присваиваем, так как ResourcesLoader точно реализует IResourceLoader
        _loader = _resourcesLoader;

        if (_loader == null)
            Debug.LogError("ResourceService: не назначен ResourcesLoader!");
    }

    // Остальные методы без изменений
    public void Load<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        if (_loader == null)
        {
            Debug.LogError("Resource loader not set!");
            onLoaded?.Invoke(null);
            return;
        }
        _loader.LoadAsync(key, onLoaded);
    }

    public void Preload(List<string> keys, Action onComplete = null)
    {
        StartCoroutine(PreloadCoroutine(keys, onComplete));
    }

    private System.Collections.IEnumerator PreloadCoroutine(List<string> keys, Action onComplete)
    {
        int loaded = 0;
        foreach (var key in keys)
        {
            bool done = false;
            _loader.LoadAsync<UnityEngine.Object>(key, (obj) =>
            {
                loaded++;
                done = true;
            });
            yield return new WaitUntil(() => done);
        }
        onComplete?.Invoke();
    }

    public void Unload(string key) => _loader?.Unload(key);
    public void Release(string key) => _loader?.Release(key);
    public T GetCached<T>(string key) where T : UnityEngine.Object => _loader?.GetCached<T>(key);
}