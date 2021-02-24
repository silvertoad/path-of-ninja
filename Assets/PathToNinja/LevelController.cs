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
        [SerializeField] private EventBind _onNext;
        [SerializeField] private EventBind _onLevelComplete;
        [SerializeField] private EventBind _onEnemyDie;
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
            _onEnemyDie.Event.AddListener(OnEnemyDie);
            _onReplay.Event.AddListener(OnReplay);
            _onNext.Event.AddListener(OnNext);

            if (!SceneManager.GetSceneByName("Scenes/UI").isLoaded)
            {
                SceneManager.LoadScene("Scenes/UI", LoadSceneMode.Additive);
            }
        }

        private void OnEnemyDie()
        {
            var isSucceed = _enemies.All(enemy => enemy.IsDead);
            if (isSucceed)
            {
                _bind.IsCompleted = true;
                _showLevelCompletePopup.Event?.Invoke(true);
            }
        }

        private void OnReplay()
        {
            SceneManager.LoadScene(_settings.SceneName);
        }

        private void OnNext()
        {
            SceneManager.LoadScene(_settings.NextScene);
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