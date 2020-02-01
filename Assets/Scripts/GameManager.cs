using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int _instanceCount;
    [SerializeField] private float damageMin = .2f;
    [SerializeField] private float damageMax = .6f;
    [SerializeField] private float _nextEventWaitMax = .2f;
    [SerializeField] private int _gameOverCount;

    [Header("References")]
    [SerializeField] private ResourceManager _resourceManager;

    private List<Breakable> _instanceList = new List<Breakable>();

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
    public float GameTime { get { return _gameInitialized ? Time.time - _timeGameStart : 0f; } }
    public int BreakablesCount { get { return _instanceList.Count; } }
    public bool IsGameOver { get { return InstanceDeathCount >= _gameOverCount; } }

    private bool _gameInitialized;

    private float _timeGameStart;
    private float _timeNextEvent;

    private int _cycleFuse = 64; // debug

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!_gameInitialized || IsGameOver || _cycleFuse <= 0)
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

            if (foundList.Count == 0)
            {
                return;
            }

            var selected = _instanceList[Random.Range(0, _instanceList.Count)];
            selected.InitDamage(Random.Range(damageMin, damageMax));

            _cycleFuse--;

            NextEvent();
        }
    }

    private void Initialize()
    {
        for (int count = 0; count < _instanceCount; count++)
        {
            var position = Random.onUnitSphere + Camera.main.transform.position;
            var rotation = Quaternion.identity;

            var instance = Instantiate(_resourceManager.GetRandomBreakable(), position, rotation);

            _instanceList.Add(instance);
        }

        _timeGameStart = Time.time;

        NextEvent();

        _gameInitialized = true;
    }

    private void NextEvent()
    {
        _timeNextEvent = Time.time + Random.Range(0, _nextEventWaitMax);
        Debug.Log("time " + Time.time + " timeNextEvent " + _timeNextEvent);
    }
}
