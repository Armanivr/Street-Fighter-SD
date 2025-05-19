using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class CustomInputModule : MonoBehaviour
{
    public static CustomInputModule instance;

    public Rigidbody2D player1; // Assign your Capsule's Rigidbody2D here.
    public Rigidbody2D player2; // Assign your Circle's Rigidbody2D here.
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [SerializeField] private Animator animator1; // Animator for player 1
    [SerializeField] private Animator animator2; // Animator for player 2

    private PlayerInputActions playerInputActions;
    private Gamepad gamePad;
    private Coroutine killRumbleTimer;
    public PlayerInput playerInput;
    private string currentControlScheme;
    private string currentActionMap = "Player";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private async void Update()
    {
        var gamepads = Gamepad.all;

        // Check for the first gamepad (Player 1)
        if (gamepads.Count >= 1 && player1 != null)
        {
            Vector2 inputP1 = gamepads[0].leftStick.ReadValue();
            player1.linearVelocity = new Vector2(inputP1.x * moveSpeed, player1.linearVelocity.y);

            if (gamepads[0].buttonSouth.wasPressedThisFrame && Mathf.Abs(player1.linearVelocity.y) < 0.01f)
            {
                player1.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (gamepads[0].buttonEast.wasPressedThisFrame)
            {
                if (animator1 != null)
                {
                    animator1.SetTrigger("Punch");
                }
                else
                {
                    Debug.LogWarning("Animator for player 1 is not assigned.");
                }
            }

            if (gamepads[0].buttonSouth.wasPressedThisFrame)
            {
                if (animator1 != null)
                {
                    animator1.SetTrigger("Jump");
                }
                else
                {
                    Debug.LogWarning("Animator for player 1 is not assigned.");
                }
            }
        }

        // Check for the second gamepad (Player 2)
        if (gamepads.Count >= 2 && player2 != null)
        {
            Vector2 inputP2 = gamepads[1].leftStick.ReadValue();
            player2.linearVelocity = new Vector2(inputP2.x * moveSpeed, player2.linearVelocity.y);

            _HasPressedJump = false;
            _HasPressedPunch = false;

            if (gamepads[1].buttonSouth.wasPressedThisFrame && Mathf.Abs(player2.linearVelocity.y) < 0.01f)
            {
                player2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (gamepads[1].buttonEast.wasPressedThisFrame && _HasPressedPunch == false)
            {
                if (animator2 != null)
                {
                    _HasPressedPunch = true
                    animator2.SetTrigger("Punch");
                    await Task.Delay(1000); 
                    _HasPressedPunch = false;

                }
                else
                {
                    Debug.LogWarning("Animator for player 2 is not assigned.");
                }
            }

            if (gamepads[1].buttonSouth.wasPressedThisFrame)
            {
                if (animator2 != null)
                {
                    animator2.SetTrigger("Jump");
                }
                else
                {
                    Debug.LogWarning("Animator for player 2 is not assigned.");
                }
            }
        }
    }

    /* ------------------------------------------------------------ Action Map: Player ------------------------------------------------------------ */

    public bool Jump()
    {
        float jumpFloat = playerInputActions.Player.Jump.ReadValue<float>();
        return jumpFloat >= 1;
    }

    public bool Punch()
    {
        float pushFloat = playerInputActions.Player.Attack.ReadValue<float>();
        return pushFloat >= 1;
    }

    public bool Kick()
    {
        float pushFloat = playerInputActions.Player.Attack.ReadValue<float>();
        return pushFloat >= 1;
    }

    private static bool LeftShoulderInUse = false;

    public bool LeftShoulder()
    {
        float leftBumperFloat = playerInputActions.Player.Block.ReadValue<float>();
        if (leftBumperFloat >= 1)
        {
            if (!LeftShoulderInUse)
            {
                LeftShoulderInUse = true;
                return true;
            }
        }
        else
        {
            LeftShoulderInUse = false;
        }
        return false;
    }

    private static bool RightShoulderInUse = false;

    public bool RightShoulder()
    {
        float rightBumperFloat = playerInputActions.Player.Crouch.ReadValue<float>();
        if (rightBumperFloat >= 1)
        {
            if (!RightShoulderInUse)
            {
                RightShoulderInUse = true;
                return true;
            }
        }
        else
        {
            RightShoulderInUse = false;
        }
        return false;
    }

    public static bool MenuInUse = false;

    public bool PlayerMenu()
    {
        float menuFloat = playerInputActions.Player.Menu.ReadValue<float>();
        if (menuFloat >= 1)
        {
            if (!MenuInUse)
            {
                MenuInUse = true;
                return true;
            }
        }
        else
        {
            MenuInUse = false;
        }
        return false;
    }

    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        if (currentControlScheme == "Player")
        {
            gamePad = Gamepad.current;
            if (gamePad != null)
            {
                gamePad.SetMotorSpeeds(lowFrequency, highFrequency);
                killRumbleTimer = StartCoroutine(KillRumble(duration, gamePad));
            }
        }
    }

    private IEnumerator KillRumble(float duration, Gamepad gamePad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gamePad.SetMotorSpeeds(0f, 0f);
    }

    /* ------------------------------------------------------------ General ------------------------------------------------------------ */

    public void EnAblePlayerActionMap()
    {
        currentActionMap = "Player";
        playerInputActions.Player.Enable();
    }
}
