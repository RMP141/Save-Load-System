using System;

public interface IResourceLoader
{
    void LoadAsync<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object;
    void Unload(string key);
    T GetCached<T>(string key) where T : UnityEngine.Object;
    void Release(string key);
}