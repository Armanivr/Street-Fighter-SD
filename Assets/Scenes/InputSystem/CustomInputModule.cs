using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputModule : MonoBehaviour
{

    public Rigidbody2D player1;
    public Rigidbody2D player2;
    private PlayerInput playerInput;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player1.Enable();
        playerInputActions.Player2.Enable();

        playerInputActions.Player1.Jump.performed += Jump;
        playerInputActions.Player2.Jump.performed += Jump;

        playerInputActions.Player1.Movement.performed += Movement_performed;
        playerInputActions.Player2.Movement.performed += Movement_performed;

    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        float speed = 5f;

        if (player1 != null)
            player1.linearVelocity = new Vector2(inputVector.x * speed, player1.linearVelocity.y);

        if (player2 != null)
            player2.linearVelocity = new Vector2(inputVector.x * speed, player2.linearVelocity.y);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump!");
        if (player1 != null)
            player1.AddForce(Vector2.up * 1, ForceMode2D.Impulse);

        if (player2 != null)
            player2.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }

    //public Vector2 LeftAnalog()
    //{
    //    Vector2 inputDir = PlayerInputSystem.Player1.Movement.ReadValue<Vector2>();
    //    //Debug.Log(inputDir);
    //    return inputDir;
    //}


    //public void Update()
    //{
    //    if(CustomInputModule.instance.Jump())
    //    {
    //        Debug.Log("Jump");
    //    }
    //}
}
