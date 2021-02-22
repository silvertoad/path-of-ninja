using PathToNinja.Model;
using UnityEngine;

namespace PathToNinja
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private Camera _camera;
        [SerializeField] private Animator _animator;

        [Space] [SerializeField] private float _dashSpeed;
        [SerializeField] private float _restTreshold;

        [Space] [SerializeField] private LevelBind _bind;
        [SerializeField] private EventBind _onHeroDone;
        [SerializeField] private EventBind _onEnemyDie;

        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IdleKey = Animator.StringToHash("idle");
        private static readonly int SlashKey = Animator.StringToHash("slash");

        private bool _checkForExit;

        void Update()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            var noDashes = _bind.DashLasts <= 0;
            if (noDashes) return;

            var screenPosition = Input.mousePosition;
            var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            DashTo(worldPosition);
        }

        private void FixedUpdate()
        {
            _animator.SetBool(IdleKey, _body.velocity.y == 0);
            _animator.SetFloat(VerticalVelocityKey, _body.velocity.y);

            if (_checkForExit)
            {
                if (_body.velocity.magnitude <= _restTreshold)
                {
                    _onHeroDone.Event.Invoke();
                    _checkForExit = false;
                }
            }
        }

        private void DashTo(Vector2 destPosition)
        {
            _bind.CurrentDashCount.Value++;
            _checkForExit = _bind.DashLasts <= 0;

            var dest = (destPosition - _body.position).normalized * _dashSpeed;
            transform.localScale = new Vector3(dest.x > 0 ? 1 : -1, 1, 1);

            _body.velocity = Vector2.zero;
            _body.AddForce(dest, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (!enemy) return;

            Slash();
            enemy.Die();
            _onEnemyDie.Event.Invoke();
        }

        private void Slash()
        {
            _animator.SetTrigger(SlashKey);
        }
    }
}