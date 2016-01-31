using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class BaseControl : MonoBehaviour
{
    private const string ANIM_WALK = "walk";
    private const string ANIM_DEATH = "death";
    protected const string ANIM_ATTACK = "attack";
    private const float WALK = 0.000001f;

	[SerializeField] float m_MovingTurnSpeed = 660;
	[SerializeField] float m_StationaryTurnSpeed = 180;

	protected Rigidbody m_Rigidbody;
	public Animator Animator;
	float m_TurnAmount;
	float m_ForwardAmount;
    private bool moving = false;
    public Vector3 Direction;
    protected Vector3 targetDirection;
    public QueaternionFromTo ThisByQuaterhnion;

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
        if (Animator == null)
            Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        ThisByQuaterhnion = GetComponent<QueaternionFromTo>();
        ThisByQuaterhnion.Init(null,OnComeRotation);
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        
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

    public void SetDeath()
    {
        ThisByQuaterhnion.enabled = false;
        Animator.SetBool(ANIM_DEATH,true);
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
        Animator.SetTrigger(ANIM_ATTACK);
    }
}

