using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulsTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(i+ "  "+  Formuls.LevelUpCost(i) + "    " + Formuls.LevelGoldAv(i));
        }
	}
	
}
