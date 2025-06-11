using UnityEngine;

namespace Extension {
    
    public class MonoSingleton<T>: MonoBehaviour where T: class {

        public static T Instance { get; private set; } = null;
        protected static bool destroyOnLoad = true;
        
        protected void Awake() {
            
            if (Instance == null) {
                Instance = this as T;
                if (!destroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this) {

                Debug.LogError(
                    $"{nameof(T)} is able to exist just one.\n"
                    + $"({(Instance as MonoBehaviour).gameObject.name}, {gameObject.name})");
                
                Destroy(gameObject);
            }
                
        }

    }
}