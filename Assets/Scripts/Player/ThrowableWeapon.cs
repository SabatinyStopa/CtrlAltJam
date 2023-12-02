using UnityEngine;

namespace CtrlJam.Player
{
	public class ThrowableWeapon : MonoBehaviour
	{
		[SerializeField] private Vector2 direction;
		[SerializeField] private bool hasHit = false;
		[SerializeField] private float speed = 10f;

        public Vector2 Direction { get => direction; set => direction = value; }

        private void FixedUpdate()
		{
			if (!hasHit) GetComponent<Rigidbody2D>().velocity = direction * speed;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
				Destroy(gameObject);
			}
			else if (collision.gameObject.tag != "Player")
			{
				Destroy(gameObject);
			}
		}
	}
}