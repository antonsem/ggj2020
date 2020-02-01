using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixer : MonoBehaviour
{
    public enum FixerType { Hammer, Screwdriver };
    [SerializeField]
    private FixerType type = FixerType.Hammer;
    [SerializeField]
    private float healingPower = 0.2f;


    public float getHealingPower()
    { return healingPower; }
    public FixerType getFixerType()
    {
        return type;
    }

}
