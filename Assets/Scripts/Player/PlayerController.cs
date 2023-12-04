using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEngine;

namespace CtrlJam.Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float jumpForce = 400f;
		[Range(0, .3f)][SerializeField] private float moveimentSmoothing = .05f;
		[SerializeField] private bool airControl = false;
		[SerializeField] private LayerMask whatIsGround;
		[SerializeField] private Transform groundCheck;
		[SerializeField] private Transform wallCheck;
		[SerializeField] private Animator animator;

		const float GROUNDED_RADIUS = .2f;
		[SerializeField]private bool isgrounded;
		private Rigidbody body;
		private bool facingRight = true;
		private Vector3 velocity = Vector3.zero;
		private float limitFallSpeed = 25f;

		public bool canDoubleJump = true;
		[SerializeField] private float dashForce = 25f;
		private bool canDash = true;
		private bool isDashing = false;
		private bool isWall = false;
		private bool isWallSliding = false;
		private bool oldWallSlidding = false;
		private bool canCheck = false;

		public float life = 10f;
		public bool invincible = false;
		private bool canMove = true;

		public ParticleSystem particleJumpUp;
		public ParticleSystem particleJumpDown;

		private float jumpWallStartX = 0;
		private float jumpWallDistX = 0;
		private bool limitVelOnWallJump = false;

		[Header("Events")]
		[Space]

		public UnityEvent OnFallEvent;
		public UnityEvent OnLandEvent;

		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		private void Awake()
		{
			body = GetComponent<Rigidbody>();

			if (OnFallEvent == null)
				OnFallEvent = new UnityEvent();

			if (OnLandEvent == null)
				OnLandEvent = new UnityEvent();
		}


		private void FixedUpdate()
		{
			var wasGrounded = isgrounded;
			isgrounded = false;

			var colliders = Physics.OverlapSphere(groundCheck.position, GROUNDED_RADIUS, whatIsGround);

			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)

					isgrounded = true;

				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
					if (!isWall && !isDashing)
						particleJumpDown.Play();
					canDoubleJump = true;
					if (body.velocity.y < 0f)
						limitVelOnWallJump = false;
				}
			}

			isWall = false;

			if (!isgrounded)
			{
				OnFallEvent.Invoke();
				var collidersWall = Physics.OverlapSphere(wallCheck.position, GROUNDED_RADIUS, whatIsGround);
				for (int i = 0; i < collidersWall.Length; i++)
				{
					if (collidersWall[i].gameObject != null)
					{
						isDashing = false;
						isWall = true;
					}
				}
			}

			if (limitVelOnWallJump)
			{
				if (body.velocity.y < -0.5f)
					limitVelOnWallJump = false;
				jumpWallDistX = (jumpWallStartX - transform.position.x) * transform.localScale.x;
				if (jumpWallDistX < -0.5f && jumpWallDistX > -1f)
				{
					canMove = true;
				}
				else if (jumpWallDistX < -1f && jumpWallDistX >= -2f)
				{
					canMove = true;
					body.velocity = new Vector2(10f * transform.localScale.x, body.velocity.y);
				}
				else if (jumpWallDistX < -2f)
				{
					limitVelOnWallJump = false;
					body.velocity = new Vector2(0, body.velocity.y);
				}
				else if (jumpWallDistX > 0)
				{
					limitVelOnWallJump = false;
					body.velocity = new Vector2(0, body.velocity.y);
				}
			}
		}


		public void Move(float move, bool jump, bool dash)
		{
			if (canMove)
			{
				if (dash && canDash && !isWallSliding) StartCoroutine(DashCooldown());
				if (isDashing) body.velocity = new Vector2(transform.localScale.x * dashForce, 0);
				else if (isgrounded || airControl)
				{
					if (body.velocity.y < -limitFallSpeed) body.velocity = new Vector2(body.velocity.x, -limitFallSpeed);

					Vector3 targetVelocity = new Vector2(move * 10f, body.velocity.y);
					body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref velocity, moveimentSmoothing);

					if (move > 0 && !facingRight && !isWallSliding) Flip();
					else if (move < 0 && facingRight && !isWallSliding) Flip();
				}
				if (isgrounded && jump)
				{
					animator.SetBool("IsJumping", true);
					animator.SetBool("JumpUp", true);
					isgrounded = false;
					body.AddForce(new Vector2(0f, jumpForce));
					canDoubleJump = true;
					particleJumpDown.Play();
					particleJumpUp.Play();
				}
				else if (!isgrounded && jump && canDoubleJump && !isWallSliding)
				{
					canDoubleJump = false;
					body.velocity = new Vector2(body.velocity.x, 0);
					body.AddForce(new Vector2(0f, jumpForce / 1.2f));
					animator.SetBool("IsDoubleJumping", true);
				}

				else if (isWall && !isgrounded)
				{
					if (!oldWallSlidding && body.velocity.y < 0 || isDashing)
					{
						isWallSliding = true;
						wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
						Flip();
						StartCoroutine(WaitToCheck(0.1f));
						canDoubleJump = true;
						animator.SetBool("IsWallSliding", true);
					}
					isDashing = false;

					if (isWallSliding)
					{
						if (move * transform.localScale.x > 0.1f)
						{
							StartCoroutine(WaitToEndSliding());
						}
						else
						{
							oldWallSlidding = true;
							body.velocity = new Vector2(-transform.localScale.x * 2, -5);
						}
					}

					if (jump && isWallSliding)
					{
						animator.SetBool("IsJumping", true);
						animator.SetBool("JumpUp", true);
						body.velocity = new Vector2(0f, 0f);
						body.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
						jumpWallStartX = transform.position.x;
						limitVelOnWallJump = true;
						canDoubleJump = true;
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
						canMove = false;
					}
					else if (dash && canDash)
					{
						isWallSliding = false;
						animator.SetBool("IsWallSliding", false);
						oldWallSlidding = false;
						wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
						canDoubleJump = true;
						StartCoroutine(DashCooldown());
					}
				}
				else if (isWallSliding && !isWall && canCheck)
				{
					isWallSliding = false;
					animator.SetBool("IsWallSliding", false);
					oldWallSlidding = false;
					wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
					canDoubleJump = true;
				}
			}
		}


		private void Flip()
		{
			facingRight = !facingRight;
			transform.rotation = facingRight ? Quaternion.identity : Quaternion.Euler(new Vector3(0, 180, 0));
		}

		public void ApplyDamage(float damage, Vector3 position)
		{
			if (!invincible)
			{
				animator.SetBool("Hit", true);
				life -= damage;
				Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
				body.velocity = Vector2.zero;
				body.AddForce(damageDir * 10);

				if (life <= 0) StartCoroutine(WaitToDead());
				else
				{
					StartCoroutine(Stun(0.25f));
					StartCoroutine(MakeInvincible(1f));
				}
			}
		}

		private IEnumerator DashCooldown()
		{
			animator.SetBool("IsDashing", true);
			isDashing = true;
			canDash = false;
			yield return new WaitForSeconds(0.1f);
			isDashing = false;
			yield return new WaitForSeconds(0.5f);
			canDash = true;
		}

		private IEnumerator Stun(float time)
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}
		private IEnumerator MakeInvincible(float time)
		{
			invincible = true;
			yield return new WaitForSeconds(time);
			invincible = false;
		}
		private IEnumerator WaitToMove(float time)
		{
			canMove = false;
			yield return new WaitForSeconds(time);
			canMove = true;
		}

		private IEnumerator WaitToCheck(float time)
		{
			canCheck = false;
			yield return new WaitForSeconds(time);
			canCheck = true;
		}

		private IEnumerator WaitToEndSliding()
		{
			yield return new WaitForSeconds(0.1f);
			canDoubleJump = true;
			isWallSliding = false;
			animator.SetBool("IsWallSliding", false);
			oldWallSlidding = false;
			wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		}

		private IEnumerator WaitToDead()
		{
			animator.SetBool("IsDead", true);
			canMove = false;
			invincible = true;
			GetComponent<Attack>().enabled = false;
			yield return new WaitForSeconds(0.4f);
			body.velocity = new Vector2(0, body.velocity.y);
			yield return new WaitForSeconds(1.1f);
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
