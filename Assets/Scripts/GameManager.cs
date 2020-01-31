using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _numberOfBreakables = 8;
    [SerializeField] private ResourceManager _resourceManager;

    public static Action<Breakable> Death;

    private List<Breakable> _breakableList = new List<Breakable>();

    private int _totalHealthMax;
    private int _totalHealth;

    private void Start()
    {
        Death += OnDeath;
    }

    private void Update()
    {
        
    }

    private void Initialize()
    {
        // clear ??

        for (int ordinal = 0; ordinal < _numberOfBreakables; ordinal++)
        {
            var instance = Instantiate(_resourceManager.GetRandomBreakable()); // random position (?)

            _totalHealthMax += instance.HealthMax;
        }

        _totalHealth = _totalHealthMax;
    }

    private void OnDeath(Breakable breakable)
    {

    }
}
