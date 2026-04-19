using UnityEngine;

public class TestResourceLoad : MonoBehaviour
{
    private void Start()
    {
        ResourceService.Instance.Load<AudioClip>("TestAudio", OnAudioLoaded);
    }

    private void OnAudioLoaded(AudioClip clip)
    {
        if (clip != null)
        {
            Debug.Log("Audio loaded successfully: " + clip.name);
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
        else
        {
            Debug.LogError("Failed to load TestAudio!");
        }
    }
}