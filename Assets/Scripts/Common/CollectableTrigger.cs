using UnityEngine;
using UnityEngine.Events;
namespace CtrlJam.Common
{
    public class CollectableTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent[] eventsToTrigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) foreach (UnityEvent eventToTrigger in eventsToTrigger) eventToTrigger?.Invoke();

        }
    }
}