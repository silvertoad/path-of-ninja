using System.Linq;
using PathToNinja.Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace PathToNinja
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelBind _bind;
        [SerializeField] private EventBind _onReplay;
        [SerializeField] private EventBind _onLevelComplete;
        [SerializeField] private BoolEventBind _showLevelCompletePopup;

        private LevelSettings _settings;
        private Enemy[] _enemies;

        private void Awake()
        {
            _settings = FindObjectOfType<LevelSettings>();
            Assert.IsNotNull(_settings, "Undefined level settings.");

            _enemies = FindObjectsOfType<Enemy>();
            _bind.Init(_settings);

            _onLevelComplete.Event.AddListener(OnLevelComplete);
            _onReplay.Event.AddListener(OnReplay);
        }

        private void OnReplay()
        {
            SceneManager.LoadScene(_settings.SceneName);
        }

        private void OnLevelComplete()
        {
            var isSucceed = _enemies.All(enemy => enemy.IsDead);
            _showLevelCompletePopup.Event?.Invoke(isSucceed);
        }

        private void OnDestroy()
        {
            _onLevelComplete.Event.RemoveListener(OnLevelComplete);
            _onReplay.Event.RemoveListener(OnReplay);
        }
    }
}