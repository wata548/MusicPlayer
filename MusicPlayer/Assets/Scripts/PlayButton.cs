using System.IO;
using Extension;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class PlayButton : MonoBehaviour {

        private const int CharacterLimit = 42;
        
        public string Link { get; private set; }
        public int Index { get; private set; }

        [SerializeField] private TMP_Text _context;

        public void SetLink() {
            _context.text = "";
        }
        
        public void SetLink(int index, string link) {
            Link = link;
            Index = index;

            var context = $"   {index}. {Path.GetFileNameWithoutExtension(link).Split('`')[0]}";
            if (context.Length > CharacterLimit)
                context = context.Substring(0, CharacterLimit) + "...";
            _context.text = context;
        }

        public void Click() {
            StartCoroutine(Mp3Load.LoadMP3(Link));
        }
    }
}
        