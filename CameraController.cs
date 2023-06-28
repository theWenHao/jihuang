using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;//跟随的目标
    [SerializeField] float transformSpeed=5;//平滑速度
    [SerializeField] Vector3 offect;//和目标的偏移量

    private void LateUpdate()
    {
        if (target!=null)
        {
            Vector3 targetOffect= target.position + offect;  //移动后的位置
            transform.position = Vector3.Lerp(transform.position, targetOffect, transformSpeed * Time.deltaTime);  //当前坐标平滑到目标点
        }
    }
}
