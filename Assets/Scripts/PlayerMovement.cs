using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Shawn Guo
 * Last Updated: 1/6/2025
 */
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.MovePosition(rb.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.MovePosition(rb.position + Vector2.left * movementSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.MovePosition(rb.position + Vector2.down * movementSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.MovePosition(rb.position + Vector2.right * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
