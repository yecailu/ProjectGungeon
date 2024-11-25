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
            //Ŀ��λ��
            var targetPosition = new Vector2(Global.Player.transform.position.x, Global.Player.transform.position.y);
            //��ǰ�����λ��
            Vector3 currentPosition = transform.position;
            //���㵱ǰӦ�õ����λ�ã�ƽ���㷨��
            currentPosition = Vector2.Lerp(currentPosition, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
            currentPosition.z = -10;
            //����λ��
            transform.position = currentPosition;
        }
    }
}
