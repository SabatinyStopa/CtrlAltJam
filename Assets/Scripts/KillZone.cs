using UnityEngine;
using UnityEngine.SceneManagement;

namespace CtrlJam.Player
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            else if(other.gameObject.layer == LayerMask.NameToLayer("Ground")) Destroy(gameObject);
        }
    }
}

