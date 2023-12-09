using UnityEngine;
using UnityEngine.SceneManagement;

namespace CtrlJam.Common
{
    public class KillerBlock : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            else if(other.gameObject.layer == LayerMask.NameToLayer("Ground")) Destroy(gameObject);
        }
    }
}

