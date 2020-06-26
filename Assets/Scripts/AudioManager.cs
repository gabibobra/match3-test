using UnityEngine;

public enum Clip { Select, Swap, Clear };

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource[] sfx;

    // Use this for initialization
    void Start()
    {
        instance = GetComponent<AudioManager>();
        sfx = GetComponents<AudioSource>();
    }

    public void PlayAudio(Clip audioClip)
    {
        sfx[(int)audioClip].Play();

    }
}
