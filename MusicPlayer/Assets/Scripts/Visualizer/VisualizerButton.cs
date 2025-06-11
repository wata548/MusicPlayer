using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class VisualizerButton: MonoBehaviour {

        [SerializeField] private AudioVisualizer _visualizer;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _on;
        [SerializeField] private Sprite _off;
        private bool _isOn = true;

        private void SetImage() {
            if(_isOn)
                _image.sprite = _on;
            else
                _image.sprite = _off;
        }
        
        public void Change() {
            _isOn = !_isOn;
            SetImage();
            _visualizer.gameObject.SetActive(_isOn);
        }
        
        private void Awake() {
            SetImage();
        }
    }
}