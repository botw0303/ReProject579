using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElment : MonoBehaviour
{
    public bool onFire = false;
    [SerializeField] GameObject fireObject;
    [SerializeField] float fireTime;
    [SerializeField] BoxCollider2D boxCol2d;
    [SerializeField] LayerMask playerLayer;
    PlayerElements player;
    [SerializeField] Collider2D playerCol;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerElements>();
    }
    private void Update()
    {
        if (onFire)
        {
            FireCoroutineStart();
        }
        CheckUseSkill();
    }

    public void CheckUseSkill()
    {
        if ((Vector2.Distance(transform.position, playerCol.transform.position)) > 7)
        {
            return;
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(boxCol2d.bounds.center, boxCol2d.bounds.size, 0f, playerLayer);

        if (cols.Length > 0 && Input.GetKeyDown(KeyCode.Mouse1) && player.ElementFire)
        {
            onFire = true;
        }


    }

    public void FireCoroutineStart()
    {
        StartCoroutine(OnFire());
    }

    IEnumerator OnFire()
    {
        fireObject.SetActive(true);

        yield return new WaitForSeconds(fireTime);

        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
