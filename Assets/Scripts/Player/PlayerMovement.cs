using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    public static float mSpeed = 7f;
    public float acc = 12f;
    public float deAcc = 10f;
    public float airAcc = 8f;
    public float gravity = -40f;
    public float maxFallSpeed = -20f;
    private float hInput;
    private Vector2 velocity;
    private float verticalVelocity;
    private bool isFacingR = true;

    [Header("Jump")]
    //Jump
    public float jumpForce = 18f;
    public float jumpHoldTime = 0.3f;
    public float jumpHoldForce = 4f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;
    private bool _isJumping = false;
    private bool _canDoubleJump = true;
    private float _jumpTimeCounter;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    //Dash
    [Header("Dash")]
    public float dashSpeed = 25f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.5f;
    private bool _canDash = true;
    private bool _isDashing;

    //Collision
    [Header("Collision")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public Transform wallCheck;
    public float checkRad = 0.2f;
    public float checkOffset = 0.5f;
    private bool _isGrounded = false;
    private bool _isTouchingCeiling = false;
    private bool isTouchingWall;


    //HUD
    [Header("Hud")]
    [SerializeField] private Slider slider;

    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRad, groundLayer);
        _isTouchingCeiling = Physics2D.OverlapCircle(ceilingCheck.position, checkRad, groundLayer);
        
        Movement();
        Jump();
        Dash();
    }

    private void Movement()
    {
        Vector2 checkPosition = isFacingR 
            ? wallCheck.position + Vector3.right * checkOffset 
            : wallCheck.position + Vector3.left * checkOffset;
        
        bool isTouchingWall = Physics2D.OverlapCircle(checkPosition, checkRad, groundLayer);
        
        if (isTouchingWall && Mathf.Sign(hInput) == (isFacingR ? 1 : -1))
        {
            velocity.x = 0;
        }
        else
        {
            float targetSpeed = hInput * mSpeed;
            var acceleration = _isGrounded ? acc : airAcc;
            velocity.x = Mathf.Lerp(velocity.x, targetSpeed, Time.deltaTime * acceleration);
        }
        
        if (!_isDashing)
        {
            verticalVelocity += gravity * Time.deltaTime;
            verticalVelocity = Mathf.Clamp(verticalVelocity, maxFallSpeed, Mathf.Infinity);
        }

        if (_isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
        
        transform.Translate(new Vector2(velocity.x, verticalVelocity) * Time.deltaTime);
        
        if ((hInput > 0 && !isFacingR) || (hInput < 0 && isFacingR))
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (_isGrounded)
            _coyoteTimeCounter = coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;

        // Buffer de salto
        if (Input.GetButtonDown("Jump"))
            _jumpBufferCounter = jumpBufferTime;
        else
            _jumpBufferCounter -= Time.deltaTime;

        // Salto
        if ((_jumpBufferCounter > 0 && _coyoteTimeCounter > 0) && !_isJumping)
        {
            _isJumping = true;
            verticalVelocity = jumpForce;
            _jumpTimeCounter = jumpHoldTime;
            _jumpBufferCounter = 0;
        }
        
        // Saltar más alto si se mantiene el botón
        if (Input.GetButton("Jump") && _isJumping && _jumpTimeCounter > 0 && !_isTouchingCeiling)
        {
            verticalVelocity += jumpHoldForce * Time.deltaTime;
            _jumpTimeCounter -= Time.deltaTime;
        }

        // Terminar el salto
        if (Input.GetButtonUp("Jump") || _isTouchingCeiling)
        {
            _isJumping = false;
            if (verticalVelocity > 0)
                verticalVelocity *= 0.5f;
        }
        
        // Doble salto
        if (!_isGrounded && Input.GetButtonDown("Jump") && _canDoubleJump)
        {
            verticalVelocity = jumpForce;
            _canDoubleJump = false;
        }
        
        if (_isGrounded && verticalVelocity <= 0)
        {
            _isJumping = false;
            _canDoubleJump = true;
        }
    }
    
    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && _canDash)
        {
            StartCoroutine(DashCo());
        }
    }

    private IEnumerator DashCo()
    {
        var originalGravity = gravity;
        gravity = 0; 
        _isDashing = true;
        float dashDirection = isFacingR ? 1 : -1;
        velocity.x = dashDirection * dashSpeed;
        _canDash = false;

        yield return new WaitForSeconds(dashTime);

        gravity = originalGravity;
        _isDashing = false;

        // Cooldown del dash
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }
    
    private void Flip()
    {
        isFacingR = !isFacingR;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRad);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ceilingCheck.position, checkRad);
        Gizmos.color = Color.red;
        Vector2 checkPosition = isFacingR 
            ? wallCheck.position + Vector3.right * checkOffset 
            : wallCheck.position + Vector3.left * checkOffset;

        Gizmos.DrawWireSphere(checkPosition, checkRad);
    }
}