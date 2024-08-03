using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] float affected_Time = 10f;

    
    //Para controlar el tiempo de carol con la fruta
    private float time_since_fruit = 0f;
    private bool start_counter = false;
    private bool has_powerup = false;

    //manteneemos su tamaño original para regresarlo cuando queramos
    private Vector3 originalScale;

    //Hacia donde va carol
    private bool vaVertical = false;
    private bool vaHorizontal = false;

    //referencia a si mismo
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float MoveCaracolH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float MoveCaracolV = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if (vaVertical && vaHorizontal)
        {
            MoveCaracolH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            MoveCaracolV = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        }
        else if (vaVertical)
        {
            MoveCaracolH = 0;
        }
        else if (vaHorizontal)
        {
            MoveCaracolV = 0;
        }

        transform.Translate(MoveCaracolH, MoveCaracolV, 0);
    }

    private void FixedUpdate() //all physics things update en este tiempo (most of them)
    {
        if (start_counter)
        {
            time_since_fruit += Time.deltaTime;
        }

        if (time_since_fruit > affected_Time)
        {
            has_powerup = false;
            time_since_fruit = 0;
            start_counter = false;
            transform.localScale = originalScale;
        }
    }

    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared")
        {
            rb.gravityScale = 0;
            vaVertical = true;
        }
        if (collision.gameObject.tag == "Suelo")
        {
            rb.gravityScale = 1;
            vaHorizontal = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pared")
        {
            vaVertical = false;
            rb.gravityScale = 1;
        }
        if (collision.gameObject.tag == "Suelo")
        {
            vaHorizontal = false;
        }

        if (collision.transform.localPosition.x > transform.localPosition.x && !isVertical())
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        
    }

    private bool isVertical()
    {
        if (transform.rotation.z > 85)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Manzana")
        {
            transform.localScale = transform.localScale + new Vector3(0.1f, 0.1f, 0);
            handlePowerup(collision);
        }
        if (collision.gameObject.tag == "Pera")
        {
            transform.localScale = transform.localScale - new Vector3(0.1f, 0.1f, 0);
            handlePowerup(collision);

        }
    }

    private void handlePowerup(Collider2D collision){
        Destroy(collision.gameObject);
        start_counter = true;
        has_powerup = true;
    }
}



