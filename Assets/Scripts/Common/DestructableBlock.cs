using System.Collections;
using UnityEngine;

namespace CtrlJam.Common
{
    public class DestructableBlock : MonoBehaviour
    {
        [SerializeField] private float timeToDisable;
        [SerializeField] private float timeToEnable;
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private MeshRenderer meshRenderer;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")) StartCoroutine(DisableBlock());
        }

        private IEnumerator DisableBlock()
        {
            yield return new WaitForSeconds(timeToDisable);
            boxCollider.enabled = false;
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(timeToEnable);
            boxCollider.enabled = true;
            meshRenderer.enabled = true;
        }
    }
}