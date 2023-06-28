using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ʒ����
/// </summary>
public enum ItemType
{
    None,
    Meat,
    CookedMeat,
    Wood,
    Campfire
}
/// <summary>
/// ��Ʒ����
/// </summary>
public class ItemDefine
{
    public ItemType ItemType;
    public Sprite Icon;
    public GameObject Prefeb;

    public ItemDefine(ItemType itemType, Sprite tcon, GameObject prefeb)
    {
        ItemType = itemType;
        Icon = tcon;
        Prefeb = prefeb;
    }
}
/// <summary>
/// ��Ʒ�Ĺ�����
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    [SerializeField] Sprite[] icons;
    [SerializeField] GameObject[] prefebs;
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// ��ȡ��Ʒ����
    /// </summary>

    public ItemDefine GetItemDefine(ItemType itemType)
    {
        return new ItemDefine(itemType, icons[(int)itemType - 1], prefebs[(int)itemType-1]);
    }
}
