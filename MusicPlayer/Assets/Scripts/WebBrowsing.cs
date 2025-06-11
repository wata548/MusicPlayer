using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace {
    public class WebBrowsing: MonoBehaviour {

        [DllImport("Youtube.dll")]
        public static extern void ShowWindows();

        [DllImport("Youtube.dll")]
        private static extern void CloseWindows();

        [DllImport("Youtube.dll")]
        private static extern IntPtr GetDownLoadUrl();

        public static bool GetDownloadMember(out string url) {

            url = "";
            IntPtr result = GetDownLoadUrl();
            if (result == IntPtr.Zero)
                return false;

            url = Marshal.PtrToStringAnsi(result);
            return true;
        }
        
        private void OnApplicationQuit() {
            CloseWindows();
        }
    }
}