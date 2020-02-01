using ExtraTools;
using UnityEngine;

public enum SoundType
{
    Default,
    Anvil,
    Pipe,
    Nail,
    Reject
}

public class AudioPlayer : Singleton<AudioPlayer>
{
    [SerializeField]
    private SoundType[] types = new SoundType[] { SoundType.Default, SoundType.Anvil, SoundType.Pipe, SoundType.Nail, SoundType.Reject };
    [SerializeField, RequiredField]
    private AudioClip[] sounds;

    public void Play(SoundType type, Vector3 position)
    {
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] == type)
                Play(sounds[i], position);
        }
    }

    public void Play(in AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    private void OnValidate()
    {
        if (types.Length != sounds.Length)
        {
            Debug.LogErrorFormat("Sound types and sounds lists are different on <color=red>{0}</color>", this);
        }
    }
}
