using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField]
    private SoundType sound = SoundType.Default;
    [SerializeField]
    private float cooldown = 0.15f;
    private float timer = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (timer > 0) return;
        timer = cooldown;
        if (collision.gameObject.CompareTag("Hammer") && collision.contacts.Length > 0)
            AudioPlayer.Instance.Play(sound, collision.contacts[0].point);
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }
}
