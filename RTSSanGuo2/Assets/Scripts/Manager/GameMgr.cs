#define TEestttt

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public enum EGameState {Init,Loading,Running,Pause,Finish}
    //有些不需要继承monobehavior 但是为了统一全部继承
    public class GameMgr : MonoBehaviour
    {
        public static GameMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }
        public EGameState state = EGameState.Init;

#if TEestttt

        private void Start()
        {
            StartCoroutine(StartGame());             
        }

        private IEnumerator StartGame() {
            while (!DataMgr.Instacne.hasInitAllData)
                yield return null;
            DataMgr.Instacne.LoadSaveData(1);            
            state = EGameState.Loading;
            while (DataMgr.Instacne.loadPercent<100)
                yield return null;
            state = EGameState.Running;
            SelectionMgr.Instacne.CanSelection = true;
        }

#endif
        private float speed=1f; //0.25  0.5  1  2  4     暂停时 -1 
        public float RunSpeed {
            get {
                return speed;
            }
            set {
                 speed = value;
            }
        }

        public void Stop() {
            state = EGameState.Pause;
        }
        public void Resume() {
            state = EGameState.Running;
        }
        
    }
}
