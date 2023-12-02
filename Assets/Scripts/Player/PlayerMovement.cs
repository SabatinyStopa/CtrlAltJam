using UnityEngine;

namespace CtrlJam.Player
{
	public class PlayerMovement : MonoBehaviour
	{

		public PlayerController controller;
		public Animator animator;

		public float runSpeed = 40f;

		float horizontalMove = 0f;
		bool jump = false;
		bool dash = false;

		
		private void Update()
		{
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

			if (Input.GetKeyDown(KeyCode.Space)) jump = true;

			if (Input.GetKeyDown(KeyCode.LeftShift)) dash = true;
		}

        public void OnFall() => animator.SetBool("IsJumping", true);

        public void OnLanding() => animator.SetBool("IsJumping", false);

        void FixedUpdate()
		{
			controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
			jump = false;
			dash = false;
		}
	}
}