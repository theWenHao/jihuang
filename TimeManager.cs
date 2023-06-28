using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] Light sunLight;

    public float dayTime;
    public float dayToNightTime;
    public float nightToDaytTime;
    public float nightTime;

    private float lightValue = 1;
    private int dayNum;
    [SerializeField] Image timeStateImg;
    [SerializeField] Text dayNumText;
    [SerializeField] Sprite[] dayStateSprites;//�����ͼƬ

    private bool isDay=true;

    public bool IsDay { get => isDay; 
        set 
        { 
            isDay = value;
            if (IsDay)
            {
                dayNum += 1;
                dayNumText.text="Day"+dayNum;
                timeStateImg.sprite = dayStateSprites[0];
            }
            else
            {
                timeStateImg.sprite = dayStateSprites[1];
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        IsDay = true;
        //����ʱ��
        StartCoroutine(UpdateTime());
    }
    private IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return null;
            //����
            if (IsDay)
            {
                lightValue -= 1 / dayToNightTime * Time.deltaTime;
                SetLightValue(lightValue);
                if (lightValue<=0)
                {
                    IsDay = false;
                    yield return new WaitForSeconds(nightTime);//�ȴ�ҹ���ȥ
                }
            }
            else
            {
                lightValue += 1 / nightToDaytTime * Time.deltaTime;
                SetLightValue(lightValue);
                if (lightValue > 1)
                {
                    IsDay =true;
                    yield return new WaitForSeconds(dayTime);//�ȴ������ȥ
                }
            }
        }
    }
    /// <summary>
    /// ���õƹ��ֵ
    /// </summary>

    private void SetLightValue(float value)
    {
        RenderSettings.ambientIntensity = value;
        sunLight.intensity = value;
    }
}
