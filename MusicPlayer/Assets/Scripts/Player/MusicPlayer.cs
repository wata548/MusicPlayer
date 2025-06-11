using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Extension;
using Extensions;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;

public partial class MusicPlayer : MonoSingleton<MusicPlayer> {

    private const float HeightSize = 1080;
    private const float PrefabSize = 40;
    private const float ScrollPower = 25;
    private const int StartIndex = -3;
    public static readonly KeyCode FunctionKey = KeyCode.LeftControl;
    
    [SerializeField] private AudioSource _player;
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _playlistButtonFolder;
    
    [SerializeField] private TMP_Text _songName;
    [SerializeField] private Slider _progress;

    [SerializeField] private TMP_Text _playListShower;

    private int _startIdx = StartIndex;
    private int _lastIdx = -1;
    private bool _isPause = false;

    private string _playList = PlayListInfo.All;
    private string _previousList = "";
    
    private PlayListInfo _audioData = new();

    private List<PlayButton> _buttons = new();

    public bool IsPause => _isPause;
    
    public void InitIndex() {
        _startIdx = StartIndex;
        var maxPos = (HeightSize + PrefabSize) * 0.5f;
        var delta = -maxPos + _buttons[0].transform.localPosition.y;
        delta += delta > 0 ? -0.01f : 0.01f;
        ScrollUpdate(delta);
    }
    
    private void UISetUp() {
        for (int i = 0; i <= HeightSize / PrefabSize; i++) {

            var pos = new Vector2((HeightSize + PrefabSize) / 2, -(HeightSize - PrefabSize) * 0.5f + i * PrefabSize);
            var newObject = Instantiate(_prefab, _playlistButtonFolder);
            newObject.transform.localPosition = pos;
            _buttons.Add(newObject.GetComponent<PlayButton>());
        }
    }

    public void Play(int index, string path) {

        _lastIdx = index;
        var name = Path.GetFileNameWithoutExtension(path);
        if (name.Length >= 28)
            name = name.SetFontSizeByPercent(0.8f);
        
        _songName.text = name;
        StartCoroutine(Mp3Load.LoadMP3(path));
    }
    public void Pause() {
        _isPause = true;
        _player.Pause();
    }
    public void UnPause() {

        _isPause = false;
        _player.UnPause();
    }
    public void SetVolume(float volume) {
        volume = Mathf.Clamp(volume, 0, 1);
        _player.volume = volume;
    }
    public void TimeSet(float ratio) {

        ratio = Mathf.Clamp(ratio, 0, 1);
        
        if (_player.clip == null )
            return;

        var curRatio = _player.time / _player.clip.length;
        if (Mathf.Abs(curRatio - ratio) <= 0.001f)
            return;
        
        var time = _player.clip.length * ratio;
        _player.time = time;
    }

    public void AddTime(float time) {
        if (_player.clip == null )
            return;
        
        _player.time = Mathf.Clamp(_player.time + time, 0, _player.clip.length);
    }

    private void ScrollUpdate(float scrollDelta) {
        
        if (Mathf.Abs(scrollDelta) <= 0.1f)
            return;
        
        foreach (var button in _buttons) {
            var pos = button.transform.localPosition;
            pos.y -= scrollDelta;
            button.transform.localPosition = pos;
        }

        UpdateList();
    }

    private void UpdateList() {
        
        int deltaIndex = 0;
        foreach (var button in _buttons) {
            float maxYPos = (HeightSize + PrefabSize) * 0.5f;
            var pos = button.transform.localPosition;

            if (maxYPos <= pos.y) {
                pos.y -= HeightSize + PrefabSize;
                button.transform.localPosition = pos;
                deltaIndex++;
            }
            else if (-maxYPos >= pos.y) {
                pos.y += HeightSize + PrefabSize;
                button.transform.localPosition = pos;
                deltaIndex--;
            }
        }
            
        _buttons = _buttons
            .OrderByDescending(button => button.transform.localPosition.y)
            .ToList();

        _startIdx += deltaIndex;
        for (int i = 0; i < _buttons.Count; i++) {
            if (_selectType  == PlaylistSelectType.Music)
                MusicListUpdate(i);
            else
                PlayListUpdate(i);
        }
        void MusicListUpdate(int i) {
            int idx = _startIdx + i;
            if (idx < 0 || idx >= _audioData[_playList].Count) {

                _buttons[i].SetLink();
                return;
            }

            var link = _audioData.GetLink(_playList, idx);
            var index = _audioData.GetIndex(_playList, idx);
            _buttons[i].SetLink(idx, link, index);    
        }
    
        void PlayListUpdate(int i) {

            int idx = _startIdx + i;
            var playlists = _audioData.GetPlayLists();
            if (idx < 0 || idx >= playlists.Count) {

                _buttons[i].SetPlayList();
                return;
            }

            _buttons[i].SetPlayList(playlists[idx]);    
        }
    }
    
    private void PlayListShowerUpdate() {
        _playListShower.text = $"   Playlist : {_playList}";
    }
    
    private new void Awake() {

        base.Awake();
        UISetUp();
        Application.runInBackground = true;
    }

    private void Start() {
        Mp3PathSetUp();
        FileUpdate();
        UpdateList();
    }

    private void Update() {

        
        if (_player.clip != null)
            _progress.value = _player.time / _player.clip.length;
        else
            _progress.value = 0;

        var scrollDelta = Input.mouseScrollDelta.y * ScrollPower;
        ScrollUpdate(scrollDelta);
        
        if (!Mp3Load.Loadding && Mp3Load.LoadSuccess) {
            
            _player.clip = Mp3Load.Result;
            _player.Play();
        }

        if (!_isPause && !_player.isPlaying && _lastIdx != -1 && !Mp3Load.Loadding) {

            _lastIdx++;
            if (_lastIdx < _audioData[_playList].Count)
                Play(_lastIdx, _audioData.GetLink(_playList, _lastIdx));
            else {
                _lastIdx = 0;
            }
        }
    }
}