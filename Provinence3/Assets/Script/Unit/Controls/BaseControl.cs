using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class BaseControl : MapObjectWithDeath
{
    protected const string ANIM_WALK = "walk";
    private const string ANIM_DEATH = "death";
    private const string ANIM_ATTACK = "attack";
    private const string ANIM_ATTACK_ALT = "range";
    protected const float WALK = 0.000001f;

	[SerializeField] float m_MovingTurnSpeed = 660;
	[SerializeField] float m_StationaryTurnSpeed = 180;

	protected Rigidbody m_Rigidbody;
	public Animator Animator;
    public bool UseAltAttack;
    protected string attackKey;
	float m_TurnAmount;
	float m_ForwardAmount;
    private bool moving = false;
    public Vector3 Direction;
    protected Vector3 targetDirection;
    public QueaternionFromTo ThisByQuaterhnion;
//    public bool haveRagDoll = false;
    public Rigidbody RagDollRigidbody;
    public List<Rigidbody> listForRagDoll; 

    public Vector3 TargetDirection
    {
        get { return targetDirection; }
    }
    public Vector3 Velocity
    {
        get { return m_Rigidbody.velocity; }
    }

    void Awake()
	{
	    Init();
	}
    public virtual bool IsPathComplete()
    {
        return true;
    }

    public virtual void SetSpped(float speed)
    {

    }

    protected virtual void Init()
    {
        if (UseAltAttack)
        {
            attackKey = ANIM_ATTACK_ALT;
        }
        else
        {
            attackKey = ANIM_ATTACK;
        }
        if (Animator == null)
            Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        if (ThisByQuaterhnion == null)
            ThisByQuaterhnion = GetComponent<QueaternionFromTo>();
        ThisByQuaterhnion.Init(null,OnComeRotation);
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if (listForRagDoll != null)
        {
            foreach (var child in listForRagDoll)
            {
                child.isKinematic = true;
            }
        }
    }

    private void OnComeRotation()
    {

    }

    public virtual bool MoveTo(Vector3 v)
    {
        return true;
    }

    protected virtual void UpdateRotation(Vector3 d)
    {

    }
    

    public void UpdateFromUnit()
    {
        UpdateCharacter();
    }

    protected virtual void UpdateCharacter()
    {
    }

    public virtual void SetToDirection(Vector3 dir)
    {
//        Debug.Log("2 " + dir + "   prev: " + targetDirection);
        targetDirection = dir;
        ThisByQuaterhnion.SetLookDir(targetDirection);
    }
    
	protected void UpdateAnimator(Vector3 move)
	{
        float speed = move.magnitude;
	    moving = speed > WALK;
        Animator.SetBool(ANIM_WALK, moving);
	}

    public override void SetDeath()
    {
        base.SetDeath();
        ThisByQuaterhnion.enabled = false;
        if (RagDollRigidbody != null)
        {
            foreach (var child in listForRagDoll)
            {
                child.isKinematic = false;
            }
            StartCoroutine(WaitRagdoll());
            Animator.enabled = false;
            var dir = MainController.Instance.level.MainHero.transform.position - transform.position;
            RagDollRigidbody.AddExplosionForce(1,dir,2);
        }
        else
        {
            Animator.SetBool(ANIM_DEATH, true);
        }
    }

    private IEnumerator WaitRagdoll()
    {
        yield return new WaitForSeconds(1.5f);

        foreach (var child in listForRagDoll)
        {
            child.isKinematic = true;
        }
    } 

    public virtual void Stop(bool setSpeedToZero = true)
    {

    }

    public void Dead()
    {
        Stop();
    }

    public virtual void PlayAttack()
    {
        Debug.Log("PLay attack controls");
        Animator.SetTrigger(attackKey);
    }

    public void Cache()
    {
        if (RagDollRigidbody != null)
        {
            listForRagDoll = RagDollRigidbody.GetComponentsInChildren<Rigidbody>(true).ToList();
        }
        else
        {
            listForRagDoll = null;
        }

    }
}

