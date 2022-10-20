using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{  public class VacuumInput : MonoBehaviour
    {
        bool _mouseOneHeld;
        bool _rightMousePressed;
        bool _rightMouseReleased;
        float _mouseTwoValue;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnSuck(InputValue value)
        {
            MouseOneInput(value.isPressed);
        }
        public void OnFire(InputValue value)
        {
            MouseTwoInput(value.Get<float>());
        }
#endif
        public void MouseOneInput(bool isHolding)
        {
            _mouseOneHeld = isHolding;
            if (isHolding)
            {
                Debug.Log("Holding mouse one");
            }
        }
        public void MouseTwoInput(float pressValue)
        {
            bool isPressed = true;
            Debug.Log("Mouse two held = " + pressValue);
            if (isPressed && pressValue < 1)
            {
                Debug.Log("Mouse2 released");
            }
        }
        public bool MouseOneHeld => _mouseOneHeld;
    }
}
