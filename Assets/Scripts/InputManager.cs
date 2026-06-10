using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions playerInput;

    private void Awake()
    {
        playerInput = new InputSystem_Actions();
        
    }

    private void OnEnable()
    {
        //playerInput.Player.Click.started += soEventInputs.InvokeClick;
        playerInput.Player.Click.Enable();
    }

    private void OnDisable()
    {
        //playerInput.Player.Click.started -= soEventInputs.InvokeClick;
        playerInput.Player.Click.Disable(); 
    }

}
