using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class Search: MonoBehaviour {

        [SerializeField] private TMP_InputField _inputField;
        
        public void SearchContext() {
            MusicPlayer.Instance.SearchMp3(_inputField.text);
        }
    }
}