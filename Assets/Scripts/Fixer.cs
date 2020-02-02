using UnityEngine;

public class Fixer : MonoBehaviour
{
    [SerializeField]
    private float healingPower = 0.2f;
    public ControllerBeat controller;
    [SerializeField, ExtraTools.RequiredField]
    private GameObject beatIndicator;

    public float getHealingPower()
    { return healingPower; }

    private bool visualize = false;

    private void OnEnable()
    {
        Events.beat.AddListener(Beat);
    }

    private void OnDisable()
    {
        Events.beat.RemoveListener(Beat);
    }

    private void Beat()
    {
        visualize = true;
    }

    private void Update()
    {
        if (visualize && controller && controller.GotTheBeat)
            beatIndicator.SetActive(false);
        else if (!controller || !controller.GotTheBeat)
        {
            beatIndicator.SetActive(true);
            visualize = false;
        }
    }
}
