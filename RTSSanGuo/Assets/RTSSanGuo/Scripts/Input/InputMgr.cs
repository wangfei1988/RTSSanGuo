using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSSanGuo
{
    public class InputMgr 
    {
        public static InputMgr _instance = new InputMgr(); //这些在Awake之前执行
        public InputMgr Instance
        {
            get
            {
                return _instance;
            }
        }
        public InputMgr() { }


        public void LoadConfig() {

        }

        ////Camera Input
        //public KeyCode CamPanningKey = KeyCode.Space;
        //public KeyCode CamPanningKey = KeyCode.Space;
        //public bool IsCamPanningKeyInput() {
        //    if()
        //}



    }
}