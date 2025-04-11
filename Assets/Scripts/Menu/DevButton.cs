using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Toybox.Menu{
    public class ScDevButton : MonoBehaviour {

        [SerializeField] private SoDevInfo _devInfo;

        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] Image _image;

        private void Start() {
            _name.text = _devInfo.Name + "<br>" + _devInfo.Surname;
            _image.sprite = _devInfo.Avatar;
        }

        public void OnClick() {
            Application.OpenURL(_devInfo.ItchioUrl);
        }
    }
}
