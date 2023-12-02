using CtrlJam.Enums;
using UnityEngine;

namespace CtrlJam.Managers
{
    public class LayerObjectsManager : MonoBehaviour
    {
        public static LayerObjectsManager Instance;
        
        [System.Serializable]
        struct ObjectsByLayer
        {
            public ObjectLayer Layer;
            public GameObject[] Objects;
        }

        [SerializeField] private ObjectsByLayer[] layerObjects;

        private void Awake() => Instance = this;

        public void SetLayer(ObjectLayer oldLayer, ObjectLayer currentLayer)
        {
            foreach (ObjectsByLayer objectByLayer in layerObjects)
            {
                if (oldLayer == objectByLayer.Layer) foreach (GameObject objectToDisative in objectByLayer.Objects) objectToDisative.SetActive(false);

                if (currentLayer == objectByLayer.Layer) foreach (GameObject objectToDisative in objectByLayer.Objects) objectToDisative.SetActive(true);
            }
        }
    }
}