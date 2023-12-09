using Microsoft.Unity.VisualStudio.Editor;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace CtrlJam.Common
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private GameObject playerObject;

        public void PlayGame()
        {
            playerObject.SetActive(true);
            videoPlayer.Play();
            Invoke("StartGame", (float)videoPlayer.length + 1f);
        }

        public void QuitGame() => Application.Quit();

        private void StartGame() => SceneManager.LoadScene(1);
    }
}