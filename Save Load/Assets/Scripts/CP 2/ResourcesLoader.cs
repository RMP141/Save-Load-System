using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour, IResourceLoader
{
    private Dictionary<string, UnityEngine.Object> _cache = new();
    private Dictionary<string, int> _refCount = new();

    public void LoadAsync<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        if (_cache.TryGetValue(key, out UnityEngine.Object cached))
        {
            _refCount[key]++;
            onLoaded?.Invoke(cached as T);
            return;
        }

        StartCoroutine(LoadResourcesCoroutine<T>(key, onLoaded));
    }

    private System.Collections.IEnumerator LoadResourcesCoroutine<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(key);
        yield return request;

        T resource = request.asset as T;
        if (resource != null)
        {
            _cache[key] = resource;
            _refCount[key] = 1;
            onLoaded?.Invoke(resource);
        }
        else
        {
            Debug.LogError($"Resource {key} not found in Resources.");
            onLoaded?.Invoke(null);
        }
    }

    public void Unload(string key)
    {
        if (_cache.ContainsKey(key))
        {
            Resources.UnloadAsset(_cache[key]);
            _cache.Remove(key);
            _refCount.Remove(key);
        }
    }

    public T GetCached<T>(string key) where T : UnityEngine.Object =>
        _cache.TryGetValue(key, out UnityEngine.Object obj) ? obj as T : null;

    public void Release(string key)
    {
        if (_refCount.ContainsKey(key))
        {
            _refCount[key]--;
            if (_refCount[key] <= 0)
                Unload(key);
        }
    }
}