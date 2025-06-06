using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class Search: MonoBehaviour {

        [SerializeField] private MusicPlayer _player;
        [SerializeField] private TMP_InputField _inputField;
        
        public void SearchContext() {
            _player.SearchMp3(_inputField.text);
        }
    }
}