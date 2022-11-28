using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Golem"))
        {
            StartCoroutine("DestroyObject");
        }
    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
