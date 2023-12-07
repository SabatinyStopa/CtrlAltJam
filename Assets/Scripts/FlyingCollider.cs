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
        private PlayerMovement playerMovement;
        private Coroutine movingPlayer;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (movingPlayer == null) return;

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.useGravity = true;
                player.AddForce(new Vector2(0f, 500f));
                
                if (movingPlayer != null) StopCoroutine(movingPlayer);
                movingPlayer = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Trrigger");
                if (movingPlayer != null) StopCoroutine(movingPlayer);
                movingPlayer = StartCoroutine(MovePlayer());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player.useGravity = true;
                if (movingPlayer != null) StopCoroutine(movingPlayer);
                movingPlayer = null;
            }
        }

        private IEnumerator MovePlayer()
        {
            player.useGravity = false;
            playerMovement.OnFall();

            while (Mathf.Abs(player.transform.position.y - flyingPoint.position.y) > 1f)
            {
                player.transform.position =
                Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, flyingPoint.position.y, player.transform.position.z), Time.deltaTime * upVelocity);
                yield return null;
            }
        }
    }
}