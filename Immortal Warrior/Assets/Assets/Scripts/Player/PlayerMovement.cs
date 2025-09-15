using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData pd;
    private PlayerInputSystem inputSystem;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        inputSystem = new PlayerInputSystem();
        inputSystem.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputSystem.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputSystem.Player.Jump.performed += ctx =>jump();
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        Vector2 movement = new Vector2(moveInput.x * pd.PlayerMovement, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    private void jump()
    {
        if(isGround())
        {
            Vector2 jumping = new Vector2(rb.linearVelocity.x, 1f * pd.PlayerJumpForce);
            rb.linearVelocity = jumping;
        }
    }

    private bool isGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}
