using UnityEngine;
using System.Collections;
using CtrlJam.Player;


namespace CtrlJam.Enemies
{

	public class Enemy : MonoBehaviour
	{
		public float life = 10;
		private bool isPlat;
		private bool isObstacle;
		private Transform fallCheck;
		private Transform wallCheck;
		public LayerMask turnLayerMask;
		private Rigidbody body;

		private bool facingRight = true;

		public float speed = 5f;

		public bool isInvincible = false;
		private bool isHitted = false;

		void Awake()
		{
			fallCheck = transform.Find("FallCheck");
			wallCheck = transform.Find("WallCheck");
			body = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void FixedUpdate()
		{

			if (life <= 0)
			{
				transform.GetComponent<Animator>().SetBool("IsDead", true);
				StartCoroutine(DestroyEnemy());
			}

			isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));
			isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);

			if (!isHitted && life > 0 && Mathf.Abs(body.velocity.y) < 0.5f)
			{
				if (isPlat && !isObstacle && !isHitted)
				{
					if (facingRight)
					{
						body.velocity = new Vector2(-speed, body.velocity.y);
					}
					else
					{
						body.velocity = new Vector2(speed, body.velocity.y);
					}
				}
				else
				{
					Flip();
				}
			}
		}

		void Flip()
		{
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		public void ApplyDamage(float damage)
		{
			if (!isInvincible)
			{
				float direction = damage / Mathf.Abs(damage);
				damage = Mathf.Abs(damage);
				transform.GetComponent<Animator>().SetBool("Hit", true);
				life -= damage;
				body.velocity = Vector2.zero;
				body.AddForce(new Vector2(direction * 500f, 100f));
				StartCoroutine(HitTime());
			}
		}

		void OnCollisionStay(Collision collision)
		{
			if (collision.gameObject.CompareTag("Player") && life > 0) collision.gameObject.GetComponent<PlayerController>().ApplyDamage(2f, transform.position);
		}

		IEnumerator HitTime()
		{
			isHitted = true;
			isInvincible = true;
			yield return new WaitForSeconds(0.1f);
			isHitted = false;
			isInvincible = false;
		}

		IEnumerator DestroyEnemy()
		{
			var capsule = GetComponent<CapsuleCollider>();
			// capsule.size = new Vector2(1f, 0.25f);
			// capsule.offset = new Vector2(0f, -0.8f);
			// capsule.direction = CapsuleDirection2D.Horizontal;
			yield return new WaitForSeconds(0.25f);
			body.velocity = new Vector2(0, body.velocity.y);
			yield return new WaitForSeconds(3f);
			Destroy(gameObject);
		}
	}
}
