using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {
    public class YoutubeDownloader: MonoBehaviour {
        private const int MaxLatter = 42;
        
        [SerializeField] private GameObject _progressWindow; 
        [SerializeField] private Slider _progressSlider; 
        [SerializeField] private TMP_Text _fileName;
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private TMP_Text _playlistProgress;
        
        public void Kill() {
            YoutubeDownload.TaskKill();
        } 
        
        private void Update() {

            if (YoutubeDownload.Downloading) {
                _progressWindow.SetActive(true);
                _progressSlider.value = YoutubeDownload.Progress / 100f;
                if(YoutubeDownload.FileName.Length > MaxLatter)
                    _fileName.text = YoutubeDownload.FileName.Substring(0, MaxLatter) + "...";
                else 
                    _fileName.text = YoutubeDownload.FileName;
                _progress.text = $"{YoutubeDownload.Progress}% ({YoutubeDownload.Speed}Mib/s)";
                _playlistProgress.text = $"{YoutubeDownload.CurPlayListCount} of {YoutubeDownload.PlayListCount}";
            }
            else {
                _progressWindow.SetActive(false);
                if (!WebBrowsing.GetDownloadMember(out var url)) {
                    return;
                }

                YoutubeDownload.Download(url);
            }
        }
    }
}