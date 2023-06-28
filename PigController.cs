using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PigController : BaseObject
{
    /// <summary>
    /// ����AI״̬
    /// </summary>
    public enum EnemyState
    {
        Idle,   //����
        Move,   //�ƶ�
        Pursue, //׷��
        Attack, //����
        Hurt,   //����
        Die,    //����
    }
    /// <summary>
    /// Ұ��Aides
    /// </summary>
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CheckCollider checkCollider;

    //�ж���Χ
    public float maxX = 5f;
    public float maxZ = -6f;
    public float minX = 6f;
    public float minZ = -6.33f;

    private EnemyState enemyState;
    private Vector3 target;
    //��״̬�޸��ǣ����½���״̬Ҫ��������
    //�൱��OnEnter 
    public EnemyState EnemyState1
    {
        get => enemyState;

        set
        {
            enemyState = value;
            switch (enemyState)
            {
                case EnemyState.Idle:
                    //���Ŷ���
                    //�رյ���
                    //��Ϣһ��ʱ��ȥѲ��
                    animator.CrossFadeInFixedTime("Idle", 0.25f);
                    navMeshAgent.enabled = false;
                    Invoke(nameof(GoMove), Random.Range(3f, 10f));
                    break;
                case EnemyState.Move:
                    // ���Ŷ���
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    navMeshAgent.enabled = true;
                    // ��һ��Ѳ�ߵ�
                    target = GetTargetPos();
                    navMeshAgent.SetDestination(target);
                    break;
                case EnemyState.Pursue:
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    navMeshAgent.enabled = true;
                    break;
                case EnemyState.Attack:
                    animator.CrossFadeInFixedTime("Attack", 0.25f);
                    transform.LookAt(PlayerController.instance.transform.position);
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Hurt:
                    animator.CrossFadeInFixedTime("Hurt", 0.25f);
                    PlayAudio(0);
                    navMeshAgent.enabled = false;
                    break;
                case EnemyState.Die:
                    animator.CrossFadeInFixedTime("Die", 0.25f);
                    PlayAudio(0);
                    navMeshAgent.enabled = false;
                    break;
               
            }
        }
    }
    private void Start()
    {
        checkCollider.Init(this, 10);
        EnemyState1 = EnemyState.Idle;
    }
    private void Update()
    {
        StateOnUpdate();
    }
    private void StateOnUpdate()
    {
        switch (enemyState)
        {
           /* case EnemyState.Idle:
                break;*/
            case EnemyState.Move:
                if (Vector3.Distance(transform.position,target)<1.5f)
                {
                    EnemyState1 = EnemyState.Idle;
                }
                break;
            case EnemyState.Pursue:
                //�����㹻������ʼ����
                if (Vector3.Distance(transform.position,PlayerController.instance.transform.position)<1)
                {
                    EnemyState1 = EnemyState.Attack;
                }
                else  //������������׷
                {
                    navMeshAgent.SetDestination(PlayerController.instance.transform.position);
                }
                break;
           /* case EnemyState.Attack:
                break;
            case EnemyState.Hurt:
                break;
            case EnemyState.Die:
                break;*/
            default:
                break;
        }
    }
    private void GoMove() 
    {
        EnemyState1 = EnemyState.Move;
    }
    private Vector3 GetTargetPos()
    {
        return new Vector3(Random.Range(minX, maxX),0, Random.Range(minZ, maxZ));
    }
    private void PlayAnimation(string stateName)
    {
        animator.CrossFadeInFixedTime(stateName, 0.25f);
    }
    public override void Hurt(int damage)
    {
        if (EnemyState1==EnemyState.Die)
        {
            return;
        }
        CancelInvoke(nameof(GoMove));//ȡ���л����ƶ�״̬���ӳٵ���
        base.Hurt(damage);
        if (Hp>0)
        {
            EnemyState1 = EnemyState.Hurt;                                                                                                                                                
        }
    }
    public override void Dead()
    {
        base.Dead();
        EnemyState1 = EnemyState.Die;
    }

    #region �����¼�
    private void StartHit()
    {
        checkCollider.StartHit();
    }
    private void StopHit()
    {
        checkCollider.StopHit();
    }
    private void StopAttack()
    {
        if (EnemyState1 != EnemyState.Die)
        {
            EnemyState1 = EnemyState.Pursue;
        }
    }
    private void HurtOver()
    {
        if (EnemyState1!=EnemyState.Die)
        {
            EnemyState1 = EnemyState.Pursue;
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
