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
        [SerializeField] private GameObject _winButton;

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
            gameObject.SetActive(false);
        }

        public void OnNextClick()
        {
            _onNext.Event.Invoke();
            gameObject.SetActive(false);
        }

        public void SetSucceed(bool isSucceed)
        {
            _resultText.text = isSucceed ? "You Win!" : "You Loose!";
            _winButton.SetActive(isSucceed);
        }
    }
}