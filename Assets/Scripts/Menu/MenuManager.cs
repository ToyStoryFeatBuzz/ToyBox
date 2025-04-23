using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToyBox.Menu {
    public class MenuManager : MonoBehaviour {

        public static MenuManager Instance { get; private set; }
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public void PlayGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenMenu(GameObject menu) {
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu) {
            menu.SetActive(false);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}
