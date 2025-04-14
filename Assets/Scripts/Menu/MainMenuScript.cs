using UnityEngine;

namespace ToyBox.Menu {
    public class MainMenuScript : MonoBehaviour {

        public void PlayGame() {
            // Fill in later
        }

        public void OpenMenu(GameObject menu) {
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu) {
            menu.SetActive(false);
        }

        public void OpenWebsite(string url) {
            Application.OpenURL(url);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}
