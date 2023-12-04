using UnityEngine;
using UnityEngine.SceneManagement;

namespace CtrlJam.Common
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneIndex = 0;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) SceneManager.LoadScene(sceneIndex);
        }
    }
}