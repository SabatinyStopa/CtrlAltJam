using UnityEngine;

namespace CtrlJam.Common
{
    public class DontDestroyMe : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}