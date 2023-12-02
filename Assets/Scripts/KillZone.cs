using UnityEngine;
using UnityEngine.SceneManagement;

namespace CtrlJam.Player
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            else Destroy(other.gameObject);
        }
    }
}

