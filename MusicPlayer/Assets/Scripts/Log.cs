using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extension;
using TMPro;
using UnityEngine;

public enum AddPosition {
    Folder,
    Download,
    Find,
}
    
public class Log: MonoSingleton<Log> {

    private Tween _animation = null;
    [SerializeField] private TMP_Text _text;

    public void AddLog(AddPosition position, string songName) {
        ShowLog($"{songName} song is Added on {position}");
    }

    public void AddLog(AddPosition position, int count) {
        ShowLog($"{count} songs are Added on {position}");
    }
    
    public void DeleteLog(string songName) {
        ShowLog($"{songName} song is deleted");
    }
        
    public void ShowLog(string message) {

        if (_animation != null)
            _animation.Kill();

        _text.text = message;
        
        var color = _text.color;
        color.a = 1;
        
        _text.color = color;
        _text.DOFade(0, 5f)
            .SetEase(Ease.InSine);
    }
}