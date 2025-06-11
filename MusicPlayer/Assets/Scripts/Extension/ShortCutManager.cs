using System;
using UnityEngine;

namespace Extension {
    public class ShortCutManager: MonoBehaviour {
        private void Update() {

            bool isChangePlaylist = (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Tab))
                                    || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.Tab));
            if (isChangePlaylist)
                MusicPlayer.Instance.ChangePlayList();
        }
    }
}