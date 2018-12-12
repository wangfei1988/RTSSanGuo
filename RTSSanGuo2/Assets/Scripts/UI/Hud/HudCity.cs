using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace RTSSanGuo
{
    //
    public class HudCity :MonoBehaviour
    {
        public Slider hpslider;
        public Text textname;


        private float xOffset;
        private float yOffset;
        private RectTransform recTransform;
        private CityBuilding city;

        private void Awake()
        {
            recTransform = GetComponent<RectTransform>();
        }

        public void Init(CityBuilding city)
        {
            xOffset = city.hudXOffset;
            yOffset = city.hudYOffset;
            this.city = city;
            hpslider.value = city.CurHP / (city.MaxHP * 1.0f);
            textname.text = city.Alias;            
        }

        public void onCityInfoChange() {
            if (city) {
                hpslider.value = city.CurHP / (city.MaxHP * 1.0f);
                textname.text = city.Alias;
            }           
        }


        public  void Update()
        {
            if (city != null)
            {
                Vector2 player2DPosition = Camera.main.WorldToScreenPoint(city.transform.position);
                recTransform.position = player2DPosition + new Vector2(xOffset, yOffset);
                //血条超出屏幕就不显示
                if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
                {
                    recTransform.gameObject.SetActive(false);
                }
                else
                {
                    recTransform.gameObject.SetActive(true);
                }
                hpslider.value = city.CurHP / (city.MaxHP * 1.0f);
                textname.text = city.Alias;
            }
            else {
                recTransform.gameObject.SetActive(false);
            }
        }

    }
}
