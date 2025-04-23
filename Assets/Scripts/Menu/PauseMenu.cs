using ToyBox.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class PauseMenu : MonoBehaviour {
        public static PauseMenu Instance { get; private set; }
        PauseManager _pauseManager => PauseManager.Instance;

        public GameObject Panel;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public void Resume() => _pauseManager.EndPause();
        
        public void MainMenu() {
            SceneManager.LoadScene(0);
        }
        
        public void Select(Selectable selectable) => selectable.Select();

        public void OpenMenu(GameObject menu) {
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu) {
            menu.SetActive(false);
        }
    }
}