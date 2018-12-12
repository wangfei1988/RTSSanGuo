using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
	public class KeyInputUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public SimpleInput.KeyInput key = new SimpleInput.KeyInput();

		public void Awake()
		{
			Graphic graphic = GetComponent<Graphic>();
			if( graphic != null )
				graphic.raycastTarget = true;
		}

		public void OnEnable()
		{
			key.StartTracking();
		}

		public void OnDisable()
		{
			key.StopTracking();
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			key.value = true;
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			key.value = false;
		}
	}
}