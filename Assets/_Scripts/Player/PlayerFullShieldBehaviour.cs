using UnityEngine;

public class PlayerFullShieldBehaviour : MonoBehaviour
{

    private SpriteRenderer fullShieldSpriteRenderer;
    private CircleCollider2D fullShieldCollider;
    private CircleCollider2D playerCollider;

    private float invulnerabilityTimer = 2;
    private float invulnerablilityLength = 0.25f;

    private bool _fullShieldEnabled = false;
    public bool FullShieldEnabled
    {
        get
        {
            return _fullShieldEnabled;
        }
        private set
        {
            _fullShieldEnabled = value;
            fullShieldSpriteRenderer.enabled = value;
            fullShieldCollider.enabled = value;

            playerCollider.enabled = !value;
        }
    }

    private void Awake()
    {
        fullShieldSpriteRenderer = GetComponent<SpriteRenderer>();
        fullShieldCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        playerCollider = GameInfo.Player.transform.GetChild(0).GetChild(0).GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        HandleFullShieldLogic();
    }

    private void HandleFullShieldLogic()
    {
        if (invulnerabilityTimer <= invulnerablilityLength)
        {
            if (!FullShieldEnabled)
            {
                FullShieldEnabled = true;
            }
            invulnerabilityTimer += Time.deltaTime;
        }
        else if (FullShieldEnabled)
        {
            FullShieldEnabled = false;
        }
    }

    public void StartInvulnerability()
    {
        invulnerabilityTimer = 0;
        HandleFullShieldLogic();
    }

}
