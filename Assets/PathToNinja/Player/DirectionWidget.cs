using UnityEngine;

namespace PathToNinja.Player
{
    public class DirectionWidget : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Vector3 _offset;

        private SwipeControl _control;
        private Transform _target;

        private void Awake()
        {
            _control = GameObject.FindObjectOfType<SwipeControl>();
            _target = FindObjectOfType<Hero>().transform;
        }

        private void Update()
        {
            _sprite.enabled = _control.IsDragged;
            if (_control.IsDragged)
            {
                var angle = Mathf.Atan2(_control.Direction.normalized.y, _control.Direction.normalized.x) *
                            Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                _sprite.size = new Vector2(_control.Direction.magnitude, 1);
                transform.position = _target.position + _offset;
            }
        }
    }
}