using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;
    PlayerControl movementController;
    Rigidbody2D rb2d;
    Vector3 moveDirection;
    private Vector3 startPosition;

    private void Awake() {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Start() {
        targetPos = posB.position;
        DirectionCalculate();
    }

    private void Update() {
        if(Vector2.Distance(transform.position, posA.position) < 0.05f) {
            targetPos = posB.position;
            DirectionCalculate();
        }

        if(Vector2.Distance(transform.position, posB.position) < 0.05f) {
            targetPos = posA.position;
            DirectionCalculate();
        }
    }

    private void FixedUpdate() {
        rb2d.velocity = moveDirection * speed;
    }

    void DirectionCalculate() {
        moveDirection = (targetPos - transform.position).normalized;
    }

    public void ResetPlatform() {
        transform.position = startPosition; // Reset the position to the initial position
        rb2d.velocity = Vector2.zero; // Reset the velocity to zero
        targetPos = posB.position; // Set the target position to posB
        DirectionCalculate(); // Recalculate the direction
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            PlayerControl playerControl = collision.GetComponent<PlayerControl>();
            AutoPlayerControl autoPlayerControl = collision.GetComponent<AutoPlayerControl>();

            if (playerControl != null) {
                playerControl.isOnPlatform = true;
                playerControl.platformRb = rb2d;
            }
            
            if (autoPlayerControl != null) {
                autoPlayerControl.SetPlatformState(true, rb2d);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            PlayerControl playerControl = collision.GetComponent<PlayerControl>();
            AutoPlayerControl autoPlayerControl = collision.GetComponent<AutoPlayerControl>();

            if (playerControl != null) {
                playerControl.isOnPlatform = false;
            }

            if (autoPlayerControl != null) {
                autoPlayerControl.SetPlatformState(false, null);
            }
        }
    }
}

