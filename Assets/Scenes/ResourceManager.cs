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


    public Breakable GetRandomHealthyBreakable()
    {
        Breakable breakable = null;
        if (_breakableList.Count.Equals(0))
        {
            throw new System.Exception("Breakable list is empty")
        }
        while (breakable == null || !breakable.isFixed() || _breakableList.Count != 0)
        {
            breakable = GetRandomBreakable();
        }
        return breakable;
    }

}
