﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifeController : MonoBehaviour {
    public int lives = 1;
    public float throwbackPower = 3f;
    private bool immune = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;
    private Color originalColor;
    public bool Immobile{get; set;}

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = gameObject.GetComponents<Collider2D>();
        originalColor = spriteRenderer.material.color;
        Immobile = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(!immune && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Hazard"))
        {

            lives--;
            immune = true;
            Immobile = true;
            StartCoroutine(TakeDamage(other));
            
        }
    }

    IEnumerator TakeDamage(Collision2D other)
    {
        Vector3 throwback = (other.transform.position - transform.position) * -throwbackPower;
        for (float f = 3f; f >= 0; f -= 1f)
        {
            rb.velocity = throwback;
            //Vector3 tmp = other.transform.position - transform.position;
            //Vector3 throwback = new Vector3(Mathf.Sign(tmp.x), Mathf.Sign(tmp.y), Mathf.Sign(tmp.z)) * -throwbackPower;
            if (f % 2 == 0) spriteRenderer.material.color = Color.red;
            else spriteRenderer.material.color = originalColor;
            Color c = spriteRenderer.material.color;
            c.a = 0.9f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(.1f);
        }
        //Debug.Log("baiges spalvos");
        immune = false;
        Immobile = false;
        rb.velocity = Vector3.zero;
        spriteRenderer.material.color = originalColor;
        if (lives <= 0) die();
    }

    private void die()
    {
        //Debug.Log("dydis:");
        //Debug.Log(colliders.Length);
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        Immobile = true;
    }
}
