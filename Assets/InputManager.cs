using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float Turn = 0;
    public bool forward = false;
    public bool backward = false;
    public bool lastChek = false;
    public bool Reset = false;

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Turn = context.ReadValue<float>();
            
        }
        else if (context.canceled)
        {
            Turn = context.ReadValue<float>();
        }
    }

    public void InputAccel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            forward = true;
            
        }
        else if (context.canceled)
        {
            forward = false;
        }
    }

    public void InputDecel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            backward = true;
            
        }
        else if (context.canceled)
        {
            backward = false;
        }
    }

    public void InputLastCheck(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastChek = true;

        }
        else if (context.canceled)
        {
            lastChek = false;
        }
    }

    public void InputReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Reset = true;

        }
        else if (context.canceled)
        {
            Reset = false;
        }
    }
}
