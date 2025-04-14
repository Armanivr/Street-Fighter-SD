using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputModule : MonoBehaviour
{
    public Rigidbody2D player1; // Assign your Capsule's Rigidbody2D here.
    public Rigidbody2D player2; // Assign your Circle's Rigidbody2D here.
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private void Update()
    {
        var gamepads = Gamepad.all;

        // Check for the first gamepad (Player 1)
        if (gamepads.Count >= 1 && player1 != null)
        {
            // Read movement from gamepad 1's left stick
            Vector2 inputP1 = gamepads[0].leftStick.ReadValue();
            player1.linearVelocity = new Vector2(inputP1.x * moveSpeed, player1.linearVelocity.y);

            // Check button press for jump
            if (gamepads[0].buttonSouth.wasPressedThisFrame && Mathf.Abs(player1.linearVelocity.y) < 0.01f)
            {
                player1.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        // Check for the second gamepad (Player 2)
        if (gamepads.Count >= 2 && player2 != null)
        {
            // Read movement from gamepad 2's left stick
            Vector2 inputP2 = gamepads[1].leftStick.ReadValue();
            player2.linearVelocity = new Vector2(inputP2.x * moveSpeed, player2.linearVelocity.y);

            // Check button press for jump
            if (gamepads[1].buttonSouth.wasPressedThisFrame && Mathf.Abs(player2.linearVelocity.y) < 0.01f)
            {
                player2.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
