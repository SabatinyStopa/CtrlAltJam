using System.Collections;
using CtrlJam.Enums;
using CtrlJam.Managers;
using UnityEngine;

namespace CtrlJam.Player
{
    public class LayerController : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private Color mainColor = Color.white;
        [SerializeField] private Color firstColor = Color.blue;
        [SerializeField] private Color secondColor = Color.red;
        [SerializeField] private Color thirdColor = Color.green;
        private ObjectLayer currentLayer;
        private float changingColorTime = 0.05f;
        private bool changingColor = false;

        private void Start()
        {
            meshRenderer.sharedMaterial.SetFloat("_Fill", -1f);
            meshRenderer.sharedMaterial.SetColor("_Color", mainColor);
            meshRenderer.sharedMaterial.SetColor("_TargetColor", mainColor);
            currentLayer = ObjectLayer.main;
        }

        private void Update()
        {
            if (changingColor) return;

            var lastCurrentLayer = currentLayer;
            var targetColor = Color.white;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentLayer = ObjectLayer.first;
                targetColor = firstColor;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentLayer = ObjectLayer.second;
                targetColor = secondColor;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentLayer = ObjectLayer.third;
                targetColor = thirdColor;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentLayer = ObjectLayer.main;
                targetColor = mainColor;
            }

            if (lastCurrentLayer != currentLayer) StartCoroutine(ChangeColor(lastCurrentLayer, targetColor));
        }

        private IEnumerator ChangeColor(ObjectLayer lastCurrentLayer, Color targetColor)
        {
            var amount = -1f;

            changingColor = true;

            meshRenderer.sharedMaterial.SetFloat("_Fill", amount);
            meshRenderer.sharedMaterial.SetColor("_TargetColor", targetColor);

            while (amount < 1)
            {
                meshRenderer.sharedMaterial.SetColor("_TargetColor", targetColor);
                meshRenderer.sharedMaterial.SetFloat("_Fill", amount);
                amount += changingColorTime;

                yield return null;
            }

            meshRenderer.sharedMaterial.SetFloat("_Fill", -1f);
            meshRenderer.sharedMaterial.SetColor("_Color", targetColor);
            LayerObjectsManager.Instance.SetLayer(lastCurrentLayer, currentLayer);
            changingColor = false;
        }
    }
}