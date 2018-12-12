using UnityEngine;
using System.Collections;

namespace RTSSanGuo
{

    public class RTSCamCtr : MonoBehaviour
    {

        public Transform m_Transform; //camera tranform
       
        public float keyboardMovementSpeed = 5f; // 移动
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public float panningSpeed = 10f;//Panning 移动
        public KeyCode panningKey = KeyCode.Mouse2;//按下鼠标右键可以额平移
        public float screenEdgeMovementSpeed = 3f; //speed with screen edge movement
        public float screenEdgeBorder = 25f;

        public Transform targetFollow;
        public Vector3 targetOffset;
        public bool rapidFollow;// 大部分都是立即跟随
        public float followingSpeed = 25f; //speed when following a target
        public float limitX = 50f; //x limit of map
        public float limitY = 50f; //z limit of map


        public float rotationSped = 3f;
        public KeyCode rotateRightKey = KeyCode.X;
        public KeyCode rotateLeftKey = KeyCode.Z;
        public float mouseRotationSpeed = 10f;
        public KeyCode mouseRotationKey = KeyCode.Mouse1;
        public LayerMask planeGroundMask = -1; // ground 是高起伏的这个是  PlaneGround 是平的，有且只有一个

        public bool autoHeight = true;
        public LayerMask groundMask = -1; //layermask of ground or other objects that affect height
        public float maxHeight = 10f; //maximal height
        public float minHeight = 15f; //minimnal height
        public float heightDampening = 5f;
        public float keyboardZoomingSensitivity = 2f;
        public KeyCode zoomInKey = KeyCode.E;
        public KeyCode zoomOutKey = KeyCode.Q;
        public float scrollWheelZoomingSensitivity = 25f;
        public string zoomingAxis = "Mouse ScrollWheel";
        public float zoomPos = 0; //value in range (0, 1) used as t in Matf.Lerp







       






        public Vector2 KeyboardInput
        {
            get { return new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)); }
        }

        public Vector2 MouseInput
        {
            get { return Input.mousePosition; }
        }

        public float ScrollWheel
        {
            get { return Input.GetAxis(zoomingAxis); }
        }

        public Vector2 MouseAxis
        {
            get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
            // "Mouse X" 和"Mouse Y" 映射于鼠标增量 ,而不是鼠标坐标。这个本身就是增量
        }

        public int ZoomDirection
        {
            get
            {
                bool zoomIn = Input.GetKey(zoomInKey);
                bool zoomOut = Input.GetKey(zoomOutKey);
                if (zoomIn && zoomOut)
                    return 0;
                else if (!zoomIn && zoomOut)
                    return 1;
                else if (zoomIn && !zoomOut)
                    return -1;
                else
                    return 0;
            }
        }

        public int RotationDirection
        {
            get
            {
                bool rotateRight = Input.GetKey(rotateRightKey);
                bool rotateLeft = Input.GetKey(rotateLeftKey);
                if (rotateLeft && rotateRight)
                    return 0;
                else if (rotateLeft && !rotateRight)
                    return -1;
                else if (!rotateLeft && rotateRight)
                    return 1;
                else
                    return 0;
            }
        }



        private void Awake()
        {
            m_Transform = transform;
        }


        public void FixedUpdate()
        {
            //控制X  Z平面的移动
            if (FollowingTarget)
                FollowTarget();
            else
                Move();
            //控制Y平面的移动，也就是缩放
            HeightCalculation();
            //控制旋转
            Rotation();

            //最后渲染前限制下
            LimitPosition();
            LimitRotation();
        }

        

        /// <summary>
        /// move camera with keyboard or with screen edge
        /// </summary>
        public void Move()
        {

            {   //按键
                Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

                desiredMove *= keyboardMovementSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self); //前后左右是相对于摄像机当前视角
            }


            {  //屏幕边缘
                Vector3 desiredMove = new Vector3();

                Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
                Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
                Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
                Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

                desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
                desiredMove.z = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;

                desiredMove *= screenEdgeMovementSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
            }

            if (Input.GetKey(panningKey) && MouseAxis != Vector2.zero)
            {
                Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

                desiredMove *= panningSpeed;
                desiredMove *= Time.deltaTime;
                desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
                desiredMove = m_Transform.InverseTransformDirection(desiredMove);

                m_Transform.Translate(desiredMove, Space.Self);
            }
        }

        /// <summary>
        /// calcualte height
        /// </summary>
        public void HeightCalculation()
        {
            float distanceToGround = DistanceToGround();
            //滚轮  按键同时启动
            zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
            zoomPos += ZoomDirection * Time.deltaTime * keyboardZoomingSensitivity;

            zoomPos = Mathf.Clamp01(zoomPos);
            float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
            float difference = 0;
            if (distanceToGround != targetHeight)
                difference = targetHeight - distanceToGround;

            m_Transform.position = Vector3.Lerp(m_Transform.position,
                new Vector3(m_Transform.position.x, targetHeight + difference, m_Transform.position.z), Time.deltaTime * heightDampening);
            //这个是个Lerp过程，并非一帧就立即跳到target高度
        }

        /// <summary>
        /// rotate camera 这里只有左右旋转 ,左右旋转不能绕着自身旋转，而是绕着地图中心点旋转
        /// </summary>
        public void Rotation()
        {
            { //左右移动，绕Y轴移动
                transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * rotationSped, Space.World);
                m_Transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed, Space.World);
            }

        }

        /// <summary>
        /// follow targetif target != null
        /// </summary>
        public void FollowTarget()
        {
            Vector3 targetPos = new Vector3(targetFollow.position.x, m_Transform.position.y, targetFollow.position.z) + targetOffset;
            if(rapidFollow)
                m_Transform.position =   targetPos ;
            else 
                m_Transform.position = Vector3.MoveTowards(m_Transform.position, targetPos, Time.deltaTime * followingSpeed);
        }

        /// <summary>
        /// limit camera position
        /// </summary>
        public void LimitPosition()
        {
            m_Transform.position = new Vector3(Mathf.Clamp(m_Transform.position.x, -limitX, limitX),
            m_Transform.position.y,//y轴位置已经有最大高度控制
            Mathf.Clamp(m_Transform.position.z, -limitY, limitY));
        }

        public void LimitRotation()
        {

        }

        /// <summary>
        /// set the target
        /// </summary>
        /// <param name="target"></param>
        public void SetFollowTarget(Transform target,bool rapid = true )
        {
            targetFollow = target;
            rapidFollow = rapid;
        }

        /// <summary>
        /// reset the target (target is set to null)
        /// </summary>
        public void ClearFollowingTarget()
        {
            targetFollow = null;
        }

        public bool FollowingTarget
        {
            get
            {
                return targetFollow != null;
            }
        }


        /// <summary>
        /// calculate distance to ground
        /// </summary>
        /// <returns></returns>
        public float DistanceToGround()
        {
            Ray ray = new Ray(m_Transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundMask.value))
                return (hit.point - m_Transform.position).magnitude;

            return 0f;
        }


        public Vector3 LookGroundCenterPoint()
        {
            Ray ray = new Ray(m_Transform.position, m_Transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, planeGroundMask.value))
                return hit.point;
            return m_Transform.position + m_Transform.forward * 10f;
        }


    }
}