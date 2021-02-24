using UnityEngine;

namespace PathToNinja.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerState : MonoBehaviour
    {
        [SerializeField] private LayerMask _trigger;
        [SerializeField] private bool _isInTrigger;
        [SerializeField] private Collider2D _collider;
        public bool IsInTrigger => _isInTrigger;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _isInTrigger = _collider.IsTouchingLayers(_trigger);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isInTrigger = false;
        }
    }
}