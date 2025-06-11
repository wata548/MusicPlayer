using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {

    public class VolumeSlider: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Slider _slider;

        public Transform SliderTransform => _slider.transform;
        
        public bool OnMouse { get; private set; } = false;
        public float Volume => _slider.value;
        
        public void ChangeVolume() {
            MusicPlayer.Instance.SetVolume(Volume);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnMouse = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnMouse = false;
        }
    }
}