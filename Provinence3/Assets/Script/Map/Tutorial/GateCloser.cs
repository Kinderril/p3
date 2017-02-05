using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GateCloser : MonoBehaviour
{
    public Gate Gate;

    void OnTriggerEnter(Collider collider)
    {
        var hero = collider.GetComponent<Hero>();
        if (hero != null)
        {
            Gate.Close();
        }
    }
}

