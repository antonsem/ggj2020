﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public readonly float HealthMax = 1;

    [SerializeField] private float _autoDamageMin;
    [SerializeField] private float _autoDamageMax;
    [SerializeField] private float _autoDamageInterval; // seconds
    [SerializeField] private Fixer defaultFixer;
    [SerializeField] private float damageCooldown = 0.15f;
    private float damageTimer = 0;

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

    private Coroutine _autoDamage;

    public void Initialize()
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
                if (grandChildren.tag.Equals("BreakableParts"))
                {
                    parts.Add(grandChildren.gameObject);
                }
                else if (grandChildren.tag.Equals("BasePart"))
                {
                    basePart = grandChildren.gameObject;
                }
            }
        }
    }

    private void OnEnable()
    {
        GameManager.SetGameEnabled += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.SetGameEnabled -= OnGameStart;
    }

    private void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    private void OnGameStart(bool state)
    {
        if (state)
            Fix();
        else
        {
            Break();
            health = 0;
        }
    }

    public void UndoDamage(float damage)
    {

        health = Mathf.Min(health + damage, HealthMax);
        if (health <= 0)
        {
            foreach (GameObject part in parts)
            {
                Destroy(part.GetComponent<Rigidbody>());
                Destroy(part.GetComponent<ChildrenCollisionRecognizer>());
            }
            Destroy(basePart.GetComponent<Rigidbody>());
            Destroy(basePart.GetComponent<ChildrenCollisionRecognizer>());
        }
        Debug.Log("Healed " + transform.name + " for " + damage + " to " + health);
        if (isFixed())
        {
            Fix();

            if (_autoDamage != null)
            {
                StopCoroutine(_autoDamage);
                _autoDamage = null;
            }
        }
    }

    public bool isFixed()
    {
        return HealthMax.Equals(health);
    }
    public void InitDamage(float damage)
    {
        DoDamage(damage);

        if (!isFixed() && _autoDamage == null)
        {
            _autoDamage = StartCoroutine(AutoDamage());
        }
        Break();
    }

    [MyBox.ButtonMethod]
    public void FakeDamage()
    {
        DoDamage(0.2f);

        if (!isFixed() && _autoDamage == null)
        {
            _autoDamage = StartCoroutine(AutoDamage());
        }
        Break();
    }
    private void DoDamage(float damage)
    {
        if (damageTimer > 0 || isDeath) return;
        damageTimer = damageCooldown;
        health -= damage;

        Debug.Log("received damage " + damage + " health now " + health + " is death " + isDeath);
    }

    private IEnumerator AutoDamage()
    {
        while (!isDeath)
        {
            yield return new WaitForSeconds(_autoDamageInterval);

            DoDamage(Random.Range(_autoDamageMin, _autoDamageMax));
        }

        _autoDamage = null;
    }

    [MyBox.ButtonMethod]
    public void Break()
    {
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        }
        for (int i = 0; i < parts.Count; i++)
        {
            if (!parts[i])
            {
                parts.Remove(parts[i]);
                continue;
            }

            if (parts[i].TryGetComponent(out Rigidbody rb))
                rb.AddExplosionForce(Random.Range(50, 120), new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), 106f);
            else
                parts[i].AddComponent<Rigidbody>().AddExplosionForce(Random.Range(50, 120), new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), 106f);

            if (parts[i].TryGetComponent(out ChildrenCollisionRecognizer child))
                child.breakable = this;
            else
                parts[i].AddComponent<ChildrenCollisionRecognizer>().breakable = this;
        }

        if (basePart != null)
        {
            if (basePart.TryGetComponent(out Rigidbody rb))
                rb.isKinematic = true;
            else
                basePart.AddComponent<Rigidbody>().isKinematic = true;

            if (basePart.TryGetComponent(out ChildrenCollisionRecognizer child))
                child.breakable = this;
            else
                basePart.AddComponent<ChildrenCollisionRecognizer>().breakable = this;
        }
    }

    [MyBox.ButtonMethod]
    public void Fix()
    {
        foreach (GameObject part in parts)
        {
            Destroy(part.GetComponent<Rigidbody>());
            Destroy(part.GetComponent<ChildrenCollisionRecognizer>());
            AnimateToDefault(part.transform, 1.0f);
        }
        if (basePart)
        {
            Destroy(basePart.GetComponent<Rigidbody>());
            Destroy(basePart.GetComponent<ChildrenCollisionRecognizer>());
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
            if ((elapsedTime / time) > 0.98f)
            {
                objectToMove.localPosition = position;
                objectToMove.localRotation = rotation;
            }

            yield return null;
        }
    }

    public void AnimateToDefault(Transform objectToMove, float time)
    {
        StartCoroutine(MoveToPosition(objectToMove, Vector3.zero, Quaternion.identity, time));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collision(collision);
    }

    public void Collision(Collision collision)
    {
        if (collision.rigidbody && collision.rigidbody.gameObject.TryGetComponent(out Fixer fixer))
        {
            if (!isFixed() && fixer.controller)
            {
                if (fixer.controller.GotTheBeat)
                    UndoDamage(fixer.getHealingPower());
                else
                {
                    DoDamage(fixer.getHealingPower());
                    AudioPlayer.Instance.Play(SoundType.Reject, transform.position);
                }
            }
            else
                DoDamage(fixer.getHealingPower());
        }
    }
}
