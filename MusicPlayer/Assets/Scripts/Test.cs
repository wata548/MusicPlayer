using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            YoutubeDownload.Download("https://www.youtube.com/watch?v=OLRbIc8KZ_8");
        }
    }
}