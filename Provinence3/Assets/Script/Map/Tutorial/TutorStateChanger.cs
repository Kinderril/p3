using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TutorStateChanger : MonoBehaviour
{
    private TutorialLevel level;
    public TutorialPart State;
    public void Init(TutorialLevel level)
    {
        this.level = level;
    }

    void OnTriggerEnter(Collider collider)
    {
        var hero = collider.GetComponent<Hero>();
        if (hero != null)
        {
            level.TutorialPart = State;
        }
    }
}

