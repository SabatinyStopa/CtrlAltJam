using UnityEngine;
namespace CtrlJam.Player
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] private float FollowSpeed = 2f;
		[SerializeField] private Transform Target;
		[SerializeField] private float shakeDuration = 0f;
		[SerializeField] private float shakeAmount = 0.1f;
		[SerializeField] private float decreaseFactor = 1.0f;

		private Transform camTransform;
		private Vector3 originalPos;

		private void Awake()
		{
			Cursor.visible = false;

			if (camTransform == null) camTransform = GetComponent(typeof(Transform)) as Transform;
		}

		private void OnEnable() => originalPos = camTransform.localPosition;

		private void Update()
		{
			if (!Target) return;
			var newPosition = Target.position;

			newPosition.z = -10;
			transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
		}

		public void ShakeCamera()
		{
			originalPos = camTransform.localPosition;
			shakeDuration = 0.2f;
		}
	}
}