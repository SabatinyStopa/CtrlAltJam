using UnityEngine;

namespace CtrlJam.Common
{
    public class TriggerGravity : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && body != null) body.useGravity = true;
        }
    }
}