using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace RTSSanGuo
{
    // 其实我们这里比一般rts还要简单
    //多远 框选只对Unit有效。 其他只能点选。  极端点可以取消框选，但是这里还是保留
    public class SelectionMgr : MonoBehaviour
    {
        public static SelectionMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }
       

        public LayerMask RaycastLayerMask; // Selection层 和Terrain 层都选中
        private RaycastHit Hit;
        private Ray RayCheck;

        public SelectAbleEntity hoveredEntiy = null; //hover 只能hover一个
        public Building selectedBuilding = null; //不可移动
        public List<Troop> selectedTroopList = new List<Troop>(); //可移动 如果有多选则UI 和事件必须写在这里
                                                                  //Building 和Troop 不可能同时选中

        public Transform moveToObject;

        [HideInInspector]
        private bool canSelection = false;
        public bool  CanSelection {
            set{
                canSelection = value;
                if (!canSelection) {
                    UnSelecteAllEntity();
                    UnHoverEntity();
                }
            }
            get {
                return canSelection;
            }           
        }
       
        //Selection Box: 直接固定写死
        public Image SelectionBox;
        public RectTransform Canvas;
        private bool hasCreatedSelectionBox = false; 
        private bool showSelectionBox = false;
        public  float MinBoxSize = 1.0f; //Holds the minimal selection box to draw it.
        private Vector3 FirstMousePos;
        private Vector3 LastMousePos;


        //右键点击一个
        private void TroopActionOnEntity(SelectAbleEntity entity) {
            //Debug.Log("Troop Action on Entity");
            foreach (Troop troop in selectedTroopList)
            {
                if (troop != null) {
                    if (entity.selectType == ESelectType.Troop) {
                        Troop targetTroop = entity as Troop;
                        if (targetTroop.CanBeAttack)
                            troop.AttackTroop(targetTroop);
                        else
                            troop.FollowTroop(targetTroop);

                    } else if (entity.selectType == ESelectType.Building) {
                        Building targetBuilding = entity as Building;
                        if (targetBuilding.CanBeAttack)
                            troop.AttackBuilding(targetBuilding);
                        else if (targetBuilding.CanTroopMoveInto)
                            troop.MoveInToBuilding(targetBuilding);
                        else
                            troop.MoveToPoint(targetBuilding.transform.position);
                    }
                }
            }
        }

        private void TroopActionOnPoint(Vector3 point)
        {
            //Debug.Log("Troop Action on Poinr" +point.ToString());
            foreach (Troop troop in selectedTroopList)
            {
                if (troop != null)
                {
                   // Debug.Log("troop move to" + troop.entityname + point.ToString());
                    troop.MoveToPoint(point);
                }
            }
          
        }

        private void BuildingActionOnPoint(Vector3 point)
        {
            if (selectedBuilding != null &&selectedBuilding.type==EBuildingType.City)
            {
                selectedBuilding.SetTroopRollyPoint(point);
            }
        }

        public Action<Building> onSelectBuilding;
        public Action<Building> onUnSelectBuilding;
        private void SelectBuilding(Building build) {
            if (!build.CanBeSelect) return;
            UnSelecteAllEntity();
            selectedBuilding = build;
            onSelectBuilding(build);
        }

        private void SelectTroop(Troop troop)
        {
            if (!troop.CanBeSelect) return;
            Debug.Log("select troop" + troop.entityname);
            List<Troop> list = new List<Troop>();
            list.Add(troop);
            SelectTroopList(list);
        }

        public Action<Troop> onSelectTroop;
        public Action<Troop> onUnSelectTroop;
        public Action<List<Troop>> onSelectTroopList;
        private void SelectTroopList(List<Troop> listTroop)
        {
            UnSelecteAllEntity();
            selectedTroopList = listTroop;
            onSelectTroopList(listTroop);//UI监听
            foreach (Troop troop in listTroop) {
                onSelectTroop(troop);
            }

        }

        private void UnSelecteAllEntity()
        {
            onUnSelectBuilding(selectedBuilding);
            selectedBuilding = null;
            foreach (Troop troop in selectedTroopList)
            {
                onUnSelectTroop(troop);
            }
            selectedTroopList.Clear();
        }

        public Action<SelectAbleEntity> onHover;
        public Action<SelectAbleEntity> onUnHover;//UI 和模型都需要订阅这个事件 hover只是头顶和模型底部   select 是头顶+模型底部+右侧UI，可同时存在
        //三国类型的hover其实没啥用，slect才有用
        private void HoverEntity(SelectAbleEntity entity)
        {
            if (hoveredEntiy != entity)//hover需要判断，点击不需要判断。重复点击，重复出发音效
            {   
                UnHoverEntity();//先取消之前的hover
                hoveredEntiy = entity;
                entity.Hover();
                onHover(entity);
            }
        }       
        private void UnHoverEntity()
        {
            if (hoveredEntiy != null) {
                hoveredEntiy.UnHover();
                onUnHover(hoveredEntiy);
            }               
            hoveredEntiy = null;
        }



        void Update()
        {
            //If the game is not running
            if (GameMgr.Instacne.state != EGameState.Running||!canSelection) return;

            if (!EventSystem.current.IsPointerOverGameObject()) //没有在UI物体上
            {
                // 点击事件--而且只针对ButtonDown
                RayCheck = Camera.main.ScreenPointToRay(Input.mousePosition);
               // Debug.DrawRay(RayCheck.origin, RayCheck.direction);
                if (Physics.Raycast(RayCheck, out Hit, Mathf.Infinity, RaycastLayerMask.value))
                {
                    Debug.DrawRay(RayCheck.origin, RayCheck.direction * 2000, Color.red);
                    //Debug.Log(Hit.transform.name+ "sssssssssss");                     
                    SelectAbleEntity HitObj = null;
                    if(Hit.transform.parent)
                        HitObj = Hit.transform.parent.GetComponent<SelectAbleEntity>();
                    if(HitObj==null)
                        HitObj = Hit.transform.GetComponent<SelectAbleEntity>();
                    //选择碰撞体
                    if (Input.GetMouseButtonDown(1))//右键Action
                    {
                        moveToObject.position = Hit.point;
                        if (HitObj != null && selectedTroopList.Count > 0) //右键点击中某个对象  执行Action
                        {
                            TroopActionOnEntity(HitObj);
                        }
                        else //啥也没点击中移动
                        {
                            if (selectedTroopList.Count > 0)  //Troop 和Building 不会共存，就算共存也是Toop优先
                                TroopActionOnPoint(Hit.point);
                            else if (selectedBuilding != null)
                            {
                                BuildingActionOnPoint(Hit.point);
                            }
                        }
                    }
                    else if (Input.GetMouseButtonDown(0)) //左键点击  只有点击下的那一瞬间会出发  ButtonUp不会触发   SelectionBox是针对ButtonUp
                    {
                        Debug.Log(Hit.transform.name + "sssssssssss" +"  " +HitObj);
                        if (HitObj != null)
                        {
                            if (HitObj.selectType == ESelectType.Building)
                                SelectBuilding(HitObj as Building);
                            else
                            {
                                Debug.Log(Hit.transform.name + "sssssssssss");
                                SelectTroop(HitObj as Troop);
                            }
                        }
                        else //一个都没选中，取消选择
                            UnSelecteAllEntity();
                    }
                    else if (!showSelectionBox)
                    { //
                        if (HitObj != null) // Hover 没有出现框选才可以hover 
                            HoverEntity(HitObj);
                        else
                            UnHoverEntity();
                    }

                }
            }
            else {
                //如果要使用这个判断，那就不能存在全屏 UI矩阵
                //Debug.Log("SSSSSSSSSSS"+EventSystem.current.IsPointerOverGameObject() + Input.mousePosition.ToString());
            }
            if (true) return;

            //Selection Box: ************************
            if (Input.GetMouseButton(0))
            {
                if (hasCreatedSelectionBox == false) //这个不代表显示,按下一瞬间这个hasCreatedSelectionBox=true,这个是显示FirstMousePos，而且只要ButtonUp 这个就会变成false
                {
                    FirstMousePos = Input.mousePosition;
                    hasCreatedSelectionBox = true;
                }
                //Check if the box size is above the minimal size.
                if (Vector3.Distance(FirstMousePos, Input.mousePosition) > MinBoxSize)
                {
                    showSelectionBox = true;
                    UnHoverEntity(); //出现框选要取消hover事件
                    //Activate the selection box object if it's not activated.
                    if (SelectionBox.gameObject.activeSelf == false)
                    {
                        SelectionBox.gameObject.SetActive(true);
                    }
                    LastMousePos = Input.mousePosition; //Always save the last mouse position.

                    //Calculate the box's size in the canvas:
                    Vector3 CurrentMousePosUI = Input.mousePosition - Canvas.localPosition;
                    Vector3 FirstMousePosUI = FirstMousePos - Canvas.localPosition;

                    //Set the selection box size in the canvas.
                    SelectionBox.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(CurrentMousePosUI.x - FirstMousePosUI.x), Mathf.Abs(CurrentMousePosUI.y - FirstMousePosUI.y));

                    Vector3 Center = (FirstMousePos + Input.mousePosition) / 2 - Canvas.localPosition; //Calculate the center of the selection box.
                    SelectionBox.GetComponent<RectTransform>().localPosition = Center; //Set the selection position.
                }
            }
            if (Input.GetMouseButtonUp(0) && showSelectionBox == true) //上面只是画图，和选择无关
            {
                hasCreatedSelectionBox = false;
                showSelectionBox = false;
                SelectionBox.gameObject.SetActive(false);
                //这里我们逆向思维，不使用碰撞检测
                List<Troop> tempTroopList = new List<Troop>();
                foreach (KeyValuePair<int, Troop> pair in EntityMgr.Instacne.dic_Troop)
                {
                    Vector3 wordPos = pair.Value.rectSelectedPoint.position;
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(wordPos);
                    if ((screenPos.x > FirstMousePos.x && screenPos.x < LastMousePos.x) || (screenPos.x < FirstMousePos.x && screenPos.x > LastMousePos.x))
                    {
                        if ((screenPos.y > FirstMousePos.y && screenPos.y < LastMousePos.y) || (screenPos.y < FirstMousePos.y && screenPos.y > LastMousePos.y))
                            tempTroopList.Add(pair.Value);
                    }
                }
                if (tempTroopList.Count > 0)
                {
                    UnSelecteAllEntity();
                    
                    SelectTroopList(tempTroopList);
                }
            }

        }

    }
}
