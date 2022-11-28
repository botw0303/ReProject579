using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerSkill : MonoBehaviour
{
    [SerializeField] GameObject golemAttackPrefab;
    [SerializeField] GameObject windEffectPrefab;
    [SerializeField] GameObject fireAttackPrefab;
    [SerializeField] Transform windEffectTransform;
    [SerializeField] Transform golemAttackTransform;
    [SerializeField] BoxCollider2D[] attackZones;
    [SerializeField] LayerMask enemy;
    [SerializeField] LayerMask boss;
	[SerializeField] GameObject CameraManager;
	[SerializeField] GameObject[] HitEffect;
    [SerializeField] Slider HpSlider;
    [SerializeField] Slider MpSlider;
	CameraShakeEffect cameraShaker;
	private BoxCollider2D collider;
	private PlayerDetect playerDetect;
    private PlayerElements playerElements;
    private SpriteRenderer playerSP;
    private PlayerMove playerMove;
    private Animator playerAnimator;
    private bool canRolling = true;
    private bool isRolling;
    private bool isAttack = false;
    private bool attack1 = false;
    private bool attack2 = false;
    private bool attack3 = false;
    private bool attack4 = false;
    private bool canNextAttack = false;
    public bool canWindJump = false;
    public bool GetWind = false;
    public bool GetFire = false;
    public bool GetWater = false;
    public bool GetGround = false;
    public float MaxHp;
    public float CurrentHp;
    public float MaxMp;
    public float CurrentMp;
    public float FireCost;
    public float WindCost;
    public float GroundCost;
    public AudioClip clip;
    public AudioSource source;

    void Start()
    {
        playerElements = GetComponentInChildren<PlayerElements>();
        playerDetect = GetComponent<PlayerDetect>();
        playerSP = GetComponent<SpriteRenderer>();
        playerMove = GetComponent<PlayerMove>();
        playerAnimator = GetComponent<Animator>();
        cameraShaker = CameraManager.GetComponent<CameraShakeEffect>();
    }

    void Update()
    {
        if(CurrentHp <= 0)
        {
            SceneManager.LoadScene(0);
        }
        HpSlider.value = CurrentHp / MaxHp;
        MpSlider.value = CurrentMp / MaxMp;
        if(CurrentMp < MaxMp)
        {
            CurrentMp += Time.deltaTime * 2f;
        }
        if(CurrentHp < MaxHp)
        {
            CurrentHp += Time.deltaTime * 0.1f;
        }
        PlayerGolemAttack();
        PlayerAirAnimation();
        PlayerWindJump();
        PlayerAttack();
        PlayerRolling();
        PlayerFireAttack();
        if (playerDetect.IsGround)
        {
            playerAnimator.SetBool("Air", false);
        }
        if (playerDetect.IsAir)
        {
            playerAnimator.SetBool("Run", false);
        }
        else
        {
            playerAnimator.SetBool("Jump", false);
            if (playerMove.DirX == 1 || playerMove.DirX == -1)
            {
                playerAnimator.SetBool("Run", true);
            }

        }
        if (playerMove.DirX == 1 && !playerMove.isWallJumping)
        {
            playerSP.flipX = false;
        }
        else if (playerMove.DirX == -1 && !playerMove.isWallJumping)
        {
            playerSP.flipX = true;
        }
        else
        {
            playerAnimator.SetBool("Run", false);
        }
    }

    public void PlayerGetDamage(int EnemyAttackPos)
    {
        if (!isRolling)
        {
			if (EnemyAttackPos == 1) Instantiate(HitEffect[0], transform.position, Quaternion.Euler(0, 0, 0));
			else if (EnemyAttackPos == -1) Instantiate(HitEffect[0], transform.position, Quaternion.Euler(0, 180, 0));
			Debug.Log("아야");
            CurrentHp -= 1;
            StartCoroutine("Hit");
			cameraShaker.VibrateForTime(0.5f);
        }
        else
        {
			if (EnemyAttackPos == 1) Instantiate(HitEffect[1], transform.position, Quaternion.Euler(0, 0, 0));
			else if (EnemyAttackPos == -1) Instantiate(HitEffect[1], transform.position, Quaternion.Euler(0, 180, 0));
			Debug.Log("느려");
        }
    }
    private void PlayerAirAnimation()
    {
        if (playerDetect.IsWall)
        {
            playerAnimator.SetBool("IsWall", false);
        }
        if (playerDetect.IsGround)
        {
            playerAnimator.SetBool("Air", false);
            playerAnimator.SetBool("IsWall", false);
        }
        if (playerMove.playerRigid.velocity.y > 0)
        {
            playerAnimator.SetBool("Jump", true);
        }
        else if (playerMove.playerRigid.velocity.y < 0)
        {
            playerAnimator.SetBool("Jump", false);
            if (playerDetect.IsWall)
            {
                playerAnimator.SetBool("IsWall", true);
                playerAnimator.SetBool("Air", false);
            }
            else
            {
                playerAnimator.SetBool("IsWall", false);
                playerAnimator.SetBool("Air", true);
            }
        }
    }
    private void PlayerFireAttack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && playerElements.ElementFire && CurrentMp >= FireCost)
        {
            CurrentMp -= FireCost;
            if (playerSP.flipX)
            {
                Instantiate(fireAttackPrefab, new Vector2(transform.position.x-2,transform.position.y), Quaternion.Euler(0f, 180f, 0));
            }
            else
            {
                Instantiate(fireAttackPrefab, new Vector2(transform.position.x +2, transform.position.y), Quaternion.identity);
            }
        }
    }
    private void PlayerGolemAttack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && playerElements.ElementGround && CurrentMp >= GroundCost)
        {
            CurrentMp -= GroundCost;
            if (playerSP.flipX)
            {
                Instantiate(golemAttackPrefab, golemAttackTransform.position ,Quaternion.Euler(0f,180f,0));
            }
            else
            {
                Instantiate(golemAttackPrefab, golemAttackTransform.position, Quaternion.identity);
            }
        }
    }
    private void PlayerWindJump()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) && playerDetect.IsAir && playerElements.ElementWind && canWindJump && !playerDetect.IsGroundWall && CurrentMp > WindCost)
        {
            CurrentMp -= WindCost;
            playerMove.WindJump();
            Instantiate(windEffectPrefab,windEffectTransform);
        }
    }
    private void PlayerRolling()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRolling && !isAttack && playerDetect.IsGround)
        {
            StartCoroutine("Rolling");
            playerAnimator.SetTrigger("Rolling");
        }
    }
    private void PlayerAttack()
    {
        if (!playerDetect.IsWall)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attack1)
            {
                attack1 = true;
                AttackEnemy(0);
                StartCoroutine("AttackCooldown");
                playerAnimator.SetTrigger("Attack1");

                SoundManager.instance.PlayOneShot(source, clip);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && !attack2 && attack1 && canNextAttack)
            {
                StopCoroutine("AttackCooldown");
				AttackEnemy(1);
				StartCoroutine("AttackCooldown");
                attack2 = true;
                playerAnimator.SetTrigger("Attack2");
                Debug.Log("2");

                SoundManager.instance.PlayOneShot(source, clip);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && attack2 && !attack3 && canNextAttack)
            {
                StopCoroutine("AttackCooldown");
				AttackEnemy(2);
				StartCoroutine("AttackCooldown");
                attack3 = true;
                playerAnimator.SetTrigger("Attack3");
                Debug.Log("3");

                SoundManager.instance.PlayOneShot(source, clip);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && attack3 && !attack4 && canNextAttack)
            {
                StopCoroutine("AttackCooldown");
				AttackEnemy(3);
				StartCoroutine("AttackCooldown"); // 이거 바꾸면 공격속도 조절가능
                attack4 = true;
                playerAnimator.SetTrigger("Attack4");
                Debug.Log("4");

                SoundManager.instance.PlayOneShot(source, clip);
            }
        }
    }

    private void AttackEnemy(int colliderNumber)
    {
		Collider2D[] EnemyAttackZone = Physics2D.OverlapBoxAll(attackZones[colliderNumber].bounds.center, attackZones[colliderNumber].size, 0,enemy);
		Collider2D[] BossAttackZone = Physics2D.OverlapBoxAll(attackZones[colliderNumber].bounds.center, attackZones[colliderNumber].size, 0,boss);
        
        foreach (Collider2D col in EnemyAttackZone)
		{
			EnemyMain enemy = col.GetComponent<EnemyMain>();
			if (enemy)
			{
				enemy.Hit(playerMove.LastH);
			}
		}
        foreach(Collider2D col in BossAttackZone)
        {
            BossController boss = col.GetComponent<BossController>();
            if (boss)
            {
                boss.TakeHit(playerMove.LastH);
            }
        }
        
	}

    private void AttackController()
    {
        canNextAttack = false;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        attack4 = false;
    }
    IEnumerator AttackCooldown()
    {
        isAttack = true;
        playerAnimator.SetBool("IsAttack", true);
        canNextAttack = false;
        yield return new WaitForSeconds(0.5f);
        canNextAttack = true;
        isAttack = false;
        playerAnimator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(1.5f);
        AttackController();
        Debug.Log(2 + "기달림");
        yield break;
    }
    IEnumerator Rolling()
    {
        canRolling = false;
        isRolling = true;
        yield return new WaitForSeconds(0.7f);
        isRolling = false;
        yield return new WaitForSeconds(1f);
        canRolling = true;
        yield break;
    }
    IEnumerator Hit()
    {
        playerSP.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        playerSP.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
        playerSP.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        playerSP.color = new Color(1, 1, 1, 1);
    }
}
