using UnityEngine;
using System.Collections;

public class TestQuaternion : MonoBehaviour {

	// Use this for initialization
	void Start () {


        var a1 = Quaternion.Euler(new Vector3(0, 5, 0));
        var a2 = Quaternion.Euler(new Vector3(0, 175, 0));
	    var a3 = Quaternion.Lerp(a1, a2, 0.5f);

        Debug.Log(" " + a3 + "   " + a3.eulerAngles);

        for (int i = 0; i < 360; i++)
	    {
	        var q = Quaternion.Euler(new Vector3(0, i, 0));
//            Debug.Log(i + "  " + q);
	    }
//        Debug.Log("---------");
        float a = -1f;
        for (int i = 0; i < 20; i++)
	    {
	        var q2 = new Quaternion(0,0,0,a);
	        a += 0.1f;
//            Debug.Log(q2 + "   " + q2.eulerAngles);
	    }
	}
	
}
