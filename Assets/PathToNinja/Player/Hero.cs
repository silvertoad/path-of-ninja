using System.Collections;
using PathToNinja.Components;
using PathToNinja.Model;
using UnityEngine;

namespace PathToNinja
{
    public class Hero : MonoBehaviour
    {
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsDashingKey = Animator.StringToHash("is-dashing");
        private static readonly int SlashKey = Animator.StringToHash("slash");
        private static readonly int IdleKey = Animator.StringToHash("idle");

        [Space] [SerializeField] private float _dashSpeed;
        [SerializeField] private float _restTreshold;
        [SerializeField] private LayerMask _platformLayer;
        [SerializeField] private TriggerState _isGrounded;

        [Space] [SerializeField] private LevelBind _bind;
        [SerializeField] private EventBind _onHeroDone;
        [SerializeField] private EventBind _onEnemyDie;

        [Space] [SerializeField] private Vector3 _footOffset;
        [SerializeField] private float _groundCheckDistance;

        private Vector3 FootPosition => transform.position + _footOffset;

        private bool _checkForExit;
        private Rigidbody2D _body;
        private Animator _animator;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void DashTo(Vector3 direction)
        {
            if (_bind.IsCompleted) return;

            if (_isGrounded.IsInTrigger)
            {
                _bind.CurrentDashCount.Value++;
                _checkForExit = _bind.DashLasts <= 0;
            }

            var platform = GetPlatform();
            if (platform != null)
            {
                StartCoroutine(JumpDown(platform));
            }

            _body.velocity = Vector2.zero;
            var dashImpulse = direction.normalized * _dashSpeed * direction.magnitude;
            _body.AddForce(dashImpulse, ForceMode2D.Impulse);
        }

        private void FixedUpdate()
        {
            var isGrounded = _isGrounded.IsInTrigger;

            _animator.SetBool(IsDashingKey, false);
            _animator.SetBool(IdleKey, isGrounded);
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

        private IEnumerator JumpDown(Collider2D platformCollider)
        {
            platformCollider.enabled = false;
            yield return new WaitForSeconds(0.2f);
            platformCollider.enabled = true;
        }

        private Collider2D GetPlatform()
        {
            return Physics2D.Raycast(FootPosition, Vector2.down, _groundCheckDistance, _platformLayer).collider;
        }

        private void OnDrawGizmosSelected()
        {
            var footPosition = FootPosition;

            Debug.DrawRay(footPosition, Vector2.down * _groundCheckDistance,
                GetPlatform() != null ? Color.green : Color.red);
            Gizmos.DrawIcon(footPosition, "Animation.FilterBySelection");
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