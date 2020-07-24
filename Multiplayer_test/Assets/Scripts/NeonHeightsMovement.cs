using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NeonHeightsMovement : MonoBehaviour
{
    BoxCollider2D thisCollider;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    private Vector2 inputVector;
    private Vector2 contactNormal;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.thisCollider = GetComponent<BoxCollider2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        if (inputVector.x > 0)
        {
            spriteRenderer.flipX = false;
            print(contactNormal.normalized);
            //SetVelocity(5*contactNormal.normalized.y, 5*contactNormal.normalized.x);
            SetVelocity(5, rigidBody.velocity.y);
        }
        else if (inputVector.x < 0)
        {
            spriteRenderer.flipX = true;
            print(contactNormal.normalized);
            //SetVelocity(-5 * contactNormal.normalized.y, -5 * contactNormal.normalized.x);
            SetVelocity(-5, rigidBody.velocity.y);
        }

    }

    void SetVelocity(float x, float y)
    {
        rigidBody.velocity = new Vector2(x, y);
    }

    public void OnMove(InputValue value)
    {

        if (!Application.isFocused)
            return;

        inputVector = value.Get<Vector2>();
    }

    public void OnSelect()
    {
        SetVelocity(rigidBody.velocity.x, 10);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Visualize the contact point
            contactNormal = contact.normal;
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
