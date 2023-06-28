using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有单位基类
/// </summary>
public class BaseObject : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    public GameObject lootObject;//掉落的物品
    //Hp的属性，判断是否死亡以及HP的更新事件。
    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            HpUpDate();
            if (hp <= 0)
            {
                Dead();
            }
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="index"></param>
    public void PlayAudio(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }
    /// <summary>
    /// hp的更新事件
    /// </summary>
    protected virtual void HpUpDate()
    {

    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void Dead()
    {
        Debug.Log("dead");
        if (lootObject!= null)
        {
            Instantiate(lootObject,
                transform.position+new Vector3(Random.Range(-0.5f,0.5f),Random.Range(1f,1.2f),Random.Range(-0.5f,0.5f)),
                Quaternion.identity);
        }
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Hurt(int damage)
    {
       Hp-=damage;
    }
    /// <summary>
    /// 添加物品
    /// </summary>

    public virtual bool AddItem(ItemType itemType)
    {
        return false;
    }
}
