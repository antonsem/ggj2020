using UnityEngine;

public class Fixer : MonoBehaviour
{
    [SerializeField]
    private float healingPower = 0.2f;
    public ControllerBeat controller;

    public float getHealingPower()
    { return healingPower; }
}
