using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<bool> SetGameEnabled;

    [SerializeField] private float damageMin = .2f;
    [SerializeField] private float damageMax = .6f;
    [SerializeField] private float _nextEventWaitMax = .2f;
    [SerializeField] private int _gameOverCount;

    [SerializeField] private List<Breakable> _instanceList = new List<Breakable>();

    public int InstanceDeathCount
    {
        get
        {
            var count = 0;
            foreach (var instance in _instanceList)
            {
                if (instance.isDeath)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public float Health { get { return InstanceDeathCount / _gameOverCount; } }
    public float GameTime { get { return _gameEnabled ? Time.time - _timeGameStart : 0f; } }
    public int BreakablesCount { get { return _instanceList.Count; } }
    public bool IsGameOver { get { return InstanceDeathCount >= _gameOverCount; } }

    private bool _gameEnabled;

    private float _timeGameStart;
    private float _timeNextEvent;

    private void OnEnable()
    {
        SetGameEnabled += OnSetGameEnabled;
    }

    private void OnDisable()
    {
        SetGameEnabled -= OnSetGameEnabled;
    }

    private void OnSetGameEnabled(bool value)
    {
        if (value)
        {
            Initialize();
        }

        _gameEnabled = value;
    }

    private void Update()
    {
        if (!_gameEnabled || IsGameOver)
        {
            return;
        }

        if (Time.time > _timeNextEvent)
        {
            List<Breakable> foundList = new List<Breakable>();

            foreach (var instance in _instanceList)
            {
                if (instance.isFixed())
                {
                    foundList.Add(instance);
                }
            }

            if (_instanceList.Count - foundList.Count >= _gameOverCount)
            {
                GameManager.SetGameEnabled?.Invoke(false);
                return;
            }
            if (foundList.Count == 0)
            {
                return;
            }

            var selected = _instanceList[UnityEngine.Random.Range(0, _instanceList.Count)];
            selected.InitDamage(UnityEngine.Random.Range(damageMin, damageMax));

            NextEvent();
        }
    }

    private void Initialize()
    {
        foreach (var instance in _instanceList)
        {
            instance.Initialize();
        }

        _timeGameStart = Time.time;

        NextEvent();
    }

    private void NextEvent()
    {
        _timeNextEvent = Time.time + UnityEngine.Random.Range(0, _nextEventWaitMax);
        Debug.Log("time " + Time.time + " timeNextEvent " + _timeNextEvent);
    }
}
