using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;//�����Ŀ��
    [SerializeField] float transformSpeed=5;//ƽ���ٶ�
    [SerializeField] Vector3 offect;//��Ŀ���ƫ����

    private void LateUpdate()
    {
        if (target!=null)
        {
            Vector3 targetOffect= target.position + offect;  //�ƶ����λ��
            transform.position = Vector3.Lerp(transform.position, targetOffect, transformSpeed * Time.deltaTime);  //��ǰ����ƽ����Ŀ���
        }
    }
}
