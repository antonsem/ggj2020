using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // prefabs
    [SerializeField] private List<Breakable> _breakableList = null;

    public Breakable GetRandomBreakable()
    {
        return _breakableList[UnityEngine.Random.Range(0, _breakableList.Count)];
    }
}
