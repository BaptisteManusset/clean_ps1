using System;
using UnityEngine;

namespace Technical.VarRef
{
    [CreateAssetMenu(fileName = "VarRef", menuName = "VarRef/VarRef", order = 0)]
    public abstract class VarRef<T> : ScriptableObject
    {
        private T m_value;

        public T Value
        {
            get => m_value;
            set
            {
                m_value = value;
                valueChanged?.Invoke(m_value);
            }
        }

        public event Action<T> valueChanged;


 
    }
}