using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace RTSSanGuo { 
    public class RtsCameraCtr : MonoBehaviour {

        [Header("General Settings:")]
        public Camera MainCamera; //Main camera object  这个建议设置为本身
        public bool useDefaultPos = true; //使用默认位置则初始化不会调节位置，否则会调节位置为下面设置
        public Vector2 CameraInitPos = new Vector2(100f, 100f); //默认地图大小是200x200米  这个位置刚好在中心
        public float CameraHeight; //Main camera's height. 这个是固定的，Zoom只能设置FOV 移动只能移动x z平面
        public Vector3 EuraAngle = new Vector3(45.0f, 0f, 0f);  //RTS 地平面一般是平的，这个旋转的45 度刚好是斜视，比较好的上帝视角

        [Header("Camera Movement:")]
        public float MvtSpeed = 1.00f; //Camera movement speed;  移动通过方向键移动                                 
        public KeyCode MoveUpKey = KeyCode.UpArrow;
        public KeyCode MoveDownKey = KeyCode.DownArrow;
        public KeyCode MoveRightKey = KeyCode.RightArrow;
        public KeyCode MoveLeftKey = KeyCode.LeftArrow;
        //screen edge movement: 鼠标到达屏幕边缘也会移动
        public bool MoveOnScreenEdge = false; //Move the camera when the mouse is the screen edge.
        public bool IgnoreUI = false;
        public int ScreenEdgeSize = 25; //Screen edge size.离屏幕边缘25个像素算移动
        //Camera position limits: //The Y axis here refers to the Z axis of the camera position. 摄像机移动范围限制
        public bool ScreenLimit = true;
        public Vector2 MinPos;
        public Vector2 MaxPos;

        Rect DownRect;
        Rect UpRect;
        Rect LeftRect;
        Rect RightRect;
        bool CanMoveOnEdge = true;

       

        [Header("Panning Movement:")]
        //Panning: 允许平移
        public bool Panning = true;
        //public KeyCode PanningKey = KeyCode.Space;
        public string PanningKey = "CameraPan"; //平移按钮名字
        Vector2 MouseAxis;
        public float PanningSpeed = 15f;

        [Header("Zoom:")]
        //Camera zoom in/zoom out:  Zoom可以调节Y轴位置，也可以调节FOV
        public bool ZoomEnabled = true; //Is zooming enabled?                                       
        public bool CanZoomWithKey = true;  //can zoom with keys?        
        public KeyCode ZoomInKey = KeyCode.PageUp;  //Zoom keys:
        public KeyCode ZoomOutKey = KeyCode.PageDown;
        public float MaxFOV = 60.0f; //Maximum value of the field of view
        public float MinFOV = 40.0f; //Minimum value of the field of view  大概相当于放大2倍
                                     //Zooming in/out smooth damp vars:
        public float ZoomSmoothTime = 1.0f;
        float ZoomVelocity;
        //Use mouse wheel for zooming in and out?
        public bool ZoomOnMouseWheel = true;
        public float ZoomScrollWheelSensitivty = 5.0f; //sensitivity for zooming in/out with mouse scroll wheel
        public float ZoomScrollWheelSpeed = 15.0f; //speed for zooming with the scrol wheel.
        float FOV; //current field of view.

        //旋转部分--这个比较复杂 暂时设置不可旋转
        public Transform target;
        public float rotateSpeed = 5f;
        public float minEularX = 30f;
        public float maxEularX = 90f; //这个已经是2D 俯视


        [Header("Follow Unit:")]
        //Follow unit:
        public bool CanFollowUnit = true; //can follow a unit.
        [HideInInspector]
        public Unit UnitToFollow; // the unit to follow.
        public KeyCode FollowUnitKey = KeyCode.Space;

        [Header("Minimap Camera:")]
        //Minimap:
        public Camera MinimapCam;
        public float OffsetX;
        public float OffsetZ;
        //UI: 
        public Canvas MinimapCanvas;
        public Image MinimapCursor;

        [Header("UI Camera:")]
        public Camera UICam; //this camera shows all the UI elements in the screen + the mvt target obj

        bool Moved = false; //has the camera moved?

        //other components 这里重新写一次只是缩写。 宁外可以快速看到这个东西用到了哪些Manager 没用到的就不用再写了
        SelectionManager SelectionMgr;
        TerrainManager TerrainMgr;
        MovementManager MvtMgr;

        void Start()
        {
         
            UnitToFollow = null;
           // SelectionMgr = GameManager.Instance.SelectionMgr;
         //   TerrainMgr = GameManager.Instance.TerrainMgr;
           // MvtMgr = MovementManager.Instance;

            if (MainCamera == null)
            {
                Debug.LogError("Please set the main camera");
            }
            else if(!useDefaultPos)
            {               
                //set the camera's position and rotation angles
                MainCamera.transform.position = new Vector3(CameraInitPos.x, CameraHeight, CameraInitPos.y);
                MainCamera.transform.eulerAngles = new Vector3(45.0f, 0f, 0.0f);
            }
            //get the camera FOV
            FOV = MainCamera.fieldOfView;
        }

        void Update()
        {
            RotCame();
            return;
            //If panning is enabled and the player is holding the panning key
            if (Panning == true && Input.GetButton(PanningKey))
            {
                PanCam(); //平移是在平面内移动摄像机 ，相对地平面是平行移动  但是Pan 可以斜线移动
            }
            else
            { //if the player is not panning the camera
                MoveCam(); //这个也是相对地平面移动
            }

            //Camera position limits if the camera has moved
            transform.position = RefinePosition(transform.position);

            if (ZoomEnabled == true)
            {
                CamZoom();
            }

            //Follow a unit:
            return;

            //if we can actually follow units:
            if (CanFollowUnit == true)
            {
                
                //if the player moved the camera
                if (Moved == true)
                {
                    //we're not following a unit anymore
                    UnitToFollow = null;
                }

                FollowUnit();
            }

            //Minimap movement :

            //if the player presses one of the mouse buttons:
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                //if the player's mouse is over the minmap
                if (MinimapCam.rect.Contains(MainCamera.ScreenToViewportPoint(Input.mousePosition)))
                {
                    //see if we touched the minimap and update accordinly:
                    OnMinimapClick();
                }
            }

            if (Moved == true) //if the player moved the camera
            {
                UpdateMinimapCursor();
            }

            Moved = false;
        }

        //method for camera panning:
        void PanCam()
        {
            MouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Input.GetAxis("Mouse X") 取值为-1  1 表示鼠标前后两帧移动

            if (MouseAxis != Vector2.zero) //if the player is moving the mouse
            {
                //calculate the target mvt vector here:
                Vector3 TargetMvt = new Vector3(-MouseAxis.x, 0.0f, -MouseAxis.y);

                TargetMvt *= PanningSpeed * Time.deltaTime;

                // Put the movement vector into world space
                TargetMvt = transform.rotation * TargetMvt;
                // Zero out any vertical movement and normalize
                float OrigLen = TargetMvt.magnitude;
                TargetMvt.y = 0.0f;
                TargetMvt.Normalize();
                TargetMvt *= OrigLen;

                transform.Translate(TargetMvt, Space.World);
                Moved = true;
            }
        }

        //a method to move the camera:
        void MoveCam()
        {
            //check if the player can move the camera on screen edge:
            CanMoveOnEdge = false;
            if (MoveOnScreenEdge)//允许靠近屏幕边缘时移动
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    CanMoveOnEdge = true;
                }
            }

            Vector3 TargetMvt = Vector3.zero; //mvt direction

            //Screen edges rects:
            DownRect = new Rect(0.0f, 0.0f, Screen.width, ScreenEdgeSize);
            UpRect = new Rect(0.0f, Screen.height - ScreenEdgeSize, Screen.width, ScreenEdgeSize);
            LeftRect = new Rect(0.0f, 0.0f, ScreenEdgeSize, Screen.height);
            RightRect = new Rect(Screen.width - ScreenEdgeSize, 0.0f, ScreenEdgeSize, Screen.height);

            //move on edge: see if the mouse is on the screen edge while the mvt on edge is allowed
            bool MoveUp = (UpRect.Contains(Input.mousePosition) && CanMoveOnEdge == true);
            bool MoveDown = (DownRect.Contains(Input.mousePosition) && CanMoveOnEdge == true);
            bool MoveRight = (RightRect.Contains(Input.mousePosition) && CanMoveOnEdge == true);
            bool MoveLeft = (LeftRect.Contains(Input.mousePosition) && CanMoveOnEdge == true);

            //determine the movement direction depending on the above bools
            TargetMvt.x = MoveRight ? 1 : MoveLeft ? -1 : 0;
            TargetMvt.z = MoveUp ? 1 : MoveDown ? -1 : 0;

            //if there's a direction to move to
            if (TargetMvt != Vector3.zero)
            {
                //move with the defined speed
                TargetMvt *= MvtSpeed * Time.deltaTime;
                TargetMvt = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * TargetMvt;

                transform.Translate(TargetMvt, Space.World);

                Moved = true; //moving 这个比按键优先级搞
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            { //keyboard movement:
              //use the Horizontal and Vertical axis to move the camera with the above defined speed
                transform.Translate(Vector3.right.normalized * Input.GetAxis("Horizontal") * MvtSpeed * Time.deltaTime);
                transform.Translate((Vector3.up + Vector3.forward).normalized * Input.GetAxis("Vertical") * MvtSpeed * Time.deltaTime);

                Moved = true; //moving
            }
        }

        //a method to zoom the camera:
        void CamZoom()
        {
            //Zoom in/out:

            //If the player presses the zoom in and out keys:
            if (Input.GetKey(ZoomInKey) && CanZoomWithKey == true)
            {
                if (ZoomVelocity > 0)
                {
                    ZoomVelocity = 0.0f;
                }
                //Smoothly zoom in:
                FOV = Mathf.SmoothDamp(FOV, MinFOV, ref ZoomVelocity, ZoomSmoothTime);
            }
            else if (Input.GetKey(ZoomOutKey) && CanZoomWithKey == true)
            {
                if (ZoomVelocity < 0)
                {
                    ZoomVelocity = 0.0f;
                }
                //Smoothly zoom out:
                FOV = Mathf.SmoothDamp(FOV, MaxFOV, ref ZoomVelocity, ZoomSmoothTime);
            }
            else if (ZoomOnMouseWheel == true)
            {
                FOV -= Input.GetAxis("Mouse ScrollWheel") * ZoomScrollWheelSensitivty;
            }
            //Always keep the field of view between the max and the min values:
            FOV = Mathf.Clamp(FOV, MinFOV, MaxFOV);

            //update the UI cam's fog as well
            if (UICam)
            { //if there's an ignore fog camera then update the FOV there too
                UICam.fieldOfView = FOV;
            }
        }

        void RotCame() {
           // transform.RotateAround(target.position, Vector3.up, 10 * Time.deltaTime); //摄像机围绕目标旋转
            var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
            var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动             
            if (Input.GetKey(KeyCode.Mouse1)) //鼠标钟健 旋转摄像机
            {
                transform.RotateAround(target.transform.position, Vector3.up, mouse_x * 5);// 前后左右旋转

                //这2个最好分开不同按钮 而且这个最好不要，设置好了就不变
                Vector3 EularAngle = transform.localEulerAngles;
                EularAngle = EularAngle + Vector3.right * mouse_y * 5;
                if (EularAngle.x < minEularX)
                    EularAngle.x = minEularX;
                else if (EularAngle.x > maxEularX)
                    EularAngle.x = maxEularX;
                transform.localEulerAngles = EularAngle;
                

                //transform.RotateAround(target.transform.position, transform.right, mouse_y * 5);//上下抬头低头看
              

            }
           
        }

        void FollowUnit() //method to follow unit
        {
            if (SelectionMgr.SelectedUnits.Count == 1)
            {
                //can only work with one unit selected:
                if (SelectionMgr.SelectedUnits[0] != null)
                {
                    if (Input.GetKeyDown(FollowUnitKey))
                    { //if the player presses the follow key
                        UnitToFollow = SelectionMgr.SelectedUnits[0]; //make the selected unit, the unit to follow.
                    }
                }
            }
        }

        //method called to check if the player clicked on the minimap and update accordinly:
        void OnMinimapClick()
        {
            Ray RayCheck;
            RaycastHit[] Hits;

            //create a raycast using the minimap camera
            RayCheck = MinimapCam.ScreenPointToRay(Input.mousePosition);
            Hits = Physics.RaycastAll(RayCheck, Mathf.Infinity);

            if (Hits.Length > 0)
            {
                for (int i = 0; i < Hits.Length; i++)
                {
                    //If we clicked on a part of the terrain:
                    if (Hits[i].transform.gameObject == TerrainMgr.FlatTerrain)
                    {
                        //if this is the left mouse button and the selection box is disabled
                        if (Input.GetMouseButtonUp(0) && SelectionMgr.SelectionBoxEnabled == false)
                        {
                            //stop following the unit if it's enabled
                            if (CanFollowUnit == true)
                                UnitToFollow = null;
                            //make the camera look at the position we clicked in the minimap
                            LookAt(Hits[i].point);
                            //mark as moved:
                            Moved = true;
                        }
                        //TO BE CHANGED
                        //if the player presses the right mouse button
                        else if (Input.GetMouseButtonUp(1))
                        {
                            //move the selected units to the new clicked position in the minimap
                            MvtMgr.Move(SelectionMgr.SelectedUnits, Hits[i].point, 0.0f, null, InputTargetMode.None);
                        }
                    }
                }
            }
        }

        void FixedUpdate()
        {
            //update the field of view here:
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, FOV, Time.deltaTime * ZoomScrollWheelSpeed);

            //if we can actually follow units:
            if (CanFollowUnit == true)
            {
                if (UnitToFollow != null)
                { //if the camera is following a unit:
                    LookAt(UnitToFollow.transform.position);
                }
            }
        }

        //looks at the selected unit:
        public void LookAtSelectedUnit()
        {
            if (SelectionMgr.SelectedUnits.Count == 1)
            {
                LookAt(SelectionMgr.SelectedUnits[0].transform.position);
            }
        }

        //look at a position in the map
        public void LookAt(Vector3 LookAtPos)
        {
            //look at the new position 
            transform.position = RefinePosition(new Vector3(LookAtPos.x + OffsetX, transform.position.y, LookAtPos.z + OffsetZ));

        }

        public void UpdateMinimapCursor()
        {
            Ray RayCheck;
            RaycastHit[] Hits;

            //raycast using the main camera
            RayCheck = MainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
            Hits = Physics.RaycastAll(RayCheck, Mathf.Infinity);

            if (Hits.Length > 0)
            {
                for (int i = 0; i < Hits.Length; i++)
                {
                    //as soon as we hit the main terrain 
                    if (Hits[i].transform.gameObject == TerrainMgr.FlatTerrain)
                    {
                        //change the mini map cursor position to suit the new camera position
                        SetMiniMapCursorPos(Hits[i].point);
                    }
                }
            }
        }

        //set the minimap cursor position here
        public void SetMiniMapCursorPos(Vector3 NewPos)
        {
            Vector2 CanvasPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MinimapCanvas.GetComponent<RectTransform>(), MinimapCam.WorldToScreenPoint(RefinePosition(NewPos)), MinimapCam, out CanvasPos);
            MinimapCursor.GetComponent<RectTransform>().localPosition = new Vector3(CanvasPos.x, CanvasPos.y, MinimapCursor.GetComponent<RectTransform>().localPosition.z);
        }

        //refine the position to suit the camera's settings
        Vector3 RefinePosition(Vector3 Position)
        {
            //if we're using screen limit
            if (ScreenLimit == true)
            {
                //clamp the position
                Position = new Vector3(Mathf.Clamp(Position.x, MinPos.x, MaxPos.x), Position.y, Mathf.Clamp(Position.z, MinPos.y, MaxPos.y));
            }

            return Position;
        }
    }
}