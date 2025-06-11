using System;
using UnityEngine;

namespace DefaultNamespace {
    public class MoveTimeButton: MonoBehaviour {
        private const float MoveTime = 5; 
        
        public void Right() {
            MusicPlayer.Instance.AddTime(MoveTime);
            Log.Instance.ShowLog($"Move music progress to {MoveTime}Seconds");
        }

        public void Left() {
            MusicPlayer.Instance.AddTime(-MoveTime);
            Log.Instance.ShowLog($"Move music progress to {-MoveTime}Seconds");
        }

        public void ToEnd() {
            
            MusicPlayer.Instance.TimeSet(1);
            Log.Instance.ShowLog("Move music progress to end");
        }

        public void ToStart() {
            
            MusicPlayer.Instance.TimeSet(0);
            Log.Instance.ShowLog("Move music progress to start");
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Left();
            if(Input.GetKeyDown(KeyCode.RightArrow))
                Right();
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ToStart();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                ToEnd();

        }
    }
}