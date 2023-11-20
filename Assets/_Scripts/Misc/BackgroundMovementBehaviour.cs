using UnityEngine;

public class BackgroundMovementBehaviour : MonoBehaviour
{
    public float ScrollSpeed = 2f;
    public float MaxHeightBeforeRepeat = -240;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (0, ScrollSpeed * -1);
    }
    private void FixedUpdate()
    {
        if(transform.position.y <= MaxHeightBeforeRepeat)
        {
            transform.position = new Vector2 (0, 0);
        }
    }
}
