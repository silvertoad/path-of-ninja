using System;
using UnityEngine;
using UnityEngine.Events;

namespace PathToNinja.Player
{
    public class SwipeControl : MonoBehaviour
    {
        [SerializeField] private DashToEvent _onDrag;
        [SerializeField] private float _minMagnitude;
        [SerializeField] private float _maxMagnitude;

        private Vector3 _start;
        private Vector3 _end;
        private Camera _camera;
        public bool IsDragged { get; private set; }
        public Vector3 Direction { get; private set; }

        private void Awake()
        {
            _camera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsDragged = true;
                _start = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Direction.magnitude > _minMagnitude)
                {
                    _onDrag.Invoke(Direction);
                }

                Debug.Log($"direction: {Direction} magnitude: {Direction.magnitude}");

                IsDragged = false;
            }

            if (IsDragged)
            {
                _end = _camera.ScreenToWorldPoint(Input.mousePosition);
            }

            Direction = _end - _start;
            Direction = Direction.normalized * Mathf.Min(Direction.magnitude, _maxMagnitude);
        }

        private void OnDrawGizmos()
        {
            if (IsDragged)
            {
                Gizmos.DrawLine(_start, _end);
            }
        }
    }

    [Serializable]
    public class DashToEvent : UnityEvent<Vector3>
    {
    }
}