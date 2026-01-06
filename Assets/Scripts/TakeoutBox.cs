using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Shawn Guo
 * Updated: 1/6/2026
 */
public class TakeoutBox : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameObject player;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().SetHasGottenTakeoutBox(true);
            Destroy(gameObject);
        }
    }
}
