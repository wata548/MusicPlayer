using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class PauseButton: MonoBehaviour {

        [SerializeField] private Image _image;
        [SerializeField] private Sprite _pause;
        [SerializeField] private Sprite _unPause;

        public void Click() {

            var _player = MusicPlayer.Instance;
            if (_player.IsPause) {
                
                _player.UnPause();
                _image.sprite = _pause;
            }
            else {
                
                _player.Pause();
                _image.sprite = _unPause;
            }
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Click();
            }
        }
    }
}