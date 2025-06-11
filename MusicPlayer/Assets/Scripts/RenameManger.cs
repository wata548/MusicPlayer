using Extension;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class RenameManger: MonoSingleton<RenameManger> {

        [SerializeField] private GameObject _window;
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _error;
        [SerializeField] private TMP_InputField _input;
        private string _origin;

        public void Rename(string origin = "") {

            if (string.IsNullOrWhiteSpace(origin))
                _title.text = "Make playlist";
            else
                _title.text = "Rename playlist";
            
            _error.text = "";
            _input.text = "";
            _origin = origin;
            _window.SetActive(true);
        }

        public void TurnOff() {
            _origin = "";
            _window.SetActive(false);
        }

        public void Submit() {
            
            if (string.IsNullOrWhiteSpace(_origin)) {

                if (MusicPlayer.Instance.MakePlaylist(_input.text)) {

                    Log.Instance.ShowLog($"{_input.text} is maked");
                    TurnOff();
                }
                else {
                    _error.text = "This name is already exist";
                }
            }
            else {
                if (MusicPlayer.Instance.RenamePlaylist(_origin, _input.text)) {
                    
                    Log.Instance.ShowLog($"{_origin} is renamed to {_input.text}");
                    TurnOff();
                }
                else {

                    _error.text = "This name is already exist";
                }
            }
        }
    }
}