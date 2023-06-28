using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���������
/// </summary>
public class Campfire : MonoBehaviour
{
    [SerializeField] new Light light;
    private float time = 20;//���ȼ��ʱ��
    private float currenTime = 20;//��ǰʣ��ȼ��ʱ��

    private void Update()
    {
        if (currenTime<=0)
        {
            currenTime = 0;
            light.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            if (currenTime>20)
            {
                currenTime = 20;
            }
            currenTime-=Time.deltaTime;
        }
        light.intensity=Mathf.Clamp(currenTime/time,0,1)*10f;
       
    }
    public void AddWood()
    {
        currenTime += 10;
        light.transform.parent.gameObject.SetActive(true);
    }
}
