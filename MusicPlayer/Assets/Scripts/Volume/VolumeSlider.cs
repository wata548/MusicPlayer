using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {

    public class VolumeSlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Slider _slider;
        [SerializeField] private AudioSource _player;

        public Transform SliderTransform => _slider.transform;
        
        public bool OnMouse { get; private set; } = false;
        
        public void ChangeVolume() {
            _player.volume = _slider.value;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnMouse = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnMouse = false;
        }
    }
}