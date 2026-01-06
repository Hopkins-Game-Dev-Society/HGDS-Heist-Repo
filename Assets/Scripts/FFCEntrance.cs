using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Shawn Guo
 * Updated: 1/6/2026
 */
public class FFCEntrance : MonoBehaviour
{
    [Header("Refs")] 
    [SerializeField] private GameObject winCanvas;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.GetComponent<PlayerMovement>().GetHasGottenTakeoutBox())
        {
            winCanvas.SetActive(true);
        }
    }
}
