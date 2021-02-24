using PathToNinja.Model;
using UnityEngine;
using UnityEngine.UI;

namespace PathToNinja.UI
{
    public class CompleteLevelPopup : MonoBehaviour
    {
        [SerializeField] private Text _resultText;

        [Space] [SerializeField] private EventBind _onReplay;
        [Space] [SerializeField] private EventBind _onNext;

        private void OnEnable()
        {
            UpdateResult();
        }

        private void UpdateResult()
        {
        }

        public void OnReplayClick()
        {
            _onReplay.Event.Invoke();
        }

        public void OnNextClick()
        {
            _onNext.Event.Invoke();
        }

        public void SetKilled(bool isSucceed)
        {
            _resultText.text = isSucceed ? "You Win!" : "You Loose!";
        }
    }
}