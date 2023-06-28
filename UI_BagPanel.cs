using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BagPanel : MonoBehaviour
{
    public static UI_BagPanel instance;
    private UI_BagItem[] items;
    [SerializeField] GameObject itemPrefeb;
   
    private void Awake()
    {
        instance=this;
    }
    private void Start()
    {
        items = new UI_BagItem[5];
        UI_BagItem item = Instantiate(itemPrefeb,transform).GetComponent<UI_BagItem>();
        item.Init(ItemManager.Instance.GetItemDefine(ItemType.Campfire));
        items[0] = item;

        for (int i = 1; i < 5; i++)
        {
            item = Instantiate(itemPrefeb,transform).GetComponent<UI_BagItem>();
            item.Init(null);
            items[i] = item;

        }
    }
    private void Update()
    {
            
    }
    public bool AddItem(ItemType itemType)
    {
        //查看有没有空格子
        for (int i = 0; i < items.Length; i++)
        {
            //有空格子
            if (items[i].itemDefine==null)
            {
                ItemDefine itemDefine=ItemManager.Instance.GetItemDefine(itemType);
                items[i].Init(itemDefine);
                return true;
            }
        }
        return false;
    }
}
