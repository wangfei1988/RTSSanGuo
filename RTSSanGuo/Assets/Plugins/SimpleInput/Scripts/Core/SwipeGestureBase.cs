using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	[RequireComponent( typeof( SimpleInputMultiDragListener ) )]
	public abstract class SwipeGestureBase<K, V> : SelectivePointerInput, ISimpleInputDraggableMultiTouch
	{
		public Vector2 swipeAmount = new Vector2( 0f, 25f );
		
		public SimpleInputMultiDragListener eventReceiver;

		protected abstract BaseInput<K, V> Input { get; }
		protected abstract V Value { get; }

		public abstract int Priority { get; }

		public void Awake()
		{
			eventReceiver = GetComponent<SimpleInputMultiDragListener>();
		}

		public void OnEnable()
		{
			eventReceiver.AddListener( this );
			Input.StartTracking();
		}

		public void OnDisable()
		{
			eventReceiver.RemoveListener( this );
			Input.StopTracking();
		}

		public bool OnUpdate( List<PointerEventData> mousePointers, List<PointerEventData> touchPointers, ISimpleInputDraggableMultiTouch activeListener )
		{
			Input.ResetValue();

			if( activeListener != null && activeListener.Priority > Priority )
				return false;

			PointerEventData pointer = GetSatisfyingPointer( mousePointers, touchPointers );
			if( pointer == null )
				return false;

#pragma warning disable 0252
			if( !IsSwipeSatisfied( pointer ) )
				return activeListener == this;
#pragma warning restore 0252

			Input.value = Value;
			return true;
		}

		public bool IsSwipeSatisfied( PointerEventData eventData )
		{
			Vector2 deltaPosition = eventData.position - eventData.pressPosition;
			if( swipeAmount.x > 0f )
			{
				if( deltaPosition.x < swipeAmount.x )
					return false;
			}
			else if( swipeAmount.x < 0f )
			{
				if( deltaPosition.x > swipeAmount.x )
					return false;
			}

			if( swipeAmount.y > 0f )
			{
				if( deltaPosition.y < swipeAmount.y )
					return false;
			}
			else if( swipeAmount.y < 0f )
			{
				if( deltaPosition.y > swipeAmount.y )
					return false;
			}

			return true;
		}
	}
}