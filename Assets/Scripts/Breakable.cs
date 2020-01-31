using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public readonly int HealthMax = 100;

    private int health;

    private void Start()
    {
        health = HealthMax;
    }

    public void UndoDamage(int damage)
    {
        health = Mathf.Max(health + damage, HealthMax);
    }

    public bool isFixed()
    {
        return HealthMax.Equals(health);
    }

    public void DoDamage(int damage)
    {
        health = Mathf.Min(health - damage, 0);

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        GameManager.Death?.Invoke(this);
    }

}
