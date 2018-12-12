using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace RTSSanGuo
{
    //
    public class HudTroop :MonoBehaviour
    {

        public Slider hpslider;
        public Text textname;

        private float xOffset;
        private float yOffset;
        private RectTransform recTransform;
        private Troop troop;

        private void Awake()
        {
            recTransform = GetComponent<RectTransform>();
        }

        public void Init(Troop troop)
        {
            xOffset = troop.hudXOffset;
            yOffset = troop.hudYOffset;
            this.troop = troop;
            hpslider.value = troop.CurHP / (troop.MaxHp * 1.0f);
            textname.text = troop.TroopName;
        }

        public void onTroopInfoChange() {
            if (troop) {
                hpslider.value = troop.CurHP / (troop.MaxHp * 1.0f);
                textname.text = troop.TroopName;
            }
        }

        public  void Update()
        {
            if (troop)
            {
                Vector2 player2DPosition = Camera.main.WorldToScreenPoint(troop.transform.position);
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
                hpslider.value = troop.CurHP / (troop.MaxHp * 1.0f);
                textname.text = troop.TroopName;
            }
            else {
                recTransform.gameObject.SetActive(false);
            }
        }

    }
}
