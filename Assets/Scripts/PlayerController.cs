/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 12/8/2021
 * Description: This should be added to the player in a simple 2D platformer 
 * *************************************/
//Updated By Kirill Ustinov (2/3/2022)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //speed and movement variables
    public float speed;
    public float airSpeed;
    private float moveInputH;
    //grab this to adjust physics
    private Rigidbody2D myRb;
    private Collider2D myCollida;
    private bool controlson;

    //used for checking what direction to be flipped
    private bool facingRight = true;

    //things for ground checking
    private bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump things
    public int extraJumps = 1;
    private int jumps;
    public float jumpForce;
    private bool jumpPressed = true;

    private float jumpTimer = 0;
    public float jumpTime = 0.2f;

    public float gravityScale = 5;

    public float groundDrag = 5;
    public float airDrag = 1;

    private AudioSource myAud;
    public AudioClip jumpNoise;

    //ladder things
    private bool isClimbing;
    public LayerMask whatIsLadder;
    public float ladderDist;
    private float moveInputV;
    public float climbSpeed;

    public float[,] posOfD;
    private int counter;
    private AudioSource source;
    public AudioClip PickUpNoise;
    public AudioClip landing;
    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    //animation
    private Animator myAnim;
    

    // Start is called before the first frame update
    void Start()
    {
        controlson = true;
        myRb = GetComponent<Rigidbody2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        myCollida = GetComponent<Collider2D>();
        jumps = extraJumps;
        counter = 0;
        posOfD = new float[3, 2] {
            {1, -1}, 
            {94, -3},
            {196, -3}
        };

        RespawnPoint = transform.position;
    }

    //Update is called once per frame
    private void Update()
    {
        if (controlson)
        {
            moveInputH = Input.GetAxisRaw("Horizontal");
            if (isGrounded == true)
            {
                jumps = extraJumps;
            }
            //check if jump can be triggered
            if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && isGrounded == true && isClimbing == false)
            {
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
            }
            else if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && jumps > 0 && isClimbing == false)
            {
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
                jumps--;
            }
            else if (Input.GetAxisRaw("Jump") == 0)
            {
                jumpPressed = false;
                jumpTimer = 0;
            }
            else if (jumpPressed == true && jumpTimer < jumpTime)
            {
                jumpTimer += Time.deltaTime;
                myRb.drag = airDrag;
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                jumpPressed = true;
            }
        }
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        if (controlson)
        {
            //check for ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            //set animators on ground
            myAnim.SetBool("OnGround", isGrounded);

            //ladder things

            moveInputV = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
            //check for the ladder if around the player
            RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, ladderDist, whatIsLadder);

            //if ladder was found see if we are climbing, stop falling
            if (hitInfo.collider != null)
            {
                myRb.gravityScale = 0;
                isClimbing = true;
                if (moveInputV > 0)
                {
                    myRb.AddForce(new Vector2(0, climbSpeed));
                }
                else if (moveInputV < 0)
                {
                    myRb.AddForce(new Vector2(0, -climbSpeed));
                }
                else
                {
                    myRb.velocity = new Vector2(myRb.velocity.x, 0);
                }
            }
            else
            {
                myRb.gravityScale = gravityScale;
                isClimbing = false;
            }

            //horizontal movement
            moveInputH = Input.GetAxisRaw("Horizontal");
            //animator settings
            if (moveInputH == 0)
            {
                myAnim.SetBool("Moving", false);
            }
            else
            {
                myAnim.SetBool("Moving", true);
            }

            if (isGrounded && !jumpPressed || isClimbing)
            {
                myRb.drag = groundDrag;
                myRb.AddForce(new Vector2(moveInputH * speed, 0));
            }
            else
            {
                myRb.drag = airDrag;
                myRb.AddForce(new Vector2(moveInputH * airSpeed, 0));
            }
            //check if we need to flip the player direction
            if (facingRight == false && moveInputH > 0)
                Flip();
            else if (facingRight == true && moveInputH < 0)
            {
                Flip();
            }
        }
    }
    //flip the player so sprite faces the other way
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && controlson)
        {
            StartCoroutine(OnDeath());
        }
        else if (collision.gameObject.CompareTag("dimension")) {
            //Debug.Log("moving player to " + posOfD[counter, 0] + " " + posOfD[counter, 1]);
            counter++;
            transform.position = new Vector2(posOfD[counter, 0], posOfD[counter, 1]);
            //myRb.MovePosition(new Vector2(posOfD[counter, 0], posOfD[counter, 1]));
            //respawn point setter on new level
            RespawnPoint = transform.position;
            
        }
    }

    private IEnumerator OnDeath() {
        myRb.velocity = Vector2.zero;
        myAnim.Play("death");
        
        myRb.gravityScale = 0;
        myCollida.enabled = false;
        controlson = false;

        source.PlayOneShot(PickUpNoise);

        yield return new WaitForSeconds(0.8f);

        myRb.gravityScale = gravityScale;
        myCollida.enabled = true;
        controlson = true;

        transform.position = RespawnPoint;
        GameManager.score++;
        myAnim.Play("PlayerIdle");
    }

    public void playFootStep() {
        source.PlayOneShot(source.clip);
    }

    public void Restart() {
        //GameManager.score = -1;
        
        counter = 0;
        RespawnPoint = new Vector2(posOfD[0, 0], posOfD[0, 1]);
        StartCoroutine(OnDeath());
        GameManager.ResetGame();
    }
}
