using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BulletWall"))
        {
            Destroy(transform.parent.gameObject);
        }

        else if (collision.collider.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject, 0.2f);
        }
    }
}
