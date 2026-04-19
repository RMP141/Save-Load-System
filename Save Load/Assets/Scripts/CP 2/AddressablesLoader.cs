using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesLoader : MonoBehaviour, IResourceLoader
{
    private Dictionary<string, object> _cache = new();
    private Dictionary<string, int> _refCount = new();
    private Dictionary<string, AsyncOperationHandle> _handles = new();

    public void LoadAsync<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        if (_cache.TryGetValue(key, out object cached))
        {
            _refCount[key]++;
            onLoaded?.Invoke(cached as T);
            return;
        }

        StartCoroutine(LoadCoroutine<T>(key, onLoaded));
    }

    private System.Collections.IEnumerator LoadCoroutine<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        _handles[key] = handle;

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T result = handle.Result;
            _cache[key] = result;
            _refCount[key] = 1;
            onLoaded?.Invoke(result);
        }
        else
        {
            Debug.LogError($"Failed to load resource: {key}");
            onLoaded?.Invoke(null);
        }
    }

    public void Unload(string key)
    {
        if (_handles.TryGetValue(key, out var handle))
        {
            Addressables.Release(handle);
            _handles.Remove(key);
            _cache.Remove(key);
            _refCount.Remove(key);
        }
    }

    public T GetCached<T>(string key) where T : UnityEngine.Object
    {
        if (_cache.TryGetValue(key, out object cached))
            return cached as T;
        return null;
    }

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