using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	[RequireComponent( typeof( SimpleInputMultiDragListener ) )]
	public class Touchpad : SelectivePointerInput, ISimpleInputDraggableMultiTouch
	{
		public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput( "Horizontal" );
		public SimpleInput.AxisInput yAxis = new SimpleInput.AxisInput( "Vertical" );

		public bool invertHorizontal, invertVertical;
		public float sensitivity = 1f;

		public SimpleInputMultiDragListener eventReceiver;

		public int Priority { get { return 1; } }

		public Vector2 m_value = Vector2.zero;
		public Vector2 Value { get { return m_value; } }

		public void Awake()
		{
			eventReceiver = GetComponent<SimpleInputMultiDragListener>();
		}

		public void OnEnable()
		{
			eventReceiver.AddListener( this );

			xAxis.StartTracking();
			yAxis.StartTracking();
		}

		public void OnDisable()
		{
			eventReceiver.RemoveListener( this );

			xAxis.StopTracking();
			yAxis.StopTracking();
		}

		public bool OnUpdate( List<PointerEventData> mousePointers, List<PointerEventData> touchPointers, ISimpleInputDraggableMultiTouch activeListener )
		{
			xAxis.value = 0f;
			yAxis.value = 0f;

			if( activeListener != null && activeListener.Priority > Priority )
				return false;

			PointerEventData pointer = GetSatisfyingPointer( mousePointers, touchPointers );
			if( pointer == null )
				return false;

			m_value = pointer.delta * SimpleInputUtils.ResolutionMultiplier * sensitivity;

			xAxis.value = invertHorizontal ? -m_value.x : m_value.x;
			yAxis.value = invertVertical ? -m_value.y : m_value.y;

			return true;
		}
	}
}