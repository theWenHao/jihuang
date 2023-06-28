using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品类型
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
/// 物品定义
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
/// 物品的管理器
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
    /// 获取物品定义
    /// </summary>

    public ItemDefine GetItemDefine(ItemType itemType)
    {
        return new ItemDefine(itemType, icons[(int)itemType - 1], prefebs[(int)itemType-1]);
    }
}
