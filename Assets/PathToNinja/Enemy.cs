using UnityEngine;

namespace PathToNinja
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Collider2D _collier;
        [SerializeField] private Animator _animator;

        public bool IsDead { get; private set; }

        public void Die()
        {
            _animator.SetBool("die", true);
            _collier.enabled = false;
            IsDead = true;
        }
    }
}