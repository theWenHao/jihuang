using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : BaseObject
{
    public static PlayerController instance; //������
    [SerializeField] Animator animator;//����������
    [SerializeField] CheckCollider checkCollider; //��ȡ����
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
        checkCollider.Init(this, 50);//�������˺�
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Ĭ��û����βЧ��
        myTrail.SetTime(0.0f, 0.0f, 1.0f);
    }
    private void Update()
    {
        UpdateHungry();
        if (!isAttacking&&!isHurting)  //�����ٹ�����Ҳ���������С�
        {
            Move();
            Attack();
        }
        else  //����������
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 10);
            
        }
        
    }
    /// <summary>
    /// �ƶ�
    /// </summary>
    private void Move()
    {
        //�ƶ�����
        float a = Input.GetAxisRaw("Horizontal");
        float b = Input.GetAxisRaw("Vertical");
        //���û���ƶ�
        if (a==0&b==0)
        {
            animator.SetBool("Walk", false);
        }
        else  //��ҽ��в���
        {
            animator.SetBool("Walk", true);
            //��ȡĿ�귽�򣬲����ɹ�ȥ��
            targetDirQuaternion = Quaternion.LookRotation(new Vector3(a, 0, b));
            transform.localRotation=Quaternion.Slerp(transform.localRotation,targetDirQuaternion,Time.deltaTime*10f);

            characterController.SimpleMove(new Vector3(a,0,b)*moveSpeed);
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    private void Attack()
    {//��⹥������
        if (Input.GetMouseButton(0))
        {
            //��ǰ�����UI��
            if (isDraging||UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            //ת��
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo,100,LayerMask.GetMask("Ground")))  //���߼��
            {
                //����������
                animator.SetTrigger("Attack");
                isAttacking = true;//���빥��״̬
                //ת�򵽹�����
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
    {   //��ⱳ���ܲ��ܷ���
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
        if (GUI.Button(new Rect(0, 0, 50, 50), "����"))
        {
            animator.SetTrigger("Attack");
        }
    }
    #region �����¼�
    private void StartHit()
    {
        //������βʱ��
        myTrail.SetTime(2.0f, 0.0f, 1.0f);
        //��ʼ������β
        myTrail.StartTrail(0.5f, 0.4f);
        //������Ч

        PlayAudio(0);
        //�������
        
        checkCollider.StartHit();
        
    }
    private void StopHit()
    {
        //ֹͣ����
        //�����β
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
