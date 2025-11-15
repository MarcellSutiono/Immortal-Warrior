using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //--------PLAYER DATA---------
    [SerializeField] private PlayerData pd;

    //--------MOVEMENT---------
    private Vector2 moveInput;

    //--------CHARGE---------
    private bool isCharging = false;
    private float chargeTime = 0f;
    private float maxChargeTime = 3f;
    public GameObject[] chargeBars;

    //--------GAMEOBJECT ---------
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    //--------GROUND CHECK---------
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    //--------ANIMATOR---------
    private Animator anim;


    void Start()
    {
        pd.IsRolling = false;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("xVelocity", MathF.Abs(moveInput.x));
        moveCharacter();
        barHandler();
    }

    public void moveValueRead(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        if (moveInput.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            sr.flipX = false;
        }
    }

    private void moveCharacter()
    {
        Vector2 movement = new Vector2(moveInput.x * pd.PlayerMovement, rb.linearVelocity.y);
        rb.linearVelocity = movement;
    }

    private void barHandler()
    {
        barClear();

        if (chargeTime >= 0 && chargeTime < 0.5f)
        {
            activateBar(0);
        }
        
        if(chargeTime >= 0.5f && chargeTime < 0.75f)
        {
            activateBar(1);
            pd.PlayerPower = pd.PlayerRawPower + (pd.PlayerRawPower * 50 / 100);
        }
        
        if(chargeTime >= 0.75f && chargeTime < 1.25f)
        {
            activateBar(2);
            pd.PlayerPower = pd.PlayerRawPower + (pd.PlayerRawPower * 100 / 100);
        }
        
        if(chargeTime >= 1.25f && chargeTime < 1.75f)
        {
            activateBar(3);
            pd.PlayerPower = pd.PlayerRawPower + (pd.PlayerRawPower * 200 / 100);
        }
        
        if (chargeTime >= 1.75f && chargeTime < 2.75f)
        {
            activateBar(4);
            pd.PlayerPower = pd.PlayerRawPower + (pd.PlayerRawPower * 350 / 100);
        }
        
        if(chargeTime >= 2.75f)
        {
            activateBar(5);
            pd.PlayerPower = pd.PlayerRawPower + (pd.PlayerRawPower * 500 / 100);
        }

        if(!isCharging)
        {
            chargeTime -= Time.deltaTime * 0.65f;
        }

        if(chargeTime < 0f)
        {
            chargeTime = 0f;
        }

    }

    private void barClear()
    {
        foreach (GameObject bar in chargeBars)
        {
            bar.SetActive(false);
        }
    }

    private void activateBar(int level)
    {
        for(int i=0; i<=level; i++)
        {
            chargeBars[i].SetActive(true);
        }
    }

    public void jump(InputAction.CallbackContext ctx)
    {
        if (isGround())
        {
            Vector2 jumping = new Vector2(rb.linearVelocity.x, 1f * pd.PlayerJumpForce);
            rb.linearVelocity = jumping;
        }
    }

    public void charge(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            anim.SetBool("isCharging", true);
            pd.PlayerMovement = pd.PlayerMovementCharging;
            isCharging = true;
            StartCoroutine(chargingAttack());
        }
        else if(ctx.canceled)
        {
            anim.SetBool("isCharging", false);
            pd.PlayerMovement = pd.PlayerDefaultMovement;
            isCharging = false;
        }
    }

    public void attackRight(InputAction.CallbackContext ctx)
    {
        sr.flipX = false;

        if (ctx.started)
        {
            anim.SetTrigger("Attack");
            pd.attackRight = true;
            StartCoroutine(attackDuration());
        }
    }

    public void attackLeft(InputAction.CallbackContext ctx)
    {
        sr.flipX = true;

        if (ctx.started)
        {
            anim.SetTrigger("Attack");
            pd.attackLeft = true;
            StartCoroutine(attackDuration());
        }
    }

    public void roll(InputAction.CallbackContext ctx)
    {
        if(!pd.IsRolling)
        {
            anim.SetTrigger("Roll");
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        pd.IsRolling = true;
        gameObject.layer = LayerMask.NameToLayer("PlayerRolling");

        Vector2 dir;
        if (moveInput != Vector2.zero)
        {
            dir = new Vector2(moveInput.x, 0);
        }
        else
        {
            dir = new Vector2(1, 0);
        }

        float startTime = Time.time;
        while (Time.time < startTime + pd.PlayerRollDuration)
        {
            rb.linearVelocity = dir * pd.PlayerRollSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("PlayerDefault");

        yield return new WaitForSeconds(pd.PlayerRollCooldown);
        pd.IsRolling = false;
    }

    private IEnumerator attackDuration()
    {
        isCharging = false;
        chargeTime = 0f;
        barClear();
        yield return new WaitForSeconds(1f);
        pd.attackRight = false;
        pd.attackLeft = false;
        pd.PlayerPower = pd.PlayerRawPower;
    }

    private IEnumerator chargingAttack()
    {
        while(isCharging)
        {
            chargeTime += Time.deltaTime;
            if(chargeTime >= maxChargeTime)
            {
                chargeTime = maxChargeTime;
            }
            yield return null;
        }
    }

    private bool isGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }
}
