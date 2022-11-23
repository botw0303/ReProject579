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
	public BoxCollider2D AttackSize;

	[Header("��ȭ ��� ����")]
	public int MinMoney;
	public int MaxMoney;
	public bool IsFixed;
	public int FixdeMoney;

	[Header("�� ������Ʈ")]
	Rigidbody2D EnemyRigidbody;
	Collider2D EnemyCollider;
	SpriteRenderer EnemyRenderer;
	Animator EnemyAnima;
	[SerializeField] GameObject Attackparent;

	int lastMove = 1;
	int nextMove;
	float enemyMoving;

	void Awake()
	{
		/*transform.rotation = Quaternion.Euler(Vector3.zero);*/
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
		Debug.Log($"���� HP : {CurHealthPoint}");
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
		RaycastHit2D Ground = Physics2D.Raycast(frontVector, Vector3.down, 1, LayerMask.GetMask("Ground"));
		if (Ground.collider == null)
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
		Debug.DrawRay(transform.position, searchVec, Color.red);
		RaycastHit2D search = Physics2D.Raycast(EnemyCollider.bounds.center, searchVec, RayDistance, LayerMask.GetMask("Player"));
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
		Vector2 searchVec = new Vector2(lastMove, 0);
		Collider2D[] AttackZone = Physics2D.OverlapBoxAll(searchVec, AttackSize.size, 0);
		foreach (Collider2D col in AttackZone)
		{
			PlayerSkill player = col.GetComponent<PlayerSkill>();
			if (player)
			{
				player.PlayerGetDamage(); // ������ �Դ°�
			}
		}

		#endregion


		void DropMoney()
		{
			if (CurHealthPoint <= 0)
			{
				if (IsFixed)
				{
					//�÷��̾��� ���� ��ȭ�� FIxedMoney�� ���� ���Ѵ�.
				}
				else if (!IsFixed)
				{
					int Money = Random.Range(MinMoney, MaxMoney);
					//�÷��̾��� ���� ��ȭ�� Money�� ���� ���Ѵ�.
				}
			}
		}
	}
}
