using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;

    public void Init(Transform target)
    {
        this.target = target;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (target != null)
	        transform.position = target.position + offset;
	}
}
