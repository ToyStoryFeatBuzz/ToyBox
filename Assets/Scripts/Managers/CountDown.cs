using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Managers {
    public class CountDown : MonoBehaviour {
        private Image _image;
        [SerializeField] private List<Sprite> _sprites;

        private void Start() {
            _image = GetComponent<Image>();
        }
    }
}