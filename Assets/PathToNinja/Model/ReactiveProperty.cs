using System;
using UnityEngine;

namespace PathToNinja.Model
{
    [Serializable]
    public class ReactiveProperty<TType>
    {
        [SerializeField] private TType _value;

        public TType Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public Action<TType> OnChanged;

        public void CleanEvent()
        {
            OnChanged = null;
        }
    }

    [Serializable]
    public class IntProperty : ReactiveProperty<int>
    {
    }
}