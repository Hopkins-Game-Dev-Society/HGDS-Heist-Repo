using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Author: Shawn Guo
 * Last Updated: 1/6/2025
 */
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5;

    [SerializeField] private string noTakeOutBoxMessage = "\"I really need a takeout box...\"";
    [SerializeField] private string yesTakeOutBoxMessage = "\"I got the takeout box, now I need to escape...\"";

    [Header("Read Only - DO NOT CHANGE")] 
    [SerializeField] private bool hasGottenTakeoutBox = false;

    [Header("Object Refs")] 
    [SerializeField] private GameObject statusTextObject;

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

    private void Update()
    {
        UpdateStatusText();
    }

    public void SetHasGottenTakeoutBox(bool hasGottenTakeoutBox)
    {
        this.hasGottenTakeoutBox = hasGottenTakeoutBox;
    }

    public bool GetHasGottenTakeoutBox()
    {
        return  this.hasGottenTakeoutBox;
    }

    void UpdateStatusText()
    {
        if (!hasGottenTakeoutBox)
        {
            statusTextObject.GetComponent<TMP_Text>().text = noTakeOutBoxMessage;
        }
        else
        {
            statusTextObject.GetComponent<TMP_Text>().text = yesTakeOutBoxMessage;
        }
    }
}
