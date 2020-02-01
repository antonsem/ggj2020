using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<Breakable> Death;

    [SerializeField] private int _numberOfBreakables = 6;
    [SerializeField] private float _gameDuration = 60f;
    [SerializeField] private float _nextEventWaitMax = .2f;
    [SerializeField] private int damageMin = 5;
    [SerializeField] private int damageMax = 50;
    [SerializeField] private ResourceManager _resourceManager;

    //public members for use with GUI

    public float GameTime
    {
        get { return _gameInitialized ? Time.time - _timeGameStart : 0f; }
    }
    public float Health
    {
        get { return _totalHealth / _totalHealthMax; }
    }
    public int BreakablesCount
    {
        get { return _breakableList.Count; }
    }
    public int BreakablesCountDeath
    {
        get
        {
            var count = 0;

            foreach (var breakable in _breakableList)
            {
                if (breakable.isDeath)
                {
                    count++;
                }
            }

            return count;
        }
    }

    // internal private members

    // aliases
    private Camera _camera;

    // game variables
    private bool _gameInitialized;
    private float _timeGameStart;
    private float _timeNextEvent;
    private int _totalHealthMax;
    private int _totalHealth;

    // instances
    private List<Breakable> _breakableList = new List<Breakable>();

    private int _lockFuse = 64; // debug

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        Initialize();

        Death += OnDeath;
    }

    private void Update()
    {
        if (!_gameInitialized || _lockFuse <= 0 || IsGameOver())
        {
            return;
        }

        var time = Time.time;

        if (time > _timeNextEvent)
        {
            Breakable breakable = null;
            do
            {
                breakable = _breakableList[UnityEngine.Random.Range(0, _breakableList.Count)];
            } while (breakable.isDeath && !IsGameOver());

            breakable.DoDamage(UnityEngine.Random.Range(damageMin, damageMax));
            _lockFuse--;

            NextEvent();
        }
    }

    private bool IsGameOver()
    {
        foreach (var breakable in _breakableList)
        {
            if (!breakable.isDeath)
            {
                return false;
            }
        }

        return true;
    }

    private void Initialize()
    {
        for (int count = 0; count < _numberOfBreakables; count++)
        {
            var position = UnityEngine.Random.onUnitSphere + _camera.transform.position;
            var rotation = Quaternion.identity;

            var instance = Instantiate(_resourceManager.GetRandomBreakable(), position, rotation);

            _breakableList.Add(instance);
            _totalHealthMax += instance.HealthMax;
        }

        _totalHealth = _totalHealthMax;

        _timeGameStart = Time.time;
        _gameInitialized = true;

        NextEvent();
    }

    private void NextEvent()
    {
        _timeNextEvent = Time.time + UnityEngine.Random.Range(0, _nextEventWaitMax);
        Debug.Log("time " + Time.time + " timeNextEvent " + _timeNextEvent);
    }

    //public Breakable GetRandomHealthyBreakable()
    //{
    //    Breakable breakable = null;
    //    if (_breakableList.Count.Equals(0))
    //    {
    //        throw new System.Exception("Breakable list is empty");
    //    }
    //    while (breakable == null || !breakable.isFixed() || _breakableList.Count != 0)
    //    {
    //        breakable = GetRandomBreakable();
    //    }
    //    return breakable;
    //}

    private void OnDeath(Breakable breakable)
    {

    }
}
