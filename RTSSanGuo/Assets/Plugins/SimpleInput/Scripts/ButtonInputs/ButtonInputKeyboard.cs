using UnityEngine;

namespace SimpleInputNamespace
{
	public class ButtonInputKeyboard : MonoBehaviour
	{
		[SerializeField]
		public KeyCode key;

		public SimpleInput.ButtonInput button = new SimpleInput.ButtonInput();

		public void OnEnable()
		{
			button.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}

		public void OnDisable()
		{
			button.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;
		}

		public void OnUpdate()
		{
			button.value = Input.GetKey( key );
		}
	}
}