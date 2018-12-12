using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public class BigMapUI :MonoBehaviour
    {
        public static BigMapUI Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public MidRightUI midRightUI;


        private void Start()
        {
            SelectionMgr.Instacne.onHover += OnHover;
            SelectionMgr.Instacne.onUnHover += OnUnHover;
            SelectionMgr.Instacne.onSelectBuilding += OnSelectBuilding;
            SelectionMgr.Instacne.onUnSelectBuilding += OnUnSelectBuilding;
            SelectionMgr.Instacne.onUnSelectTroop += OnUnSelectTroop;
            SelectionMgr.Instacne.onSelectTroop += OnSelectTroop;
            SelectionMgr.Instacne.onSelectTroopList += OnSelectTroopList;
        }

        private void OnHover(SelectAbleEntity entity)
        {

        }
        private void OnUnHover(SelectAbleEntity entity)
        {

        }
        private void OnSelectTroop(Troop troop)
        {

        }
        private void OnUnSelectTroop(Troop troop)
        {

        }

        private void OnSelectTroopList(List<Troop> troop)
        {

        }

        public void OnSelectBuilding(Building building)
        {
            midRightUI.OnSelectBuilding(building);
        }
        public void OnUnSelectBuilding(Building building)
        {
           
        }


    }
}
