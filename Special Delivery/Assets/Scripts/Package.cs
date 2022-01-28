using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    private PlayerMovement player;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        Reset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) {
            player.hasPackage = true;
            this.gameObject.SetActive(false);
        }
    }
    public void Reset()
    {
        if (!player.hasPackage)
        {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }
}
