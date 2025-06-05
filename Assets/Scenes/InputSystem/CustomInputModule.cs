using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System;

public class CustomInputModule : MonoBehaviour
{
    public static CustomInputModule instance;

    public Rigidbody2D player1;
    public Rigidbody2D player2; 
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [SerializeField] private Animator animator1; // Animator for player 1
    [SerializeField] private Animator animator2; // Animator for player 2

    private PlayerInputActions playerInputActions;
    private Gamepad gamePad;
    private Coroutine killRumbleTimer;
    public PlayerInput playerInput;
    private string currentControlScheme;
    private string currentActionMap;

    public Transform attackPointP1;
    public Transform attackPointP2;
    public float attackRange = 0.5f;
    public LayerMask player2Layer;
    public LayerMask player1Layer;

    public int attackDamage = 5;

    private bool _HasPressedJumpP1 = false;
    private bool _HasPressedPunchP1 = false;
    private bool _HasPressedJumpP2 = false;
    private bool _HasPressedPunchP2 = false;


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

            // Walk animation for player 1
            if (animator1 != null && !_HasPressedPunchP1 && !_HasPressedJumpP1)
            {
                bool isWalking = Mathf.Abs(inputP1.x) > 0.1f;
                animator1.SetBool("Walk", isWalking);
            }

            if (gamepads[0].buttonEast.wasPressedThisFrame && !_HasPressedPunchP1 && !_HasPressedJumpP1)
            {
                if (animator1 != null)
                {
                    _HasPressedPunchP1 = true;
                    animator1.SetTrigger("Punch");
                    animator1.SetBool("Walk", false);
                    await Task.Delay(500);
                    _HasPressedPunchP1 = false;

                        Debug.Log("Attack");
                    Attack();
                }
                else
                {
                    Debug.LogWarning("Animator for player 1 is not assigned.");
                }
                void Attack()
                {
                    // Detecteer vijanden binnen het bereik van de aanval
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointP1.position, attackRange, player2Layer);

                    // Raak ze
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                    }

                }


            }

            if (gamepads[0].buttonSouth.wasPressedThisFrame && Mathf.Abs(player1.linearVelocity.y) < 0.01f && !_HasPressedJumpP1 && !_HasPressedPunchP1)
            {
                if (animator1 != null)
                {
                    _HasPressedJumpP1 = true;
                    player1.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    animator1.SetTrigger("Jump");
                    animator1.SetBool("Walk", false);
                    await Task.Delay(1300);
                    _HasPressedJumpP1 = false;
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

            // Walk animation for player 2
            if (animator2 != null && !_HasPressedPunchP2 && !_HasPressedJumpP2)
            {
                bool isWalking = Mathf.Abs(inputP2.x) > 0.1f;
                animator2.SetBool("Walk", isWalking);
            }

            if (gamepads[1].buttonEast.wasPressedThisFrame && !_HasPressedPunchP2 && !_HasPressedJumpP2)
            {
                if (animator1 != null)
                {
                    _HasPressedPunchP1 = true;
                    animator2.SetTrigger("Punch");
                    animator2.SetBool("Walk", false);
                    await Task.Delay(500);
                    _HasPressedPunchP1 = false;

                    Debug.Log("Attack");
                    Attack();
                }
                else
                {
                    Debug.LogWarning("Animator for player 1 is not assigned.");
                }
                void Attack()
                {
                    // Detecteer vijanden binnen het bereik van de aanval
                    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPointP2.position, attackRange, player1Layer);

                    // Raak ze
                    foreach (Collider2D enemy in hitEnemies)
                    {
                        enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                    }

                }
            }

            if (gamepads[1].buttonSouth.wasPressedThisFrame && Mathf.Abs(player2.linearVelocity.y) < 0.01f && !_HasPressedJumpP2 && !_HasPressedPunchP2)
            {
                if (animator2 != null)
                {
                    _HasPressedJumpP2 = true;
                    player2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    animator2.SetTrigger("Jump");
                    animator2.SetBool("Walk", false); // Stop walk anim
                    await Task.Delay(1300);
                    _HasPressedJumpP2 = false;
                }
                else
                {
                    Debug.LogWarning("Animator for player 2 is not assigned.");
                }
            }
            //else
            //{
            //    Debug.LogWarning("Cooldown");
            //}
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

