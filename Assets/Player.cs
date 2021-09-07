using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    LayerMask layerMask;
    ////////////////////// INITIATIVE \\\\\\\\\\\\\\\\\\\\\\\\\
    void Start()
    {
        anime = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        base_gravityscale = rb.gravityScale;
        layerMask = LayerMask.GetMask("Ground");
    }
    void Update()
    {
        Debug.Log(isGrounded);
        Walking();
        Jumping();
        if (isGrounded && canPlay) 
        {
            //play land sound
            canPlay = false;
        }
        if(walking)
        {
            //play walk sound
        }
    }

    ////////////////////// MOVEMENT \\\\\\\\\\\\\\\\\\\\\\\\\

    Rigidbody2D rb;
    [SerializeField] int acceleration, jumpForce, friction;
    [SerializeField] float speed;
    private float base_gravityscale, jumpTimeCounter;
    private bool isGrounded, isJumping;
    public float jumpTime, checkRadius;
    public Transform feetPos;
    bool canPlay, walking;
    Animator anime;

    #region Jumping
    void Jumping()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, layerMask);

        if(isGrounded)
        {
            anime.SetBool("Grounded", false);
        }
        else
        {
            canPlay = true;
            anime.SetBool("Grounded", true);
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump();
        }
        if (Input.GetKey(KeyCode.W) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            isJumping = false;
        }
    }

    void Jump() =>
        rb.velocity = new Vector2(rb.velocity.x ,jumpForce);

    #endregion
    #region Walking
    void Walking()
    {
        float horizontal = Input.GetAxis("Horizontal");
        anime.SetFloat("Speed", Mathf.Abs(horizontal));
        if (horizontal != 0)
        {
            if (isGrounded) walking = true; else walking = false;

            rb.gravityScale = base_gravityscale;

            if (horizontal > 0)
            {
                if (rb.velocity.x < speed)
                 rb.velocity += new Vector2(acceleration * Time.deltaTime, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                if(rb.velocity.x > -speed)
                    rb.velocity -= new Vector2(acceleration * Time.deltaTime, 0);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            walking = false;

            if (rb.velocity.x > 0)
            {
                rb.velocity -= new Vector2(friction, 0) * Time.deltaTime;
            }
            else if (rb.velocity.x < 0)
            {
                rb.velocity += new Vector2(friction, 0) * Time.deltaTime;
            }
            if (isGrounded)
            {
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = base_gravityscale;
            }
        }
    }
    #endregion
}
