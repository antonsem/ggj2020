using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenCollisionRecognizer : MonoBehaviour
{

    public Breakable breakable;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
        if(breakable!=null)
        breakable.Collision(collision);
    }
}
