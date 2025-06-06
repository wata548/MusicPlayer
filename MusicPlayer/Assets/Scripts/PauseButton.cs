using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class PauseButton: MonoBehaviour {

        [SerializeField] private Image _image;
        
        [SerializeField] private MusicPlayer _player;
        [SerializeField] private Sprite _pause;
        [SerializeField] private Sprite _unPause;

        public void Click() {

            if (_player.IsPause) {
                _player.UnPause();
                _image.sprite = _pause;
            }
            else {
                _player.Pause();
                _image.sprite = _unPause;
            }
        }
    }
}