using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<bool> SetGameEnabled;
    public static Action<int> SetGameScore;

    [SerializeField] private float damageMin = .2f;
    [SerializeField] private float damageMax = .6f;
    [SerializeField] private float _nextEventWaitMax = .2f;
    [SerializeField] private int _gameOverCount;

    [SerializeField] private List<Breakable> _instanceList = new List<Breakable>();

    [SerializeField] private bool _autostart;

    private Coroutine _scoreCounter;

    private void Start()
    {
        if (_autostart)
        {
            SetGameEnabled?.Invoke(true);
        }
    }

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

    public int InstanceFixedCount
    {
        get
        {
            var count = 0;
            foreach (var instance in _instanceList)
            {
                if (instance.isFixed())
                {
                    count++;
                }
            }

            return count;
        }
    }

    public int InstanceDamagedCount
    {
        get
        {
            var count = 0;
            foreach (var instance in _instanceList)
            {
                if (!instance.isFixed() && !instance.isDeath)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public float Health {
        get
        {
            var segmentSize = 1f / _gameOverCount;
            var instanceDeathCount = InstanceDeathCount;

            var undeadCount = (_instanceList.Count - instanceDeathCount) + 1;

            var progressMicro = (undeadCount -InstanceDamagedCount) / (float) undeadCount;
            var progressMacro = (_gameOverCount - instanceDeathCount) / (float)_gameOverCount;

            var progress = Mathf.Max(progressMacro - segmentSize + progressMicro * segmentSize, 0f);
            return progress;
        }
    }
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
        if (_scoreCounter != null)
        {
            StopCoroutine(_scoreCounter);
            _scoreCounter = null;
        }

        if (value)
        {
            Initialize();

            _scoreCounter = StartCoroutine(ScoreCounter());
        }

        _gameEnabled = value;
    }

    private void Update()
    {
        Debug.Log("HEALTH: " + Health);
        ScoreWheelController.SetScoreWheelValue?.Invoke(Health);

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

            var selected = foundList[UnityEngine.Random.Range(0, foundList.Count)];
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

    private IEnumerator ScoreCounter()
    {
        var score = 0;

        while (true)
        {
            yield return new WaitForSeconds(1f);

            score += InstanceFixedCount;
            GameManager.SetGameScore?.Invoke(score);
        }
    }

    private void NextEvent()
    {
        _timeNextEvent = Time.time + UnityEngine.Random.Range(0, _nextEventWaitMax);
        Debug.Log("time " + Time.time + " timeNextEvent " + _timeNextEvent);
    }
}
