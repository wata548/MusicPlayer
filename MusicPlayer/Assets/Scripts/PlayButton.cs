using System;
using System.IO;
using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace {
    public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        //in music
        //left-click => Play
        // + Ctrl => Add to playlist
        //right-click => Erase at playlist
        // + Ctrl => Erase at all
        //
        //in playlist 
        //left-click => interact
        // + Ctrl => rename or make playlist
        //right-click => no action
        // + Ctrl => Erase this playlist
        
        private const int CharacterLimit = 42;
        
        public int RealIndex { get; private set; }
        public string Link { get; private set; }
        public int Index { get; private set; }
        public bool IsMusicMode { get; private set; } = true;

        [SerializeField] private TMP_Text _context;
        private bool _onPointer = false;

        public string PlayList { get; private set; }

        public void SetPlayList(string playListName) {
            IsMusicMode = false;
            PlayList = playListName;
            _context.text = $"   {playListName}";
        }

        public void SetPlayList() {
            
            IsMusicMode = false;
            PlayList = "";
            _context.text = "";
        }
        
        public void SetLink() {
            IsMusicMode = true;
            _context.text = "";
            RealIndex = -1;
            Index = -1;
            Link = "";
        }
        
        public void SetLink(int index, string link, int realIndex) {
            IsMusicMode = true;
            Link = link;
            Index = index;
            RealIndex = realIndex;

            var context = $"   {index + 1}. {Path.GetFileNameWithoutExtension(link).Split('`')[0]}";
            if (context.Length > CharacterLimit)
                context = context.Substring(0, CharacterLimit) + "...";
            _context.text = context;
        }

        private void Update() {

            if ((IsMusicMode && Index == -1) 
                || (!IsMusicMode && string.IsNullOrWhiteSpace(PlayList)))
                return;
            
            bool click = Input.GetMouseButtonUp(1);
            if (!click || !_onPointer)
                return;
            
            bool isFunction = Input.GetKey(MusicPlayer.FunctionKey);
            if (!IsMusicMode) {
                if (isFunction)
                    MusicPlayer.Instance.DeletePlayList(PlayList);
                return;
            }
            
            if (isFunction) {
                MusicPlayer.Instance.Remove(RealIndex);
            }
            else  {
                MusicPlayer.Instance.RemoveOnPlayList(RealIndex);
            }
        }

        public void Click() {

            if (!IsMusicMode) {

                bool isFunction = Input.GetKey(MusicPlayer.FunctionKey);

                if (isFunction) {
                    //if PlayList is is null or white space make new playList
                    RenameManger.Instance.Rename(PlayList);
                }
                else if (!string.IsNullOrWhiteSpace(PlayList))
                    MusicPlayer.Instance.SelectPlayList(PlayList);
                
                return;
            }
            
            if (Input.GetKey(MusicPlayer.FunctionKey)) {

                MusicPlayer.Instance.AddToPlayList(RealIndex);
                return;
            }
            
            if(Index != -1)
                MusicPlayer.Instance.Play(Index, Link);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _onPointer = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _onPointer = false;
        }
    }
}