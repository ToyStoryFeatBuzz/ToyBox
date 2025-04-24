using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Build {
    public class ChooseBoxAnim : MonoBehaviour {
        private BuildsManager _buildsManager => BuildsManager.Instance;
        public void OnOpened() {
            Debug.Log("Opened");
            _buildsManager.SpawnItem();
        }
    }
}