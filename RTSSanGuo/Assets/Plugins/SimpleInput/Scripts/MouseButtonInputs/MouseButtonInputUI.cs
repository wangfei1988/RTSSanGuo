using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleInputNamespace
{
	public class MouseButtonInputUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public SimpleInput.MouseButtonInput mouseButton = new SimpleInput.MouseButtonInput();

		public void Awake()
		{
			Graphic graphic = GetComponent<Graphic>();
			if( graphic != null )
				graphic.raycastTarget = true;
		}

		public void OnEnable()
		{
			mouseButton.StartTracking();
		}

		public void OnDisable()
		{
			mouseButton.StopTracking();
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			mouseButton.value = true;
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			mouseButton.value = false;
		}
	}
}