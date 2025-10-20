using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData pd;
    private PlayerInputSystem inputSystem;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    //--------GROUND CHECK---------
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameObject sword;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = sword.GetComponent<Animator>();
    }

    void Update()
    {
        moveCharacter();
    }

    public void moveValueRead(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
        else if (ctx.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    private void moveCharacter()
    {
        Vector2 movement = new Vector2(moveInput.x * pd.PlayerMovement, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    public void jump(InputAction.CallbackContext ctx)
    {
        if (isGround())
        {
            Vector2 jumping = new Vector2(rb.linearVelocity.x, 1f * pd.PlayerJumpForce);
            rb.linearVelocity = jumping;
        }
    }

    public void attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            anim.SetTrigger("Slice");
        }
    }

    private bool isGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}
