using PathToNinja.Model;
using UnityEngine;
using UnityEngine.UI;

namespace PathToNinja.UI
{
    public class CompleteLevelPopup : MonoBehaviour
    {
        [SerializeField] private Text _resultText;

        [Space] [SerializeField] private EventBind _onReplay;

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

        public void SetKilled(bool isSucceed)
        {
            _resultText.text = isSucceed ? "You Win!" : "You Loose!";
        }
    }
}