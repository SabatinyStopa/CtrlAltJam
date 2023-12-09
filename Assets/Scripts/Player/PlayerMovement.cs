using Unity.Mathematics;
using UnityEngine;

namespace CtrlJam.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private Animator animator;
		[SerializeField] private Rigidbody body;
		[SerializeField] private float jumpForce;
		[SerializeField] private float speed = 40f;
		[SerializeField] private float speedGravity = 50f;
		[SerializeField] private Transform isGroundedChecker;

		private bool isGrounded;
		private float horizontalMoviment;
		private bool beingAppliedExternalForces = false;

		public bool BeingAppliedExternalForces { get => beingAppliedExternalForces; set => beingAppliedExternalForces = value; }

		private void Update()
		{
			horizontalMoviment = Input.GetAxis("Horizontal");
			isGrounded = Physics.OverlapSphere(isGroundedChecker.position, 0.1f, LayerMask.NameToLayer("Ground")).Length > 0;

			animator.SetFloat("Speed", Mathf.Clamp01(horizontalMoviment));
			animator.SetBool("IsGrounded", isGrounded);

			if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();

			if (horizontalMoviment != 0)
			{
				if (horizontalMoviment > 0) transform.rotation = new Quaternion(0, 0, 0, 0);
				else transform.rotation = new Quaternion(0, 180, 0, 0);

			}
		}

		private void FixedUpdate()
		{
			if (!BeingAppliedExternalForces) body.AddForce(Vector3.down * speedGravity, ForceMode.Acceleration);

			body.velocity = new Vector3(horizontalMoviment * Time.fixedDeltaTime * speed, body.velocity.y, 0);
		}

		private void Jump() => body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	}
}