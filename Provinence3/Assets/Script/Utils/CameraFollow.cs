using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Unit targetTransform;
    public Vector3 offset;
    public Vector3 xyOffset;
    private Vector3 oldPos;
    public Vector3 cureentDir;
    public CameraShake CameraShake;
    private float maxSqrDistance = 3f;
    private float maxDstance;
    private float maxWaitTime = 1.3f;
    public float frictionPower = 0.04f;
    private float expireTimeUpd;
    public Vector3 lastTarget;
    public bool withExpandLook;

    void Awake()
    {
        maxDstance = Mathf.Sqrt(maxSqrDistance);
        if (CameraShake == null)
            CameraShake = GetComponent<CameraShake>();
    }

    public void Init(Unit target)
    {
        this.targetTransform = target;
        targetTransform.OnUnitAttack += OnUnitAttack;
        targetTransform.OnUnitDestroy += OnUnitDestroy;
    }

    private void OnUnitDestroy()
    {
        targetTransform.OnUnitAttack -= OnUnitAttack;
        targetTransform.OnUnitDestroy -= OnUnitDestroy;
    }

    private void OnUnitAttack(Vector3 dir)
    {
        expireTimeUpd = Time.time + maxWaitTime;
        lastTarget = dir.normalized * maxDstance;
    }

    void Update ()
	{
	    if (targetTransform != null)
	    {
	        if (withExpandLook)
	        {
	            var dir = (lastTarget - xyOffset).normalized;
	            if (expireTimeUpd > Time.time)
	            {
	                cureentDir = dir*frictionPower;
	            }
	            else
	            {
	                cureentDir = -dir*frictionPower;
	            }
	            bool withDirection = dir.sqrMagnitude > 0.001f;

	            Vector3 xyOffsetNew = xyOffset + cureentDir;
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
	                    cureentDir = Vector3.zero;
	                    xyOffset = xyOffsetNew;
	                }
	            }
	        }

	        var np = targetTransform.transform.position + offset + xyOffset;
            transform.position = np;

        }
    }
}
