using ExtraTools;
using UnityEngine;

public class SparkGenerator : Singleton<SparkGenerator>
{
    [SerializeField, MyBox.ReadOnly]
    private float timer = 0;
    [SerializeField]
    private float defaultTimer = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (timer > 0) return;
        timer = defaultTimer;

        if (collision.contacts.Length > 0)
        {
            ParticleSystem sys = ExtraTools.ResourceManager.Instance.spark.Get(collision.contacts[0].point + collision.contacts[0].normal * 0.025f).GetComponent<ParticleSystem>();
            sys.Play();
            sys.transform.up = collision.contacts[0].normal;

        }
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }
}
