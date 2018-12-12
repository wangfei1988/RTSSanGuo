using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	public class SteeringWheel : MonoBehaviour, ISimpleInputDraggable
	{
		public SimpleInput.AxisInput axis = new SimpleInput.AxisInput( "Horizontal" );

		public Graphic wheel;

		public RectTransform wheelTR;
		public Vector2 centerPoint;

		public float maximumSteeringAngle = 200f;
		public float wheelReleasedSpeed = 350f;
		public float valueMultiplier = 1f;

		public float wheelAngle = 0f;
		public float wheelPrevAngle = 0f;

		public bool wheelBeingHeld = false;

		public float m_value;
		public float Value { get { return m_value; } }

		public float Angle { get { return wheelAngle; } }

		public void Awake()
		{
			wheel = GetComponent<Graphic>();
			wheelTR = wheel.rectTransform;

			SimpleInputDragListener eventReceiver = gameObject.AddComponent<SimpleInputDragListener>();
			eventReceiver.Listener = this;
		}

		public void OnEnable()
		{
			axis.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}

		public void OnDisable()
		{
			axis.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;
		}

		public void OnUpdate()
		{
			// If the wheel is released, reset the rotation
			// to initial (zero) rotation by wheelReleasedSpeed degrees per second
			if( !wheelBeingHeld && wheelAngle != 0f )
			{
				float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
				if( Mathf.Abs( deltaAngle ) > Mathf.Abs( wheelAngle ) )
					wheelAngle = 0f;
				else if( wheelAngle > 0f )
					wheelAngle -= deltaAngle;
				else
					wheelAngle += deltaAngle;
			}

			// Rotate the wheel image
			wheelTR.localEulerAngles = new Vector3( 0f, 0f, -wheelAngle );

			m_value = wheelAngle * valueMultiplier / maximumSteeringAngle;
			axis.value = m_value;
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			// Executed when mouse/finger starts touching the steering wheel
			wheelBeingHeld = true;
			centerPoint = RectTransformUtility.WorldToScreenPoint( eventData.pressEventCamera, wheelTR.position );
			wheelPrevAngle = Vector2.Angle( Vector2.up, eventData.position - centerPoint );
		}

		public void OnDrag( PointerEventData eventData )
		{
			// Executed when mouse/finger is dragged over the steering wheel
			Vector2 pointerPos = eventData.position;

			float wheelNewAngle = Vector2.Angle( Vector2.up, pointerPos - centerPoint );

			// Do nothing if the pointer is too close to the center of the wheel
			if( ( pointerPos - centerPoint ).sqrMagnitude >= 400f )
			{
				if( pointerPos.x > centerPoint.x )
					wheelAngle += wheelNewAngle - wheelPrevAngle;
				else
					wheelAngle -= wheelNewAngle - wheelPrevAngle;
			}

			// Make sure wheel angle never exceeds maximumSteeringAngle
			wheelAngle = Mathf.Clamp( wheelAngle, -maximumSteeringAngle, maximumSteeringAngle );
			wheelPrevAngle = wheelNewAngle;
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			// Executed when mouse/finger stops touching the steering wheel
			// Performs one last OnDrag calculation, just in case
			OnDrag( eventData );

			wheelBeingHeld = false;
		}
	}
}