using UnityEngine;

namespace CtrlJam.Common
{
    public class Flower : MonoBehaviour
    {
        [SerializeField] private GameObject flyingBoxCollider;
        private bool playerInTheRange = false;

        private void Update()
        {
            if (playerInTheRange && Input.GetKeyDown(KeyCode.E)) Active();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) playerInTheRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) playerInTheRange = false;
        }

        private void Active() => flyingBoxCollider.SetActive(true);
    }
}