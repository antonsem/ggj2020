using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public readonly int HealthMax = 100;

    private int health;

    private List<GameObject> parts;

    private void Start()
    {
        health = HealthMax;
        initParts(transform);
    }

    private void initParts(Transform t)
    {
        /*foreach (Transform children in t)
        {
            if (t.tag.Equals("BreakableParts"))
            {
                Debug.Log("Object added");
                parts.Add(t.gameObject);
            }
            initParts(t);
        }*/
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

    [MyBox.ButtonMethod]
    public void Break()
    {
        foreach (GameObject part in parts)
        {
            part.AddComponent<Rigidbody>();
        }
    }

    [MyBox.ButtonMethod]
    public void Fix()
    {
        foreach (GameObject part in parts)
        {
            Destroy(part.GetComponent<Rigidbody>());
            AnimateToDefault(part.transform, 3.0f);
        }


    }

    public IEnumerator MoveToPosition(Transform objectToMove, Vector3 position, Quaternion rotation, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.position;
        Quaternion startingRot = objectToMove.rotation;
        while (elapsedTime < time)
        {
            objectToMove.position = Vector3.Lerp(startingPos, position, (elapsedTime / time));
            objectToMove.rotation = Quaternion.Lerp(startingRot, rotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void AnimateToDefault(Transform objectToMove, float time)
    {
        StartCoroutine(MoveToPosition(objectToMove, Vector3.zero, Quaternion.identity, time));
    }

}
