using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DefaultNamespace;
using Extension;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour {

    private const float Size = 1080;
    private const float PrefabSize = 40;
    private const float ScrollPower = 10;
    private const string Search = "Search";
    private const string All = "All";

    [SerializeField] private AudioSource _player;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Slider _progress;
    private int _startIdx = 0;
    private int _lastIdx = -1;
    private bool _isPause = false;

    private string _playList = All;
    
    private Dictionary<string, List<string>> _audioClipPaths = new();

    private List<PlayButton> _buttons = new();

    public bool IsPause => _isPause;
    
    private void SetUp() {
        for (int i = 0; i <= Size / PrefabSize; i++) {

            var pos = new Vector2(560, -(Size + PrefabSize) * 0.5f + i * PrefabSize);
            var newObject = Instantiate(_prefab, _canvas.transform);
            newObject.transform.localPosition = pos;
            _buttons.Add(newObject.GetComponent<PlayButton>());
        }
    }

    private void FindMp3() {
        
        var directoryPath = Path.Combine(Application.streamingAssetsPath, "Music");
        var directory = new DirectoryInfo(directoryPath);

        var files = directory.GetFiles()
            .Where(file => file.Extension == ".mp3")
            .Select(file => file.FullName)
            .ToList();

        _audioClipPaths.Clear();
        _audioClipPaths.Add(All, new());

        foreach (var file in files) {
            var playLists = file.Split('`')
                .Skip(1)
                .Distinct();

            _audioClipPaths[All].Add(file);

            foreach (var playList in playLists) {

                var playListFix = playList.Trim();
                if (playListFix == All)
                    continue;

                _audioClipPaths.TryAdd(playListFix, new());
                _audioClipPaths[playListFix].Add(file);
            }
        }

        foreach (var playList in _audioClipPaths) {
            playList.Value.Sort();
        }
    }

    public void SearchMp3(string searchTarget) {

        _playList = Search;
        
        _audioClipPaths.TryAdd(Search, new());
        _audioClipPaths[Search].Clear();

        var temp = searchTarget
            .Split(' ')
            .Select(target => target.Trim())
            .Where(target => !string.IsNullOrWhiteSpace(target))
            .Distinct();
        searchTarget = string.Join('|', temp);

        var result = new List<(int, string)>();
        foreach (var link in _audioClipPaths[All]) {

            string fileName = Path.GetFileNameWithoutExtension(link).Split('`')[0];
            var match = Regex.Matches(fileName, searchTarget, RegexOptions.IgnoreCase);
            var count = match
                .Select(match => match.Value)
                .Distinct().Count();
            
            if(count == temp.Count())
                _audioClipPaths[Search].Add(link);
        }

        _audioClipPaths[Search].Sort();

        UpdateList();
    }
    
    public void Pause() {
        _isPause = true;
        _player.Pause();
    }
    public void UnPause() {

        _isPause = false;
        _player.UnPause();
    }
    
    public void TimeSet(float ratio) {

        ratio = Mathf.Clamp(ratio, 0, 1);
        
        if (_player.clip == null )
            return;
        if (!_player.isPlaying)
            return;

        var curRatio = _player.time / _player.clip.length;
        if (Mathf.Abs(curRatio - ratio) <= 0.001f)
            return;
        
        var time = _player.clip.length * ratio;
        _player.time = time;
    }

    private void ScrollUpdate() {
        
        var scrollDelta = Input.mouseScrollDelta.y * ScrollPower;
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
            float maxYPos = (Size + PrefabSize) * 0.5f;
            var pos = button.transform.localPosition;

            if (maxYPos <= pos.y) {
                pos.y -= Size + PrefabSize;
                button.transform.localPosition = pos;
                deltaIndex++;
            }
            else if (-maxYPos >= pos.y) {
                pos.y += Size + PrefabSize;
                button.transform.localPosition = pos;
                deltaIndex--;
            }
        }
            
        _buttons = _buttons
            .OrderByDescending(button => button.transform.localPosition.y)
            .ToList();

        _startIdx += deltaIndex;
        for (int i = 0; i < _buttons.Count; i++) {
            int idx = _startIdx + i;
            if (idx < 0 || idx >= _audioClipPaths[_playList].Count) {

                _buttons[i].SetLink();
                continue;
            }

            _buttons[i].SetLink(idx + 1, _audioClipPaths[_playList][idx]);
        }
    }
    
    private void Start() {

        SetUp();
        Application.runInBackground = true;

        FindMp3();
        UpdateList();
    }
    private void Update() {
        if (_player.clip != null)
            _progress.value = _player.time / _player.clip.length;
        else
            _progress.value = 0;
        
        ScrollUpdate();

        if (Mp3Load.LoadEnd && Mp3Load.LoadSuccess) {
            _player.clip = Mp3Load.Result;
            _player.Play();
        }
        //if(_player.isPlaying && _lastIdx != -1)
    }
}