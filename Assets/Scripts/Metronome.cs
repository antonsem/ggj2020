using ExtraTools;
using UnityEngine;

public class Metronome : Singleton<Metronome>
{
    [SerializeField]
    private float beatTime = 0.5f;
    private float timer = 0;
    [SerializeField]
    private float preBeatBuffer = 0.1f;
    private bool prebeat = false;

    public void Restart(float time = 0)
    {
        timer = time;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = beatTime;
            prebeat = false;
            Events.beat.Invoke();
        }
        else if(timer < preBeatBuffer && !prebeat)
        {
            prebeat = true;
            Events.preBeat.Invoke(preBeatBuffer);
        }
    }
}
