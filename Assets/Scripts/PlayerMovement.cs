using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    public float mSpeed = 7f;
    public float acc = 12f;
    public float deAcc = 10f;
    public float airAcc = 8f;
    public float gravity = -40f;
    public float maxFallSpeed = -20f;
    private float hInput;
    private Vector2 velocity;
    private float verticalVelocity;
    private bool isFacingR = true;
    
    //Jump
    public float jumpForce = 18f;
    public float jumpHoldTime = 0.3f;
    public float jumpHoldForce = 4f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;
    
    //Dash
    public float dashSpeed = 25f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.5f;
    private bool _canDash = true;
    private bool _isDashing;
    
    //Collision
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform wallCheck;
    public float checkRad = 0.2f;
    private bool _isGrounded = false;
    private bool _isJumping = false;
    private bool _canDoubleJump = true;
    private float _jumpTimeCounter;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    

    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRad, groundLayer);
        
        Movement();
        Jump();
        Dash();
    }

    private void Movement()
    {
        var speed = hInput * mSpeed;
        var acceleration = _isGrounded ? acc : airAcc;
        
        velocity.x  = Mathf.Lerp(velocity.x, speed, Time.deltaTime * acceleration);
        
        verticalVelocity += gravity * Time.deltaTime;
        verticalVelocity = Mathf.Clamp(verticalVelocity, maxFallSpeed, Mathf.Infinity);
        
        if (_isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
        
        transform.Translate(new Vector2(velocity.x, verticalVelocity) * Time.deltaTime);

        switch (hInput)
        {
            case > 0 when !isFacingR:
            case < 0 when isFacingR:
                Flip();
                break;
        };
    }

    private void Jump()
    {
        if (_isGrounded)
            _coyoteTimeCounter = coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;

        // Buffer de salto: Permitir entrada de salto antes de aterrizar
        if (Input.GetButtonDown("Jump"))
            _jumpBufferCounter = jumpBufferTime;
        else
            _jumpBufferCounter -= Time.deltaTime;

        // Realizar el salto
        if ((_jumpBufferCounter > 0 && _coyoteTimeCounter > 0) && !_isJumping)
        {
            _isJumping = true;
            verticalVelocity = jumpForce;
            _jumpTimeCounter = jumpHoldTime;
            _jumpBufferCounter = 0;
        }
        
        // Saltar más alto si se mantiene el botón
        if (Input.GetButton("Jump") && _isJumping && _jumpTimeCounter > 0)
        {
            verticalVelocity += jumpHoldForce * Time.deltaTime;
            _jumpTimeCounter -= Time.deltaTime;
        }

        // Terminar el salto si se suelta el botón
        if (Input.GetButtonUp("Jump"))
        {
            _isJumping = false;
            if (verticalVelocity > 0)
                verticalVelocity *= 0.5f; // Reducir la velocidad de ascenso
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
        gravity = 0; // Desactivar gravedad durante el dash
        _isDashing = true;
        float dashDirection = isFacingR ? 1 : -1;
        velocity.x = dashDirection * dashSpeed;

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
    }
}