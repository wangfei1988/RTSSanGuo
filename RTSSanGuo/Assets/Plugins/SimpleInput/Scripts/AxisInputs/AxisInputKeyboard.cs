using UnityEngine;

namespace SimpleInputNamespace
{
	public class AxisInputKeyboard : MonoBehaviour
	{
		[SerializeField]
		public KeyCode key;

		public SimpleInput.AxisInput axis = new SimpleInput.AxisInput();
		public float value = 1f;

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
			axis.value = Input.GetKey( key ) ? value : 0f;
		}
	}
}