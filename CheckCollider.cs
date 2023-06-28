using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private BaseObject owner;
    public int damage;
    private bool canHit;
    //���˵ı�ǩ
    [SerializeField] List<string> enemyTags = new List<string>();
    //���Լ������Ʒ��ǩ
    [SerializeField] List<string> itemTags = new List<string>();
    private List<GameObject> lastAttackObjectList=new List<GameObject>();
    public  void Init(BaseObject owner,int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }
    /// <summary>
    /// �����˺����
    /// </summary>
    public void StartHit()
    {
        canHit = true;
    }
    /// <summary>
    /// �ر��˺����
    /// </summary>
    public void StopHit()
    {
        canHit = false;
        lastAttackObjectList.Clear();
    }
    private void OnTriggerStay(Collider other)
    {
        //�����ǰ�����˺����
        if (canHit)
        {
            //�˴��˺���û������&&�������˵ı�ǩ �����˺���ǩ��
            if (!lastAttackObjectList.Contains(other.gameObject)&&enemyTags.Contains(other.tag))
            {
                lastAttackObjectList.Add(other.gameObject);
                //������˺�
                other.GetComponent<BaseObject>().Hurt(damage);
            }
            return;
        }
        if (itemTags.Contains(other.tag))
        {
            
            //����ƷtagתΪö��
            ItemType itemType=System.Enum.Parse<ItemType>(other.tag);
           
            if (owner.AddItem(itemType))
            {
                owner.PlayAudio(1);//������Ч
                Destroy(other.gameObject);//���ټ񵽵���Ʒ 
            }
        }
    }
}
