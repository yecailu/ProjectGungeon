using QFramework.ProjectGungeon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QFramework.ProjectGungeon
{
    public class CameraController : MonoBehaviour
    {
        public List<Color> Colors;

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

            Room.OnRoomEnter.Register((room) =>
            {
                if(room.ColorIndex == -1)
                {
                    room.ColorIndex = Random.Range(0, Colors.Count);
                }
                 
                var currentColor = mCamera.backgroundColor;
                var dstColor = Colors[room.ColorIndex];
                ActionKit.Lerp(0, 1, 0.5f, (p) =>
                {
                    mCamera.backgroundColor = Color.Lerp(currentColor, dstColor, p);
                }, () =>
                {
                    mCamera.backgroundColor = dstColor;
                })  
                .Start(this);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }



        void Update()
        {
            mCamera.orthographicSize = (1.0f - Mathf.Exp(-Time.deltaTime * 5))
                .Lerp(mCamera.orthographicSize, Global.GunAdditionalCameraSize + 8);
            if (Global.Player) 
            {
                //目标位置
                var targetPosition = new Vector2(Global.Player.transform.position.x, Global.Player.transform.position.y) + Global.CameraPosOffset;


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
                //如果房间存在
                if (Global.CurrentRoom)
                {
                    //主角于房间的方向
                    var direction = Global.Player.Direction2DFrom(Global.CurrentRoom);
                    //房间宽度，最后一个数值可以更改摆动幅度
                    var width = Global.CurrentRoom.Config.Width * 0.5f * 10;
                    if(direction.x < 0)
                    {
                        var originAngleZ = transform.localEulerAngles.z;
                        var targetAngleZ = (direction.x.Abs() / width).Lerp(0, 2.5f);
                        if (originAngleZ >= 2.6f) originAngleZ -= 360;

                        transform.LocalEulerAnglesZ((1.0f - Mathf.Exp(-Time.deltaTime * 5))
                            .Lerp(originAngleZ, targetAngleZ));
                    }
                    else
                    {

                        var originAngleZ = transform.localEulerAngles.z;
                        var targetAngleZ = (direction.x.Abs() / width).Lerp(0, -2.5f);
                        if (originAngleZ >= 2.6f) originAngleZ -= 360;

                        transform.LocalEulerAnglesZ((1.0f - Mathf.Exp(-Time.deltaTime * 5))
                            .Lerp(originAngleZ, targetAngleZ));
                    }



                }
             
            }
        }
    }
}