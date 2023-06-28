using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BagItem : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image bg;
    [SerializeField] Image iconMag;

    private bool isSelect;

    public ItemDefine itemDefine;

    public bool IsSelect { get => isSelect; 
       set 
        { 
            isSelect=value;
            if (isSelect)
            {
                bg.color = Color.green;
            }
            else
            {
                bg.color = Color.white;
            }
        }
        }
    private void Update()
    {
        if (IsSelect&&itemDefine!=null&&Input.GetMouseButtonDown(1))
        {
            if (PlayerController.instance.UseItem(itemDefine.ItemType))
            {
                Init(null);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsSelect = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsSelect = false;
    }
    /// <summary>
    /// 初始化，如果是空物体，则调用空格子逻辑
    /// </summary>
    /// <param name="itemDefine"></param>
    public void Init(ItemDefine itemDefine=null)
    {
        this.itemDefine = itemDefine;
        IsSelect=false;
        if (this.itemDefine == null)
        {
            iconMag.gameObject.SetActive(false);
        }
        else
        {

            iconMag.gameObject.SetActive(true);
            iconMag.sprite = itemDefine.Icon;
        }
     }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        PlayerController.instance.isDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        iconMag.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemDefine == null) return;
        PlayerController.instance.isDraging = false;
        //发射射线查看当前碰到的物体
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo))
        {
            string targetTag = hitInfo.collider.tag;
            iconMag.transform.localPosition = Vector3.zero;//iCon归位
            switch (itemDefine.ItemType)
            {
                case ItemType.None:
                    break;
                case ItemType.Meat:
                    if (targetTag=="Ground")
                    {
                        Instantiate(itemDefine.Prefeb, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if (targetTag == "Campfire")
                    {
                        Init(ItemManager.Instance.GetItemDefine(ItemType.CookedMeat));
                    }
                        break;
                case ItemType.CookedMeat:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefeb, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if (targetTag == "Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire>().AddWood();
                        Init(null);
                    }
                        break;
                case ItemType.Wood:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefeb, hitInfo.point + new Vector3(0, 1.5f, 0), Quaternion.identity);
                        Init(null);
                    }
                    else if (targetTag=="Campfire")
                    {
                        hitInfo.collider.GetComponent<Campfire>().AddWood();
                        Init(null);
                    }
                    break;
                case ItemType.Campfire:
                    if (targetTag == "Ground")
                    {
                        Instantiate(itemDefine.Prefeb, hitInfo.point , Quaternion.identity);
                        Init(null);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
        
	

	
    
        
    

