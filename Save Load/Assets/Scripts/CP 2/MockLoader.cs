using System;
using System.Collections.Generic;
using UnityEngine;

public class MockLoader : MonoBehaviour, IResourceLoader
{
    private Dictionary<string, UnityEngine.Object> _fakeCache = new();

    public void LoadAsync<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        StartCoroutine(FakeLoadCoroutine<T>(key, onLoaded));
    }

    private System.Collections.IEnumerator FakeLoadCoroutine<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
    {
        yield return new WaitForSeconds(0.1f);

        if (_fakeCache.TryGetValue(key, out UnityEngine.Object cached))
        {
            onLoaded?.Invoke(cached as T);
            yield break;
        }

        T fake = null;
        if (typeof(T) == typeof(GameObject))
            fake = new GameObject("Fake " + key) as T;
        else if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
            fake = ScriptableObject.CreateInstance(typeof(T)) as T;
        else
            fake = new GameObject("Fake " + key).AddComponent(typeof(T)) as T;

        _fakeCache[key] = fake;
        onLoaded?.Invoke(fake);
    }

    public void Unload(string key) => _fakeCache.Remove(key);
    public T GetCached<T>(string key) where T : UnityEngine.Object => _fakeCache.TryGetValue(key, out UnityEngine.Object obj) ? obj as T : null;
    public void Release(string key) { /* ничего */ }
}