using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    private Vector3 xyOffset;
    private Vector3 oldPos;
    private Vector3 reverseDirection;
    public CameraShake CameraShake;
    public float maxSqrDistance = 10f;
    public float frictionPower = 0.1f;

    void Awake()
    {
        if (CameraShake == null)
            CameraShake = GetComponent<CameraShake>();
    }

    public void Init(Transform target)
    {
        this.target = target;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (target != null)
	    {
	        var delta = target.position - oldPos;
	        bool withDirection = delta.sqrMagnitude > 0.001f;

            if (withDirection)
	        {
	            reverseDirection = -delta.normalized*frictionPower;
//	            oldPos = target.position;
//	            var np = target.position + offset;
//	            transform.position = np;
	        }
	        else
	        {

            }
            var xyOffsetNew = xyOffset + reverseDirection;
	        if (withDirection)
	        {
	            if (xyOffsetNew.sqrMagnitude < maxSqrDistance)
	            {
	                xyOffset = xyOffsetNew;
	            }
	        }
	        else
	        {
	            if (xyOffsetNew.sqrMagnitude > 0)
	            {
	                reverseDirection = Vector3.zero;
                    xyOffset = xyOffsetNew;
                }
	        }
            var np = target.position + offset + xyOffset;
            //            var np = transform.position + reverseDirection;
            transform.position = np;

        }
    }
}
