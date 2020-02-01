using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField]
    private SoundType sound = SoundType.Default;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hammer") && collision.contacts.Length > 0)
            AudioPlayer.Instance.Play(sound, collision.contacts[0].point);
    }
}
