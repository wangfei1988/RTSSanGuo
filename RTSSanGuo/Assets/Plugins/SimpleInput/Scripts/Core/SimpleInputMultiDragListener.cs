using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
	[DisallowMultipleComponent]
	[RequireComponent( typeof( RectTransform ) )]
	public class SimpleInputMultiDragListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public const float POINTERS_VALIDATION_INTERVAL = 5f;

		public List<ISimpleInputDraggableMultiTouch> listeners = new List<ISimpleInputDraggableMultiTouch>( 1 );
		public ISimpleInputDraggableMultiTouch activeListener;

		public List<PointerEventData> mousePointers = new List<PointerEventData>();
		public List<PointerEventData> touchPointers = new List<PointerEventData>();
		public List<PointerEventData> validPointers = new List<PointerEventData>();

		public float pointersNextValidation = POINTERS_VALIDATION_INTERVAL;

		public void Awake()
		{
			Graphic graphic = GetComponent<Graphic>();
			if( graphic == null )
			{
				graphic = gameObject.AddComponent<Image>();
				graphic.color = Color.clear;
			}

			graphic.raycastTarget = true;
		}

		public void OnEnable()
		{
			SimpleInput.OnUpdate += OnUpdate;
		}

		public void OnDisable()
		{
			SimpleInput.OnUpdate -= OnUpdate;
		}

		public void AddListener( ISimpleInputDraggableMultiTouch listener )
		{
			int priority = listener.Priority;
			int i = 0;
			while( i < listeners.Count && listeners[i].Priority < priority )
				i++;

			listeners.Insert( i, listener );
		}

		public void RemoveListener( ISimpleInputDraggableMultiTouch listener )
		{
			listeners.Remove( listener );

			if( activeListener == listener )
				activeListener = null;
		}

		public void OnUpdate()
		{
			pointersNextValidation -= Time.unscaledDeltaTime;
			if( pointersNextValidation <= 0f )
			{
				pointersNextValidation = POINTERS_VALIDATION_INTERVAL;
				ValidatePointers();
			}

			for( int i = listeners.Count - 1; i >= 0; i-- )
			{
				if( listeners[i].OnUpdate( mousePointers, touchPointers, activeListener ) )
				{
					if( activeListener == null || activeListener.Priority < listeners[i].Priority )
						activeListener = listeners[i];
				}
				else if( activeListener == listeners[i] )
					activeListener = null;
			}

			for( int i = 0; i < mousePointers.Count; i++ )
				mousePointers[i].delta = new Vector2( 0f, 0f );

			for( int i = 0; i < touchPointers.Count; i++ )
				touchPointers[i].delta = new Vector2( 0f, 0f );
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			List<PointerEventData> pointers = eventData.IsTouchInput() ? touchPointers : mousePointers;

			for( int i = 0; i < pointers.Count; i++ )
			{
				if( pointers[i].pointerId == eventData.pointerId )
				{
					pointers[i] = eventData;
					return;
				}
			}

			pointers.Add( eventData );
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			for( int i = 0; i < mousePointers.Count; i++ )
			{
				if( mousePointers[i].pointerId == eventData.pointerId )
				{
					mousePointers.RemoveAt( i );
					break;
				}
			}

			for( int i = 0; i < touchPointers.Count; i++ )
			{
				if( touchPointers[i].pointerId == eventData.pointerId )
				{
					touchPointers.RemoveAt( i );
					break;
				}
			}
		}

		public void ValidatePointers()
		{
			for( int i = mousePointers.Count - 1; i >= 0; i-- )
			{
				if( !Input.GetMouseButton( (int) mousePointers[i].button ) )
					mousePointers.RemoveAt( i );
			}

			for( int i = Input.touchCount - 1; i >= 0; i-- )
			{
				int fingerId = Input.GetTouch( i ).fingerId;
				for( int j = 0; j < touchPointers.Count; j++ )
				{
					if( touchPointers[j].pointerId == fingerId )
					{
						validPointers.Add( touchPointers[j] );
						break;
					}
				}
			}

			List<PointerEventData> temp = touchPointers;
			touchPointers = validPointers;
			validPointers = temp;
			validPointers.Clear();
		}
	}
}