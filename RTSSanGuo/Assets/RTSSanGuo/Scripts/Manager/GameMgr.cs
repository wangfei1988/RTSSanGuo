using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo
{
   public enum EGameState {Init,Loading, Running, Paused, Finish }
   //全局控制， 其实game 开发很多和资源  坐标位置有很大关系，很多逻辑都绑在一起

   public  class GameMgr:MonoBehaviour
   {        
        public static GameMgr Instance = null; //这些在Awake之前执行
        public EGameState gameState = EGameState.Init;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
                Instance = this;
            else
                Debug.LogError("more than one instance");
        }


        public void PauseGame() {
            Time.timeScale = 0f;
            gameState = EGameState.Paused;
        }

        public void ContinueGame() {
            Time.timeScale = 1f;
            gameState = EGameState.Running;
        }

       
          

    }
}
