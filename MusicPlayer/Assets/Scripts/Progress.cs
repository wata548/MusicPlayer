using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace DefaultNamespace {
    public class Progress: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        private const float WidthSize = 1920;
        private const float Size = 800;
        private const float UnpauseInterval = 0.1f;
        
        [SerializeField] private Slider _progress;
        [SerializeField] private MusicPlayer _player;
        private bool _onMouse;
        private bool _clicking = false;
        private float _lastMove = 0;


        public void OnPointerEnter(PointerEventData eventData) {
            _onMouse = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _onMouse = false;
        }

        public void Update() {

            if (_onMouse && Input.GetMouseButtonDown(0)) {
                _clicking = true;
            }

            if (Input.GetMouseButtonUp(0)) {
                
                _clicking = false;
                //_player.UnPause();
            }

            if (!_clicking)
                return;
            
            if (Input.mousePositionDelta.magnitude <= 0) {
                
                if (Time.time - _lastMove >= UnpauseInterval) {
                    //_player.UnPause();
                }
                return;
            }

            _lastMove = Time.time;
            var distance = (Input.mousePosition - transform.localPosition).x 
                           - (WidthSize - Size) / 2;
            _player.TimeSet(distance / Size);
            //_player.Pause();
        }
    }
}