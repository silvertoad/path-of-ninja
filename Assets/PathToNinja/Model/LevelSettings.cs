using UnityEngine;

namespace PathToNinja.Model
{
    public class LevelSettings : MonoBehaviour
    {
        [SerializeField] private int _maxDashCount;
        [SerializeField] private string _sceneName;
        [SerializeField] private string _nextSceneName;

        public int MaxDashCount => _maxDashCount;
        public string SceneName => _sceneName;
        public string NextScene => _nextSceneName;
    }
}