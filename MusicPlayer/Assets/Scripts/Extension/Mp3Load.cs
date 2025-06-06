using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Extension {
    public static class Mp3Load {
        private static AudioClip _resultClip;
        private static bool _LoadSuccess = false;
        private static bool _loadEnd = false;

        public static bool LoadEnd => _loadEnd;
        public static bool LoadSuccess => _LoadSuccess;
        public static AudioClip Result {
            get {

                _loadEnd = false;
                _LoadSuccess = false;
                return _resultClip;
            }
        }


        //this audio clip save in ram, and affected by gc
        public static IEnumerator LoadMP3(string path) {

            _loadEnd = false;
            _LoadSuccess = false;
        
            var www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG);
            yield return www.SendWebRequest();

            _loadEnd = true;

            if (www.result != UnityWebRequest.Result.Success) {
            
                //throw new FileLoadException($"Fail to load mp3File({Path.GetFileNameWithoutExtension(path)}.mp3)");
                Debug.Log("Fail load mp3File");
                _LoadSuccess = false;
            }
        
            else {
                _resultClip = DownloadHandlerAudioClip.GetContent(www);
                _LoadSuccess = true;
            }
        
        }

    }
}