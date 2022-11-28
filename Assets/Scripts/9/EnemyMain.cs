using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMain : MonoBehaviour
{
	[Header("���� ����")]
	public int MaxHealthPoint;
	int CurHealthPoint;
	public float Speed;
	public float RayDistance;
	public int AttackPoint;
	public int AttackDelay, DelayTemp;
	public Vector2 AttackSize;

	[Header("�� ������Ʈ")]
	Rigidbody2D EnemyRigidbody;
	Collider2D EnemyCollider;
	SpriteRenderer EnemyRenderer;
	Animator EnemyAnima;

	int lastMove = 1;
	int nextMove;
	float enemyMoving;
	bool isDead = false;

	void Awake()
	{
		EnemyRigidbody = this.GetComponent<Rigidbody2D>();
		EnemyCollider = this.GetComponent<BoxCollider2D>();
		EnemyRenderer = this.GetComponent<SpriteRenderer>();
		EnemyAnima = this.GetComponent<Animator>();
		EnemyAnima.SetFloat("Move", enemyMoving);
		CurHealthPoint = MaxHealthPoint;
		Invoke("Think", 3);
	}

	void Start()
	{
		StartCoroutine("AttackLate");
	}

	void Update()
	{
		SearchPlayer();
		if (AttackDelay == DelayTemp)
		{
			StopAllCoroutines();
			StartCoroutine("AttackLate");
		}
	}

	#region �����̱�
	void Moving(int move)
	{
		EnemyAnima.SetFloat("Move", enemyMoving);
		this.EnemyRigidbody.velocity = new Vector2(move * Speed, EnemyRigidbody.velocity.y);
		Vector2 frontVector = new Vector2(EnemyRigidbody.position.x + move * 0.5f, EnemyRigidbody.position.y);
		RaycastHit2D Ground = Physics2D.Raycast(frontVector, Vector3.down, 5, LayerMask.GetMask("Ground"));
		Debug.DrawRay(frontVector, Vector3.down, Color.red);
		//Debug.Log($"�� ���� :{(bool)Ground}");
		RaycastHit2D Wall = Physics2D.Raycast(EnemyCollider.bounds.center, new Vector2(nextMove, 0), 2f, LayerMask.GetMask("Ground"));
		//Debug.Log($"�� ���� :{(bool)Wall}");
		Debug.DrawRay(EnemyCollider.bounds.center, new Vector2(nextMove, 0), Color.blue);
		if (Ground.collider == null || Wall.collider != null)
		{
			Turn();
		}
	}
	void Turn()
	{
		nextMove *= -1;
		EnemyRenderer.flipX = nextMove == -1;
		CancelInvoke();
		Invoke("Think", 2);
	}
	void Think()
	{
		nextMove = Random.Range(-1, 2);
		EnemyAnima.SetFloat("Move", enemyMoving);
		if (nextMove != 0) EnemyRenderer.flipX = nextMove == -1;
		float nextThinkTime = Random.Range(2f, 5f);
		Invoke("Think", nextThinkTime);
	}
	#endregion

	void SearchPlayer()
	{
		Vector2 searchVec = new Vector2(lastMove, 0);
		RaycastHit2D search = Physics2D.Raycast(EnemyCollider.bounds.center, searchVec, RayDistance, LayerMask.GetMask("Player"));
		//Debug.Log($"�÷��̾� ���� : {(bool)search}");
		if (search.collider != null)
		{
			EnemyRenderer.flipX = lastMove == -1;
			nextMove = lastMove;
			enemyMoving = 0;
			Moving(0);
			Debug.Log("�÷��̾� ����");
			if (AttackDelay == 0)
			{
				Attack();
				EnemyAnima.SetTrigger("Attack");
				AttackDelay = DelayTemp;
			}
		}
		else if (search.collider == null)
		{
			if (nextMove != 0)
			{
				enemyMoving = 1;
				lastMove = nextMove;
			}
			else if (nextMove == 0) enemyMoving = 0;
			Moving(nextMove);
		}
	}

	IEnumerator AttackLate()
	{
		while (CurHealthPoint > 0 && AttackDelay != 0)
		{
			AttackDelay -= 1;
			yield return new WaitForSeconds(1f);
		}
	}

	#region �����ϱ�/�ޱ�

	void Attack()
	{
		Collider2D[] AttackZone = Physics2D.OverlapBoxAll(EnemyCollider.bounds.center, AttackSize, 0);
		foreach (Collider2D col in AttackZone)
		{
			PlayerSkill player = col.GetComponent<PlayerSkill>();
			if (player)
			{
				player.PlayerGetDamage();
			}
		}
	}

	public void Hit() //������ �Ա�
	{
		if (CurHealthPoint > 0)
		{
			CurHealthPoint--;
			EnemyAnima.SetTrigger("Hit");
		}
		if (CurHealthPoint == 0)
		{
			isDead = true;
			EnemyRigidbody.velocity = Vector2.zero;
			EnemyAnima.SetBool("Dead", isDead);
			Destroy(gameObject, EnemyAnima.GetCurrentAnimatorStateInfo(0).length);
		}
	}

	#endregion
}
