using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    private List<MeshRenderer> _meshRendererList = new List<MeshRenderer>();
    private Collider _collider;

    private void Awake()
    {
        _meshRendererList.AddRange(GetComponentsInChildren<MeshRenderer>());
        _collider = GetComponentInChildren<Collider>();
    }

    private void OnEnable()
    {
        GameManager.SetGameEnabled += OnSetGameEnabled;
    }

    private void OnDisable()
    {
        GameManager.SetGameEnabled += OnSetGameEnabled;
    }

    private void Update()
    {
        transform.Rotate(0f, .4f, 0f);
    }

    private void OnSetGameEnabled(bool value)
    {
        foreach (var meshRenderer in _meshRendererList)
        {
            meshRenderer.enabled = !value;
        }

        _collider.enabled = !value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.SetGameEnabled?.Invoke(true);
    }
}
