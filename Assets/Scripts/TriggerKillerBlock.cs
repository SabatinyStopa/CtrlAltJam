using UnityEngine;

namespace CtrlJam.Common
{
    public class TriggerKillerBlock : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) body.useGravity = true;
        }
    }
}