using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManagerScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    [SerializeField] GameObject player;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions.FindAction("TouchPress");
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        Debug.Log(value);

        Vector3 position =  Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.y = player.transform.position.y;
        player.transform.position = position;
    }

}
