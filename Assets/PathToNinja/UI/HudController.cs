using PathToNinja.Model;
using UnityEngine;

namespace PathToNinja.UI
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private DashCounter _counter;
        [SerializeField] private LevelBind _level;
        [SerializeField] private CompleteLevelPopup _completePopup;

        [Space] [SerializeField] private BoolEventBind _onShowResult;

        private void Start()
        {
            _level.Settings.OnChanged += x => UpdateDashCounter();
            _level.CurrentDashCount.OnChanged += x => UpdateDashCounter();
            _onShowResult.Event.AddListener(OnShowResults);

            UpdateDashCounter();
        }

        private void OnShowResults(bool isSucceed)
        {
            _completePopup.SetSucceed(isSucceed);
            _completePopup.gameObject.SetActive(true);
        }

        private void UpdateDashCounter()
        {
            _counter.SetCount(_level.CurrentDashCount.Value, _level.Settings.Value.MaxDashCount);
        }

        private void OnDestroy()
        {
            _level.Settings.CleanEvent();
            _level.CurrentDashCount.CleanEvent();

            _onShowResult.Event.RemoveListener(OnShowResults);
        }
    }
}