using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���е�λ����
/// </summary>
public class BaseObject : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    public GameObject lootObject;//�������Ʒ
    //Hp�����ԣ��ж��Ƿ������Լ�HP�ĸ����¼���
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
    /// ������Ч
    /// </summary>
    /// <param name="index"></param>
    public void PlayAudio(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }
    /// <summary>
    /// hp�ĸ����¼�
    /// </summary>
    protected virtual void HpUpDate()
    {

    }

    /// <summary>
    /// ����
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
    /// ����
    /// </summary>
    /// <param name="damage"></param>
    public virtual void Hurt(int damage)
    {
       Hp-=damage;
    }
    /// <summary>
    /// �����Ʒ
    /// </summary>

    public virtual bool AddItem(ItemType itemType)
    {
        return false;
    }
}
