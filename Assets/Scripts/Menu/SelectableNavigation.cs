using System;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class SelectableNavigation  : MonoBehaviour{
        [SerializeField] StNavigation _navigation;
        
        protected Selectable _selectable;
        
        private void Start() {
            _selectable = GetComponent<Selectable>();
            _navigation.Init(_selectable);
            _navigation.ApplyNavigation();
        }
    }
    
    [Serializable]
    public struct StNavigation {
        public Selectable Up;
        public Selectable Down;
        public Selectable Left;
        public Selectable Right;

        private Selectable _selectable;

        public void Init(Selectable selectable) {
            _selectable = selectable;
            
        }

        public void ApplyNavigation() {
            Navigation navigation = _selectable.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnUp = Up;
            navigation.selectOnDown = Down;
            navigation.selectOnLeft = Left;
            navigation.selectOnRight = Right;
            _selectable.navigation = navigation;
        }
    }
}