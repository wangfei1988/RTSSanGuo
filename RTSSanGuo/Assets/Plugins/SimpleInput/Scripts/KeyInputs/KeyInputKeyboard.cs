using UnityEngine;

namespace SimpleInputNamespace
{
	public class KeyInputKeyboard : MonoBehaviour
	{
		[SerializeField]
		public KeyCode realKey;

		public SimpleInput.KeyInput key = new SimpleInput.KeyInput();

		public void OnEnable()
		{
			key.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}

		public void OnDisable()
		{
			key.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;
		}

		public void OnUpdate()
		{
			key.value = Input.GetKey( realKey );
		}
	}
}