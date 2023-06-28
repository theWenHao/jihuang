using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    private BaseObject owner;
    public int damage;
    private bool canHit;
    //敌人的标签
    [SerializeField] List<string> enemyTags = new List<string>();
    //可以捡起的物品标签
    [SerializeField] List<string> itemTags = new List<string>();
    private List<GameObject> lastAttackObjectList=new List<GameObject>();
    public  void Init(BaseObject owner,int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }
    /// <summary>
    /// 开启伤害检测
    /// </summary>
    public void StartHit()
    {
        canHit = true;
    }
    /// <summary>
    /// 关闭伤害检测
    /// </summary>
    public void StopHit()
    {
        canHit = false;
        lastAttackObjectList.Clear();
    }
    private void OnTriggerStay(Collider other)
    {
        //如果当前允许伤害检测
        if (canHit)
        {
            //此次伤害还没被检测过&&包含敌人的标签 （可伤害标签）
            if (!lastAttackObjectList.Contains(other.gameObject)&&enemyTags.Contains(other.tag))
            {
                lastAttackObjectList.Add(other.gameObject);
                //具体的伤害
                other.GetComponent<BaseObject>().Hurt(damage);
            }
            return;
        }
        if (itemTags.Contains(other.tag))
        {
            
            //捡到物品tag转为枚举
            ItemType itemType=System.Enum.Parse<ItemType>(other.tag);
           
            if (owner.AddItem(itemType))
            {
                owner.PlayAudio(1);//播放音效
                Destroy(other.gameObject);//销毁捡到的物品 
            }
        }
    }
}
