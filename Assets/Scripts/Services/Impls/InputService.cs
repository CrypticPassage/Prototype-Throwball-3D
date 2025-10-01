using UnityEngine;

namespace Services.Impls
{
    /// <summary>
    /// Даний сервіс відповідає за інпут гравця.
    /// Задля простоти та економії часу було залишено даний сервіс і для Android білду (гра корректно працює і з даними методами для миші).
    /// Для потенційного "непрототипу" краще дописати сервіс по типу "MobileInputService" з використанням Input.Touch.
    /// </summary>
    public class InputService : MonoBehaviour, IInputService
    {
        public bool IsClickDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool IsClickUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public bool IsClickHeld()
        {
            return Input.GetMouseButton(0);
        }
    }
}