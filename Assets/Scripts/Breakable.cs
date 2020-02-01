using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public readonly float HealthMax = 1;
    [SerializeField]
    private Fixer.FixerType fixerType = Fixer.FixerType.Hammer;
    public bool isDeath
    {
        get
        {
            return health <= 0;
        }
    }

    private float health;
    private GameObject basePart;
    private List<GameObject> parts = new List<GameObject>();

    private void Start()
    {
        health = HealthMax;

        initParts(transform);
    }

    private void initParts(Transform t)
    {
        foreach (Transform children in t)
        {
            foreach (Transform grandChildren in children)
            {
                Debug.Log(grandChildren.name);
                if (grandChildren.tag.Equals("BreakableParts"))
                {
                    Debug.Log("Object added");
                    parts.Add(grandChildren.gameObject);
                }
                else if (grandChildren.tag.Equals("BasePart"))
                {
                    basePart = grandChildren.gameObject;
                }
            }

        }
    }

    public void UndoDamage(float damage)
    {

        health = Mathf.Min(health + damage, HealthMax);
        Debug.Log("Healed " + transform.name + " for " + damage + " to " + health);
        if(isFixed())
        {
            Fix();
        }
    }

    public bool isFixed()
    {
        return HealthMax.Equals(health);
    }

    public void DoDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);

        if (health <= 0)
        {
            Death();
        }

        Debug.Log("received damage " + damage + " health now " + health + " is death " + isDeath);
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
        basePart.AddComponent<Rigidbody>().isKinematic = true;
    }

    [MyBox.ButtonMethod]
    public void Fix()
    {
        foreach (GameObject part in parts)
        {
            Destroy(part.GetComponent<Rigidbody>());
            Destroy(basePart.GetComponent<Rigidbody>());
            AnimateToDefault(part.transform, 1.0f);

        }
    }

    public IEnumerator MoveToPosition(Transform objectToMove, Vector3 position, Quaternion rotation, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.localPosition;
        Quaternion startingRot = objectToMove.localRotation;
        while (elapsedTime < time)
        {
            objectToMove.localPosition = Vector3.Lerp(startingPos, position, (elapsedTime / time));
            objectToMove.localRotation = Quaternion.Lerp(startingRot, rotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void AnimateToDefault(Transform objectToMove, float time)
    {
        StartCoroutine(MoveToPosition(objectToMove, Vector3.zero, Quaternion.identity, time));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isFixed())
        {
            Fixer fixer = collision.gameObject.GetComponent<Fixer>();

            if (fixer != null && fixer.getFixerType().Equals(fixerType))
            {
                UndoDamage(fixer.getHealingPower());
            }

        }
    }

    [MyBox.ButtonMethod]
    void FakeHitWithHammer()
    {
        if (!isFixed())
        {
            Fixer fixer = new Fixer();

            if (fixer != null && fixer.getFixerType().Equals(fixerType))
            {
                UndoDamage(fixer.getHealingPower());
            }

        }
    }
}
