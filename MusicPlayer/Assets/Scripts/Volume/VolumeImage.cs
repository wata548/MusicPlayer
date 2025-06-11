using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {

    public class VolumeImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        private bool _onMouse = false;
        [SerializeField] private VolumeSlider _slider;
        private Tween _animation;

        private void Awake() {
            
            MusicPlayer.Instance.SetVolume(_slider.Volume);
        }
        
        private void Update() {

            if (_onMouse) {
                _slider.gameObject.SetActive(true);

                if (_animation != null)
                    _animation.Kill();
                
                //slider is rotated 90degree
                _slider.SliderTransform.localScale = new(0, 1);
                _animation = _slider.SliderTransform.DOScaleX(1, 0.3f)
                    .SetEase(Ease.OutCubic);
            }

            if (!_onMouse && !_slider.OnMouse) {

                if (_animation != null)
                    _animation.Kill();

                _slider.SliderTransform.localScale = Vector2.one;
                _slider.gameObject.SetActive(false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _onMouse = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _onMouse = false;
        }
    }
}