using UnityEngine;

public class BackgroundMovementBehaviour : MonoBehaviour
{
    public float ScrollSpeed = 2f;
    public float MaxHeightBeforeRepeat = -240;
    public float lerpValue = 0.5f;
    private Rigidbody2D player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, ScrollSpeed * -1);
        player = GameInfo.Player.GetComponentInChildren<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y <= MaxHeightBeforeRepeat)
        {
            transform.position = new Vector2(0, 0);
        }

        if (player != null && player.velocity.x != 0)
        {

            if (player.velocity.x > 0)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(ScrollSpeed * -1 * 3, rb.velocity.y), lerpValue);
            }
            else if (player.velocity.x < 0)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(ScrollSpeed * 3, rb.velocity.y), lerpValue);

            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), lerpValue);
        }
    }
}
