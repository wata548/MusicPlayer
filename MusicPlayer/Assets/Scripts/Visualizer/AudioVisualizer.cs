using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class AudioVisualizer: MonoBehaviour {

        private const float YPos = -1.5f;
        private const float StartPos = -7.3f;
        private const float EndPos = -0.3f;
        
        private const int Count = 16;
        private const int SpectrumDetail = 16;

        private const float SpectrumScale = 12f;
        private const float MinSpectrumScale = 0.1f;
        private const float MaxSpectrumScale = 4.5f;
        private const float LerpScale = 15;
        
        private const float Margin = 0.05f;
        
        [SerializeField] private GameObject _barPrefab;
        [SerializeField] private AudioSource _player;

        private List<GameObject> _bars = new();

        private void Awake() {

            var pos = new Vector2(StartPos, YPos);
            var interval = (EndPos - StartPos) / Count;
            var scale = new Vector2(interval - Margin, MinSpectrumScale);
            _barPrefab.transform.localScale = scale;
            
            for (int i = 0; i < Count; i++) {
                var newBar =Instantiate(_barPrefab, transform);
                    
                newBar.transform.position = pos;
                _bars.Add(newBar);
                pos.x += interval;
            }
        }
        
        private void Update() {

            if (!_player.isPlaying) 
                return;
            
            var data = new float[Count * SpectrumDetail];
            _player.GetSpectrumData(data, 0, FFTWindow.Triangle);

            for (int i = 0; i < Count; i++) {

                var scale = _bars[i].transform.localScale;
                var curScale = Mathf.Clamp(
                    data[i] * SpectrumScale,
                    MinSpectrumScale,
                    MaxSpectrumScale
                );

                scale.y = Mathf.Lerp(scale.y, curScale, Time.deltaTime * LerpScale);

                _bars[i].transform.localScale = scale;
            }
        }
    }
}