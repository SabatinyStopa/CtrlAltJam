using UnityEngine;

namespace CtrlJam.Player
{
	public class ThrowableProjectile : MonoBehaviour
	{
		public Vector2 direction;
		public bool hasHit = false;
		public float speed = 15f;
		public GameObject owner;

		private void FixedUpdate()
		{
			if (!hasHit) GetComponent<Rigidbody>().velocity = direction * speed;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				other.gameObject.GetComponent<PlayerController>().ApplyDamage(2f, transform.position);
				Destroy(gameObject);
			}
			else if (owner != null && other.gameObject != owner && other.gameObject.tag == "Enemy")
			{
				other.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
				Destroy(gameObject);
			}
			else if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player")
			{
				Destroy(gameObject);
			}

		}
	}
}