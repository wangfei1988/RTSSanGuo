using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RTSSanGuo {

public class MidRightUI_City : MonoBehaviour {

    // Use this for initialization

        public Button btn_buildTroop;
        private CityBuilding city;
        private void Awake()
        {
            btn_buildTroop.onClick.AddListener( BuildTroop); 
        }

        public void Init(CityBuilding building) {
            this.city = building;
        }

        public void BuildTroop() {
            if (city)
                city.BuildTroop(city.ID); //这里先简单起见 prefab 类型就是cityid 类型
        }
    
}
}