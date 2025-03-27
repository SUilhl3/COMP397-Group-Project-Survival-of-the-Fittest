using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Platformer397
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<bool> Interact = delegate { };
        public event UnityAction<bool> Attack = delegate { };
        public event UnityAction<bool> Reload = delegate { };
        public event UnityAction<bool> WeaponOne = delegate { };
        public event UnityAction<bool> WeaponTwo = delegate { };
        public event UnityAction<bool> Grenade = delegate { };
        public event UnityAction<bool> GrenadeRelease = delegate { };
        public event UnityAction<bool> Tactical = delegate { };
        public event UnityAction<bool> TacticalRelease = delegate { };

        InputSystem_Actions input;
        private void OnEnable()
        {
            if(input == null)
            {
                input = new InputSystem_Actions();
                input.Player.SetCallbacks(this);
            }
        }

        public void EnablePlayerActions()
        {
            input.Enable();
        }




        public void OnMove(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                case InputActionPhase.Canceled:
                    Move?.Invoke(context.ReadValue<Vector2>());
                    break;
                default:
                    break;
            }
            Move?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnLook(InputAction.CallbackContext context)
        {

        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack?.Invoke(true);
            }
            else if (context.canceled)
            {
                Attack?.Invoke(false);
            }
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Reload?.Invoke(true);
            }
            else if (context.canceled)
            {
                Reload?.Invoke(false);
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Interact?.Invoke(true);
            }
            else if (context.canceled)
            {
                Interact?.Invoke(false);
            }
        }
        public void OnWeaponSlot1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                WeaponOne?.Invoke(true);
            }
            else if (context.canceled)
            {
                WeaponOne?.Invoke(false);
            }
        }

        public void OnWeaponSlot2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                WeaponTwo?.Invoke(true);
            }
            else if (context.canceled)
            {
                WeaponTwo?.Invoke(false);
            }
        }

        public void OnGrenade(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Grenade?.Invoke(true);
                GrenadeRelease?.Invoke(false);
            }
            else if (context.canceled)
            {
                Grenade?.Invoke(false);
                GrenadeRelease?.Invoke(true);
            }
        }

        public void OnTactical(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Tactical?.Invoke(true);
                TacticalRelease?.Invoke(false);
            }
            else if (context.canceled)
            {
                Tactical?.Invoke(false);
                TacticalRelease?.Invoke(true);
            }
        }
        public void OnCrouch(InputAction.CallbackContext context)
        {

        }
        public void OnJump(InputAction.CallbackContext context)
        {

        }
        public void OnPrevious(InputAction.CallbackContext context)
        {

        }
        public void OnNext(InputAction.CallbackContext context)
        {

        }
        public void OnSprint(InputAction.CallbackContext context)
        {

        }
        
    }
}
