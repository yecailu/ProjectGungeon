using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    void Start()
    {
        
    }


    
    void Update()
    {
        if (Global.Player)
        {
            //目标位置
            var targetPosition = new Vector2(Global.Player.transform.position.x, Global.Player.transform.position.y);
            //当前摄像机位置
            Vector3 currentPosition = transform.position;
            //计算当前应该到达的位置（平滑算法）
            currentPosition = Vector2.Lerp(currentPosition, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
            currentPosition.z = -10;
            //设置位置
            transform.position = currentPosition;
        }
    }
}
