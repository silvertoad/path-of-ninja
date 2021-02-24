using System.Collections;
using System.Linq;
using PathToNinja.Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

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
        private bool _isInCompleteRoutine;

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
                Debug.Log("die");
                StartCoroutine(SucceedRoutine());
            }
        }

        private IEnumerator SucceedRoutine()
        {
            _isInCompleteRoutine = true;
            
            var pixelCamera = FindObjectOfType<PixelPerfectCamera>();
            var hero = FindObjectOfType<Hero>();

            var dest = new Vector3(hero.transform.position.x, hero.transform.position.y,
                pixelCamera.transform.position.z);
            var frames = 10;
            var currentFrame = 0;
            var oldPos = pixelCamera.transform.position;
            var oldPpu = pixelCamera.assetsPPU;
            var defaultFrameRate = Application.targetFrameRate;
            Application.targetFrameRate = 15;
            while (currentFrame <= frames)
            {
                var progress = currentFrame / (float) frames;
                pixelCamera.transform.position = Vector3.Lerp(oldPos, dest, progress);

                pixelCamera.assetsPPU = (int) Mathf.Lerp(oldPpu, 50, progress);
                currentFrame++;

                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            Application.targetFrameRate = defaultFrameRate;
            _bind.IsCompleted = true;
            _showLevelCompletePopup.Event?.Invoke(true);
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
            if (_isInCompleteRoutine) return;
            
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