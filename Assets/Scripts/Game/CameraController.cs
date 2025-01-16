using QFramework.ProjectGungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class CameraController : MonoBehaviour
    {
        //EasyEvent是QFramework提供的事件系统，可以方便的进行事件的注销
        public static EasyEvent<float, int> Shake = new EasyEvent<float, int>();

        public float ShakeA = 0;
        public int ShakeFrames = 0;
        public bool Shaking = false; 

        private Camera mCamera;

        private void Start()
        {
            mCamera = GetComponent<Camera>();
            Shake.Register((a, frames) =>
            {
                Shaking = true;
                ShakeFrames = frames;
                ShakeA = a;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }



        void Update()
        {
            mCamera.orthographicSize = (1.0f - Mathf.Exp(-Time.deltaTime * 5))
                .Lerp(mCamera.orthographicSize, Global.GunAdditionalCameraSize + 8);
            if (Global.Player)
            {
                //目标位置
                var targetPosition = new Vector2(Global.Player.transform.position.x, Global.Player.transform.position.y);


                if (Shaking)
                {
                    //当前摄像机位置
                    Vector3 currentPosition = transform.position;
                    //计算当前应该到达的位置（平滑算法）
                    currentPosition = Vector2.Lerp(currentPosition, targetPosition, (1.0f - Mathf.Exp(-Time.deltaTime * 5)));
                    currentPosition.z = -10;

                    ShakeFrames--;

                    var shakeA = (ShakeFrames / 30.0f).Lerp(ShakeA, 0);//随着帧数衰减

                    currentPosition.x = currentPosition.x + UnityEngine.Random.Range(-shakeA, shakeA);
                    currentPosition.y = currentPosition.y + UnityEngine.Random.Range(-shakeA, shakeA);

                    if (ShakeFrames <= 0)
                    {
                        Shaking = false;
                    }
                    //设置位置
                    transform.position = currentPosition;
                }
                else
                {
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
    }
}