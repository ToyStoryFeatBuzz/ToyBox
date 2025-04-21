using UnityEngine;

namespace ToyBox.Menu {
    [CreateAssetMenu(fileName = "SoDevInfo", menuName = "Scriptable Objects/SoDevInfo")]

    public class SoDevInfo : ScriptableObject {
        public string Name;
        public string Surname;
        public string ItchioUrl;

        public Sprite Avatar;
    }
}
