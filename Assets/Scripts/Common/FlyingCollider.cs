using System.Collections;
using CtrlJam.Player;
using UnityEngine;

namespace CtrlJam.Common
{
    public class FlyingCollider : MonoBehaviour
    {
        [SerializeField] private Transform flyingPoint;
        [SerializeField] private float upVelocity = 10f;
        private Rigidbody player;
        private Coroutine movingPlayer;

        private void Start() => player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        private void Update()
        {
            if (movingPlayer == null) return;

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.useGravity = true;
                player.AddForce(new Vector2(0f, 500f));

                if (movingPlayer != null) StopCoroutine(movingPlayer);
                player.GetComponent<PlayerMovement>().BeingAppliedExternalForces = false;
                movingPlayer = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (movingPlayer != null) StopCoroutine(movingPlayer);
                movingPlayer = StartCoroutine(MovePlayer());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player.useGravity = true;
                player.GetComponent<PlayerMovement>().BeingAppliedExternalForces = false;
                if (movingPlayer != null) StopCoroutine(movingPlayer);
                movingPlayer = null;
            }
        }

        private IEnumerator MovePlayer()
        {
            player.useGravity = false;
            player.GetComponent<PlayerMovement>().BeingAppliedExternalForces = true;

            while (Mathf.Abs(player.transform.position.y - flyingPoint.position.y) > 1f)
            {
                player.transform.position =
                Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, flyingPoint.position.y, player.transform.position.z), Time.deltaTime * upVelocity);
                yield return null;
            }
        }
    }
}