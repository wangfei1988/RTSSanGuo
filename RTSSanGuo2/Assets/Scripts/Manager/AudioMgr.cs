using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public class AudioMgr :MonoBehaviour
    {
        public static AudioMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public GameObject backGroundMusicObj;
        public GameObject effectAudioObj;//音效  普通音效   点击 选中。     不包括UI ，UI的绑定在UI自己上面。
        public GameObject skillEffectAudioObj;// 优先级更高音效  尽量不要2个同时出现。 全局的一定能听到，绑定在具体对象上的，必须audioListener靠近才可以


        public AudioClip ProductionBuildingSelectionAudio; //Audio played when the building is selected.
        public AudioClip ProductionBuildingStartBuildAudio; //Audio played when the building is selected.
        public AudioClip ProductionBuildingStartUpGradeAudio; //When the building upgrade starts. 自动修复
        public AudioClip ProductionBuildinHealthFullAudio; //When the building has been upgraded.

        public AudioClip WeaponBuildingSelectionAudio; //Audio played when the building is selected.
        public AudioClip WeaponBuildingStartBuildAudio; //Audio played when the building is selected.
        public AudioClip WeaponBuildinHealthFullAudio; //When the building has been upgraded.


        public AudioClip TroopSelectionAudio; //Audio played when the building is selected.
        public AudioClip TroopAttackOrderSound; //played when the unit is ordered to attack
        public AudioClip TroopMoveToOrderSound; //这些都是鼠标 动作音效,不是Troop 本身的移动音效，本身音效放到Troop对象上
        public AudioClip TroopInValidActionSound; //When the building has been upgraded.


        public  void PlayAudio(GameObject SourceObj, AudioClip Clip, bool Loop)
        {
            //First make sure that the source object has an audio source component:
            //Also make there audio clips available to play:
            if (SourceObj != null)
            {
                if (SourceObj.GetComponent<AudioSource>() && Clip != null)
                {
                    AudioSource AudioSrc = SourceObj.GetComponent<AudioSource>();
                    AudioSrc.Stop(); //Stop the current audio clip from playing.

                    //Randomly pick an audio clip from the cosen list
                    AudioSrc.clip = Clip;
                    AudioSrc.loop = Loop; //Set the loop settings

                    AudioSrc.Play(); //Play it.
                }
            }
        }

        public  void StopAudio(GameObject SourceObj)
        {
            //First make sure that the source object has an audio source component:
            if (SourceObj != null)
            {
                if (SourceObj.GetComponent<AudioSource>())
                {
                    //Stop playing audio:
                    SourceObj.GetComponent<AudioSource>().Stop();
                }
            }
        }




    }
}
