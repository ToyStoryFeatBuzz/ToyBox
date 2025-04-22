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

        private void Start() {
            AudioManager.Instance.PlayMusic("MenuMusic");
        }
        
        public void PlayGame() {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX("ButtonClick1");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenMenu(GameObject menu) {
            AudioManager.Instance.PlaySFX("ButtonClick2");
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu) {
            AudioManager.Instance.PlaySFX("ButtonClick2");
            menu.SetActive(false);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}
