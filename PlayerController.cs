using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : BaseObject
{
    public static PlayerController instance; //单例化
    [SerializeField] Animator animator;//动画控制器
    [SerializeField] CheckCollider checkCollider; //获取武器
    [SerializeField] CharacterController characterController;
    [SerializeField] float moveSpeed;
    [SerializeField] float hungry;
    private bool isAttacking;
    private bool isHurting;
    public bool isDraging;
    private Quaternion targetDirQuaternion;
    [SerializeField] UnityEngine.UI.Image hpImage;
    [SerializeField] UnityEngine.UI.Image hungryImage;


    public WeaponTrail myTrail;
    private float t = 0.033f;
    private float tempT = 0;
    private float animationIncrement = 0.003f;

    void LateUpdate()
    {
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (myTrail.time > 0)
                {
                    myTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (myTrail.time > 0)
            {
                myTrail.UpdateTrail(Time.time, t);
            }
        }
    }
    public float Hungry { get => hungry; 
        set 
        { 
            hungry = value;
            
        }
    }

    private void Awake()
    {
        instance = this;
        checkCollider.Init(this, 50);//武器的伤害
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        // 默认没有拖尾效果
        myTrail.SetTime(0.0f, 0.0f, 1.0f);
    }
    private void Update()
    {
        UpdateHungry();
        if (!isAttacking&&!isHurting)  //即不再攻击中也不在受伤中。
        {
            Move();
            Attack();
        }
        else  //攻击过程中
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 10);
            
        }
        
    }
    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        //移动控制
        float a = Input.GetAxisRaw("Horizontal");
        float b = Input.GetAxisRaw("Vertical");
        //玩家没有移动
        if (a==0&b==0)
        {
            animator.SetBool("Walk", false);
        }
        else  //玩家进行操作
        {
            animator.SetBool("Walk", true);
            //获取目标方向，并过渡过去。
            targetDirQuaternion = Quaternion.LookRotation(new Vector3(a, 0, b));
            transform.localRotation=Quaternion.Slerp(transform.localRotation,targetDirQuaternion,Time.deltaTime*10f);

            characterController.SimpleMove(new Vector3(a,0,b)*moveSpeed);
        }
    }
    /// <summary>
    /// 攻击
    /// </summary>
    private void Attack()
    {//检测攻击按键
        if (Input.GetMouseButton(0))
        {
            //当前鼠标在UI上
            if (isDraging||UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            //转向
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo,100,LayerMask.GetMask("Ground")))  //射线检测
            {
                //碰到地面了
                animator.SetTrigger("Attack");
                isAttacking = true;//进入攻击状态
                //转向到攻击点
                targetDirQuaternion = Quaternion.LookRotation(hitInfo.point - transform.position);
            }
            
        }
        
    }
    private void UpdateHungry()
    {
        hungry -= Time.deltaTime * 3;
        if (hungry <= 0)
        {
            hungry = 0;
            Hp -= Time.deltaTime / 2;
        }
        hungryImage.fillAmount = hungry / 100;

    }
    protected override void HpUpDate()
    {
        hpImage.fillAmount = Hp / 100;
    }
    public override void Hurt(int damage)
    {
        base.Hurt(damage);
        animator.SetTrigger("Hurt");
        PlayAudio(2);
        isHurting = true;
    }
    public override bool AddItem(ItemType itemType)
    {   //检测背包能不能放下
        return UI_BagPanel.instance.AddItem(itemType);
    }
    public bool UseItem(ItemType itemType)
    {
        switch (itemType)
        {

            case ItemType.Meat:
                Hp -= 10;
                hungry += 20;
                break;
            case ItemType.CookedMeat:
                Hp += 40;
                hungry += 40;
                break;
            case ItemType.Wood:
                Hp -= 40;
                break;
            default:
                break;
        }
        return true;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 50, 50), "攻击"))
        {
            animator.SetTrigger("Attack");
        }
    }
    #region 动画事件
    private void StartHit()
    {
        //设置拖尾时长
        myTrail.SetTime(2.0f, 0.0f, 1.0f);
        //开始进行拖尾
        myTrail.StartTrail(0.5f, 0.4f);
        //播放音效

        PlayAudio(0);
        //攻击检测
        
        checkCollider.StartHit();
        
    }
    private void StopHit()
    {
        //停止攻击
        //清除拖尾
        myTrail.ClearTrail();
        isAttacking = false;
        checkCollider.StopHit();
        
    }
    private void HurtOver()
    {
        isHurting = false;
    }
    #endregion


}
