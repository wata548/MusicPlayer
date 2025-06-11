using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            MusicPlayer.Instance.FileUpdate();
        }
    }
}