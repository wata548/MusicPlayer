using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extension;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayListInfo {
    
    public const string Search = "Search";
    public const string All = "All";   
    
    [JsonProperty]
    public List<string> Url { get; private set; } = new();
    [JsonProperty]
    public Dictionary<string, List<int>> PlayList { get; private set; } = new();

    public List<int> this[string index] => PlayList[index];

    public void Shuffle(string playList) {
        PlayList[playList].Shuffle<int>();
    }

    public int GetIndex(string playList, Index i) =>
        PlayList[playList][i];
    
    public string GetLink(string playList, Index i) =>
        Url[PlayList[playList][i]];

    public void AddToPlayList(string playlist, int idx) {
        bool isAddable = true;
        foreach (var element in PlayList[playlist]) {
            if (element != idx)
                continue;
            
            isAddable = false;
            break;
        }

        if (isAddable)
            PlayList[playlist].Add(idx);
        
    }
    
    public void AddToPlayList(string playList, string context) {

        PlayList.TryAdd(playList,new());
        
        for (int i = 0; i < Url.Count; i++) {

            if (Url[i].Equals(context)) {
                
                if(!playList.Equals(All))
                    PlayList[playList].Add(i);
                return;
            }
        }
     
        Url.Add(context);
        PlayList[playList].Add(Url.Count - 1);
        if(!playList.Equals(All)) {
            PlayList[playList].Add(Url.Count - 1);
        }
    }
    
    public void ClearThrash() {
        var trashBin = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Thrash"));
        int cnt = 0;
        foreach (var thrash in trashBin.GetFiles()) {
            cnt++;
            File.Delete(thrash.FullName);
        }

        Log.Instance.ShowLog($"{cnt} file is deleted on thrash bin");
    }

    public void Rename(string origin, string newName) {
        if (!PlayList.ContainsKey(origin))
            return;

        PlayList[newName] = PlayList[origin];
        PlayList.Remove(origin);
    }
    
    public void Remove(int target) {

        var targetPath = Url[target];
        var targetName = Path.GetFileName(targetPath);
        var trashBin = Path.Combine(Application.streamingAssetsPath, "Thrash");
        var destination = Path.Combine(trashBin, targetName);

        try {

            File.Move(targetPath, destination);
        }
        catch (IOException) {
            
            File.Delete(Url[target]);
            Debug.Log("this file already exist on thrash bin");
        }

        Url.RemoveAt(target);

        var result = new Dictionary<string, List<int>>();
        foreach (var playlist in PlayList) {
            result[playlist.Key] = playlist.Value
                .Where(idx => idx != target)
                .Select(idx => idx > target ? idx - 1 : idx)
                .ToList();
        }

        PlayList = result;
        Log.Instance.ShowLog($"{targetName} is deleted");
    }

    public void Remove(int idx, string playlist) {
        if (playlist == All)
            return;

        PlayList[playlist] = PlayList[playlist]
            .Where(element => element != idx)
            .ToList();

        var name = Path.GetFileNameWithoutExtension(Url[idx]);
        Log.Instance.ShowLog($"{name} is deleted on {playlist} playlist");
    }

    public List<string> GetPlayLists() =>
        PlayList
            .Select(element => element.Key)
            .Where(element => element != Search)
            .ToList();
    
    
    public PlayListInfo() {
        PlayList.Add("All", new());
    }
}