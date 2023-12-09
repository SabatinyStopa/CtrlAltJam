using UnityEngine;
using UnityEngine.SceneManagement;

namespace CtrlJam.Common
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

