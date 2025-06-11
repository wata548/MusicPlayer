using System;
using UnityEngine;

namespace Extension {
    public class CameraSize: MonoBehaviour {
        
        private const float Width = 1920;
        private const float Height = 1080;

        private float beforeWidth = 1920;
        private float beforeHeight = 1080;

        private void Update() {

            var scale = Camera.main!.rect;
            if (Mathf.Approximately(scale.x / scale.y, Width / Height)) {
                return;
            }

            if (!Mathf.Approximately(beforeHeight, scale.y)) {
                
                beforeWidth = scale.x;
                beforeHeight = Height / Width * beforeWidth;
            }
            
            else {
                beforeHeight = scale.y;
                beforeWidth = Width / Height * beforeHeight;
            }

            scale.x = beforeWidth;
            scale.y = beforeHeight;
            Camera.main!.rect = scale;
        }
    }
}