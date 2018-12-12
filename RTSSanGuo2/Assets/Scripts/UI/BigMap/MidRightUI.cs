using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSSanGuo
{
    public class MidRightUI : MonoBehaviour
    {

        public MidRightUI_City ui_city;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnSelectBuilding(Building building)
        {
            ui_city.gameObject.SetActive(true);
            ui_city.Init(building as CityBuilding);
        }


    }
}
