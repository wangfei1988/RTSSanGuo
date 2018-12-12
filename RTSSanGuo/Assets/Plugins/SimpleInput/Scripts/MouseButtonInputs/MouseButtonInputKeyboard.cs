using UnityEngine;

namespace SimpleInputNamespace
{
	public class MouseButtonInputKeyboard : MonoBehaviour
	{
		[SerializeField]
		public KeyCode key;

		public SimpleInput.MouseButtonInput mouseButton = new SimpleInput.MouseButtonInput();

		public void OnEnable()
		{
			mouseButton.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}

		public void OnDisable()
		{
			mouseButton.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;
		}

		public void OnUpdate()
		{
			mouseButton.value = Input.GetKey( key );
		}
	}
}