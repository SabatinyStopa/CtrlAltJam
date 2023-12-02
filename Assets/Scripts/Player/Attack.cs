using System.Collections;
using UnityEngine;

namespace CtrlJam.Player
{
	public class Attack : MonoBehaviour
	{
		[SerializeField] private float dmgValue = 4;
		[SerializeField] private GameObject throwableObject;
		[SerializeField] private Transform attackCheck;
		private Rigidbody body;
		[SerializeField] private Animator animator;
		[SerializeField] private bool canAttack = true;

		[SerializeField] private GameObject cam;

		private void Awake() => body = GetComponent<Rigidbody>();

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.X) && canAttack)
			{
				canAttack = false;
				animator.SetBool("IsAttacking", true);
				StartCoroutine(AttackCooldown());
			}

			if (Input.GetKeyDown(KeyCode.V))
			{
				GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
				Vector2 direction = new Vector2(transform.localScale.x, 0);
				throwableWeapon.GetComponent<ThrowableWeapon>().Direction = direction;
				throwableWeapon.name = "ThrowableWeapon";
			}
		}

		IEnumerator AttackCooldown()
		{
			yield return new WaitForSeconds(0.25f);
			canAttack = true;
		}

		public void DoDashDamage()
		{
			dmgValue = Mathf.Abs(dmgValue);
			Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
			for (int i = 0; i < collidersEnemies.Length; i++)
			{
				if (collidersEnemies[i].gameObject.tag == "Enemy")
				{
					if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
					{
						dmgValue = -dmgValue;
					}
					collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
					cam.GetComponent<CameraFollow>().ShakeCamera();
				}
			}
		}
	}
}
