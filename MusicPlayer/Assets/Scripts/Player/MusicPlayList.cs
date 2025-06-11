using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DG.Tweening.Core.Easing;
using Newtonsoft.Json;
using SFB;
using UnityEngine;

public partial class MusicPlayer {

    enum PlaylistSelectType {
        Music,
        ChangePlayList,
        AddToPlayList,
    }
    private PlaylistSelectType _selectType;
    private int _addTarget = -1;
    
    [SerializeField] private GameObject _search;
    
    private string PlayListJson = "PlayList.json";

    private void Mp3PathSetUp() {

        var rawListInfo = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, PlayListJson));
        _audioData = JsonConvert.DeserializeObject<PlayListInfo>(rawListInfo);

        foreach (var playList in _audioData.PlayList) {
            playList.Value.Sort();
        }
    }

    public void SearchButton() {
        if (_playList == PlayListInfo.Search) {
            _search.SetActive(false);
            
            _playList = _previousList;
            UpdateList();
        }
        else {
            _previousList = _playList;
            _playList = PlayListInfo.Search;
            _search.SetActive(true);
        }
    }
    
    public void SearchMp3(string searchTarget) {
        
        _audioData.PlayList.TryAdd(PlayListInfo.Search, new());
        _audioData[PlayListInfo.Search].Clear();

        var temp = searchTarget
            .Split(' ')
            .Select(target => target.Trim())
            .Where(target => !string.IsNullOrWhiteSpace(target))
            .Distinct();
        searchTarget = string.Join('|', temp);

        var result = new List<(int, string)>();
        for (int i = 0; i < _audioData[PlayListInfo.All].Count; i++){

            string path = _audioData.GetLink(PlayListInfo.All, i);
            string fileName = Path.GetFileNameWithoutExtension(path);
            var match = Regex.Matches(fileName, searchTarget, RegexOptions.IgnoreCase);
            var count = match
                .Select(match => match.Value)
                .Distinct().Count();
            
            if(count == temp.Count())
                _audioData.AddToPlayList(PlayListInfo.Search, path);
        }

        _audioData[PlayListInfo.Search].Sort();

        UpdateList();
    }

    public void AddToPlayList(string playList, string path) {
        if (string.IsNullOrWhiteSpace(path))
            return;

        _audioData.AddToPlayList(playList, path);
    }

    public void FileUpdate() {
        
        var directory = new DirectoryInfo (Path.Combine(Application.streamingAssetsPath, "Music"));
        var updateTarget = directory
            .GetFiles("*.mp3")
            .Select(fileinfo => fileinfo.Name)
            .Except(_audioData.Url, StringComparer.InvariantCultureIgnoreCase);

        int cnt = 0;
        foreach (var target in updateTarget) {
            cnt++;
            _audioData.AddToPlayList(PlayListInfo.All, 
                Path.Combine(Application.streamingAssetsPath, "Music", target));
        }
        
        Log.Instance.AddLog(AddPosition.Find, cnt);

        UpdateList();
    }
    
    public void ClearThrash() {
        _audioData.ClearThrash();
        Log.Instance.ShowLog("Thrash is cleared");
    }
    
    public void FindFileInTrash() =>
        FindFiles(Path.Combine(Application.streamingAssetsPath, "Thrash"));
    
    public void FindFiles(string directory = "") {

        var paths = StandaloneFileBrowser.OpenFilePanel("Select Files", directory, "mp3", true);
        foreach (var path in paths) {
            var destination = Path.Combine(Application.streamingAssetsPath, "Music", Path.GetFileName(path));
            try {
                File.Copy(path, destination);
            }
            catch (IOException) {
                Debug.Log("This file already exist");
            }
            _audioData.AddToPlayList(_playList, destination);
        }
        Log.Instance.AddLog(AddPosition.Folder, paths.Length);

        UpdateList();
    }

    public void Reverse() {
        
        if(_audioData[_playList].Count >= 2 && _audioData[_playList][0] > _audioData[_playList][1])
            _audioData[_playList].Sort();
        else 
            _audioData[_playList].Reverse();

        Log.Instance.ShowLog($"{_playList} is reversed");
        UpdateList();
    }
    
    public void Shuffle() {
        _audioData.Shuffle(_playList);

        Log.Instance.ShowLog($"{_playList} is shuffled");
        UpdateList();
    }

    public void Remove(int idx) {
        
        _audioData.Remove(idx);
        
        UpdateList();
    }

    public void RemoveOnPlayList(int idx) {
        
        _audioData.Remove(idx, _playList);
        
        UpdateList();
    }
    
    public void DeletePlayList(string playList) {

        if (playList == PlayListInfo.All)
            return;
        _audioData.PlayList.Remove(playList);
        UpdateList();
    }

    public void AddToPlayList(int idx) {
        _selectType = PlaylistSelectType.AddToPlayList;
        _addTarget = idx;

        UpdateList();
    }
    
    public void ChangePlayList() {

        if (_selectType == PlaylistSelectType.ChangePlayList) {
            _selectType = PlaylistSelectType.Music;
        }
        else {
            _selectType = PlaylistSelectType.ChangePlayList;
            _playListShower.text = "   Select playlist";
        }
            
        UpdateList();
    }

    public void RenamePlaylist(string origin, string newName) {
        if (_playList == origin)
            _playList = newName;
        _audioData.Rename(origin, newName);
    }
    
    public void SelectPlayList(string playList) {

        switch (_selectType) {
            case PlaylistSelectType.ChangePlayList:
                Log.Instance.ShowLog($"go {_playList} to {playList} playlist");
                _playList = playList;
                PlayListShowerUpdate();
                break;
            case PlaylistSelectType.AddToPlayList:
                PlayListShowerUpdate();
                _audioData.AddToPlayList(playList, _addTarget);
                var targetName = Path.GetFileNameWithoutExtension(_audioData.Url[_addTarget]);
                Log.Instance.ShowLog($"{targetName} is added to {playList}");
                break;               
        }

        _selectType = PlaylistSelectType.Music;

        UpdateList();
    }
    
    private void OnApplicationQuit() {

        //save data
        var data = JsonConvert.SerializeObject(_audioData, Formatting.Indented);
        var path = Path.Combine(Application.streamingAssetsPath, "PlayList.Json");
        File.WriteAllText(path, data);
    }
}