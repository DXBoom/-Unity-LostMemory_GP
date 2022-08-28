using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    public GameObject Platform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.CheckSelectPlatform(Platform);
        }

        else if (collision.collider.CompareTag("Bullets"))
        {
            return;
        }

        else
        {
            GameManager.Instance.LoseGame();
        }
    }
}
