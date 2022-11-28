using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float windJumpPower;
    [SerializeField] float playerDashSpeed;
    [SerializeField] float playerCurrentSpeed;
    [SerializeField] GameObject AttackZoneParent;
    [SerializeField] float wallJumpX;
    [SerializeField] float wallJumpY;
    public bool IsMove { get; set; }
    public float DirX { get; set; }
    public Rigidbody2D playerRigid { get; set; }
    private PlayerDetect playerDetect;
    private SpriteRenderer spriteRenderer;
    public bool isWallJumping { get; set; }
    float h;
    public float LastH;

    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip walkClip;

    /*SoundManager bgm = new SoundManager();
    AudioSource bgmSource;*/
    public AudioClip bgmClip;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerDetect = GetComponent<PlayerDetect>();
        playerRigid = GetComponent<Rigidbody2D>();
        playerCurrentSpeed = playerSpeed;

        SoundManager.instance.Bgm(bgmClip);
    }
    void Update()
    {
        if (h != 0) LastH = h;
        if (playerDetect.IsWall && playerRigid.velocity.y > 0 && !isWallJumping)
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, 0);
        }
        WallJump();
        IsWall();
        if (!isWallJumping)
        {
            Move();
        }
        Jump();
    }
    private void Move()
    {
        h = Input.GetAxisRaw("Horizontal");

        if (h != 0 && playerSource.isPlaying == false) SoundManager.instance.PlayOneShot(playerSource, walkClip);
        DirX = h;
        //playerRigid.velocity = new Vector2(h * playerCurrentSpeed, playerRigid.velocity.y);
        if (!playerDetect.detectLeft && h == -1)
        {
            playerRigid.velocity = new Vector2(h * playerCurrentSpeed, playerRigid.velocity.y);
            AttackZoneParent.transform.localScale = new Vector3(h * -1, 1, 1);
        }
        else if (!playerDetect.detectRight && h == 1)
        {
            playerRigid.velocity = new Vector2(h * playerCurrentSpeed, playerRigid.velocity.y);
            AttackZoneParent.transform.localScale = new Vector3(h * -1, 1, 1);
        }
        else
        {
            playerRigid.velocity = new Vector2(0, playerRigid.velocity.y);
        }

    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerDetect.IsGround == true && !playerDetect.IsGroundWall)
        {
            playerRigid.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }
    public void WindJump()
    {
        playerRigid.AddForce(new Vector2(0, windJumpPower), ForceMode2D.Impulse);
        Debug.Log("1");
    }
    public void WallJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (playerDetect.IsGroundWall || playerDetect))
        {
            if (playerDetect.detectLeft)
            {
                spriteRenderer.flipX = false;
                StartCoroutine("WallJumpingWaiter");
                playerRigid.AddForce(new Vector2(wallJumpX, wallJumpY), ForceMode2D.Impulse);
            }
            else if (playerDetect.detectRight)
            {
                spriteRenderer.flipX = true;
                StartCoroutine("WallJumpingWaiter");
                playerRigid.AddForce(new Vector2(-wallJumpX, wallJumpY), ForceMode2D.Impulse);
            }
        }
    }
    private void IsWall()
    {
        if (playerDetect.IsAir && (playerDetect.detectRight || playerDetect.detectLeft))
        {
            playerRigid.gravityScale = 0.2f;
        }
        else
        {
            playerRigid.gravityScale = 1f;
        }
    }
    IEnumerator WallJumpingWaiter()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(0.4f);
        isWallJumping = false;
    }
}