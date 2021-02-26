using System.Collections;
using PathToNinja.Components;
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
        [SerializeField] private float _destPointMagnitude;
        [SerializeField] private LayerMask _platformLayer;
        [SerializeField] private TriggerState _isGrounded;

        [Space] [SerializeField] private LevelBind _bind;
        [SerializeField] private EventBind _onHeroDone;
        [SerializeField] private EventBind _onEnemyDie;

        [Space] [SerializeField] private Vector3 _footOffset;
        [SerializeField] private float _groundCheckDistance;

        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IdleKey = Animator.StringToHash("idle");
        private static readonly int SlashKey = Animator.StringToHash("slash");
        private static readonly int IsDashingKey = Animator.StringToHash("is-dashing");

        private bool _checkForExit;
        private float _originalGravity;
        private Vector3 _destination;
        [SerializeField] private bool _isDashing;

        void Start()
        {
            _originalGravity = _body.gravityScale;
        }

        void Update()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            var noDashes = _bind.DashLasts <= 0;
            if (noDashes) return;

            var screenPosition = Input.mousePosition;
            var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
            // DashTo(worldPosition);
        }

        public void DashTo(Vector3 direction)
        {
            if (_bind.IsCompleted) return;
            
            var platform = GetPlatform();
            if (platform != null)
            {
                StartCoroutine(JumpDown(platform));
            }

            _bind.CurrentDashCount.Value++;
            _checkForExit = _bind.DashLasts <= 0;

            _body.velocity = Vector2.zero;
            var dashImpulse = direction.normalized * _dashSpeed * direction.magnitude;
            _body.AddForce(dashImpulse, ForceMode2D.Impulse);
        }

        private void FixedUpdate()
        {
            if (_isDashing)
            {
                var position = Vector3.MoveTowards(transform.position, _destination,
                    Time.fixedDeltaTime * _dashSpeed);
                _body.MovePosition(position);

                if ((transform.position - _destination).magnitude < _destPointMagnitude)
                {
                    _isDashing = false;
                    _animator.SetBool(IsDashingKey, false);
                }
            }
            else
            {
                var isGrounded = _isGrounded.IsInTrigger;

                _animator.SetBool(IsDashingKey, false);
                _animator.SetBool(IdleKey, isGrounded);
                _animator.SetFloat(VerticalVelocityKey, _body.velocity.y);
            }

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
            var platform = GetPlatform();
            if (platform != null)
            {
                StartCoroutine(JumpDown(platform));
            }

            _isDashing = true;
            _destination = destPosition;
            _bind.CurrentDashCount.Value++;
            _checkForExit = _bind.DashLasts <= 0;

            var dest = (destPosition - _body.position).normalized * _dashSpeed;
            transform.localScale = new Vector3(dest.x > 0 ? 1 : -1, 1, 1);

            _body.velocity = Vector2.zero;
            _animator.SetBool(IsDashingKey, true);
        }

        private IEnumerator JumpDown(Collider2D platformCollider)
        {
            platformCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            platformCollider.enabled = true;
        }

        private Vector3 FootPosition => transform.position + _footOffset;

        private Collider2D GetPlatform()
        {
            return Physics2D.Raycast(FootPosition, Vector2.down, _groundCheckDistance, _platformLayer).collider;
        }

        private void OnDrawGizmosSelected()
        {
            var footPosition = FootPosition;
            // Debug.DrawRay(footPosition, Vector2.down * _groundCheckDistance,
            //     _isGrounded.IsInTrigger ? Color.green : Color.red);
            
            Debug.DrawRay(footPosition, Vector2.down * _groundCheckDistance,
                GetPlatform() != null ? Color.green : Color.red);
            Gizmos.DrawIcon(footPosition, "Animation.FilterBySelection");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _body.gravityScale = _originalGravity;
            _isDashing = false;
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