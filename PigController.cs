using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PigController : BaseObject
{
    /// <summary>
    /// 敌人AI状态
    /// </summary>
    public enum EnemyState
    {
        Idle,   //待机
        Move,   //移动
        Pursue, //追击
        Attack, //攻击
        Hurt,   //受伤
        Die,    //死亡
    }
    /// <summary>
    /// 野猪Aides
    /// </summary>
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CheckCollider checkCollider;

    //行动范围
    public float maxX = 5f;
    public float maxZ = -6f;
    public float minX = 6f;
    public float minZ = -6.33f;

    private EnemyState enemyState;
    private Vector3 target;
    //当状态修改是，重新进入状态要做得事情
    //相当于OnEnter 
    public EnemyState EnemyState1
    {
        get => enemyState;

        set
        {
            enemyState = value;
            switch (enemyState)
            {
                case EnemyState.Idle:
                    //播放动画
                    //关闭导航
                    //休息一段时间去巡逻
                    animator.CrossFadeInFixedTime("Idle", 0.25f);
                    navMeshAgent.enabled = false;
                    Invoke(nameof(GoMove), Random.Range(3f, 10f));
                    break;
                case EnemyState.Move:
                    // 播放动画
                    animator.CrossFadeInFixedTime("Move", 0.25f);
                    navMeshAgent.enabled = true;
                    // 找一个巡逻点
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
                //距离足够近，开始攻击
                if (Vector3.Distance(transform.position,PlayerController.instance.transform.position)<1)
                {
                    EnemyState1 = EnemyState.Attack;
                }
                else  //不够进，继续追
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
        CancelInvoke(nameof(GoMove));//取消切换到移动状态的延迟调用
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

    #region 动画事件
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
