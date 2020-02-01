using UnityEngine.Events;
using UnityEngine;

public class Emmiter : UnityEvent
{ }

public class FloatEmmiter : UnityEvent<float>
{ }

public static class Events
{
    public static Emmiter beat = new Emmiter();
    public static FloatEmmiter preBeat = new FloatEmmiter();
}
