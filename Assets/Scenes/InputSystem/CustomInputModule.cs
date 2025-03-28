using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputModule : MonoBehaviour
{

    private Rigidbody2D player1;
    private Rigidbody2D player2;
    private PlayerInput playerInput;



    private void Awake()
    {
        player1= GetComponent<Rigidbody2D>();
        player2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        PlayerInputActions playerInputactions = new PlayerInputActions();
        //playerInputActions.Player1.Enable();   ------------------------------  ( Deze werkt momenteel niet voor een reden die ik niet doorheb) 



    }
    public void Jump()
    {
        Debug.Log("Jump!");
        player1.AddForce(Vector2.up * 1, ForceMode2D.Impulse);
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
