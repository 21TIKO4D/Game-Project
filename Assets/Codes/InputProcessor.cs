using UnityEngine;
using UnityEngine.InputSystem;

namespace PeliExample
{
    public class InputProcessor : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset inputControls;

        [SerializeField]
        private string actionMapName;

        private InputAction[] inputActions;

        private void Awake()
        {
            InputActionMap actionMap = inputControls.FindActionMap(actionMapName);
            inputActions = actionMap.actions.ToArray();

        }

        private void OnEnable()
        {
            foreach (var action in inputActions)
            {
                action.Enable();
                action.performed += ProcessInput;
                action.canceled += ProcessInput;
            }
        }

        private void OnDisable()
        {
            foreach (var action in inputActions)
            {
                action.Enable();
                action.performed -= ProcessInput;
                action.canceled -= ProcessInput;
            }
        }

        private void ProcessInput(InputAction.CallbackContext callbackContext) {
            SendMessage($"On{callbackContext.action.name}", callbackContext, SendMessageOptions.DontRequireReceiver);
        }
    }
}
