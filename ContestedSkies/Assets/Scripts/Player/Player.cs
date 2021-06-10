using UnityEngine;

public class Player : MonoBehaviour
{
    // Health
    public bool hasPaper = true;

    private bool isInvuln;
    public float invulnLength = 0.5f;
    private float invulnTimeLeft;


    // Movement
    public PlayerControls playerControls;
    private Rigidbody2D rb;

    [SerializeField]
    private float baseMoveSpeed;
    private float speed;
    private Vector3 inputDir;

    [SerializeField]
    private Vector3 botLeftMoveBound;
    [SerializeField]
    private Vector3 topRightMoveBound;
    

    private bool isStunned;
    public float stunLength = 0.25f;
    private float stunTimeLeft;

    // Projectiles
    public GameObject projectile1;
    public GameObject projectile2;

    // Paper

    public GameObject paperPrefab;
    public float minPaperSpawnDist;
    [SerializeField]
    private Vector3 botLeftPaperBound;
    [SerializeField]
    private Vector3 topRightPaperBound;

    [Header("References")]
    public PaperTimer paperTimer;
    public Animator playerSpotlightAnimator;
    public GameObject paperSpotlightPrefab;
    private Animator paperSpotlightAnimator;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        ResetPlayer();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }
    void OnDisable()
    {
        playerControls.Disable();
    }
    void Update()
    {
        if (playerControls.Player.Fire1.triggered)
        {
            FireWeapon1();
        } else if (playerControls.Player.Fire2.triggered)
        {
            FireWeapon2();
        }
    }

    private void FireWeapon2()
    {
        Instantiate(projectile1, this.transform.position, Quaternion.identity);
    }

    private void FireWeapon1()
    {
        Instantiate(projectile2, this.transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        if (CanMove())
        {
            PlayerMovementHandler();
        }
        
        if (isInvuln)
        {
            invulnTimeLeft -= Time.deltaTime;
            if (invulnTimeLeft <= 0)
            {
                invulnTimeLeft = 0;
                isInvuln = false;
            }
        }

        if (isStunned)
        {
            stunTimeLeft -= Time.deltaTime;
            if (stunTimeLeft <= 0)
            {
                stunTimeLeft = 0;
                isStunned = false;
            }
        }

        // if (CanMove())
        // {
        //     Vector3 nextPos = transform.position + inputDir * speed * Time.deltaTime;

        //     nextPos = CheckBounds(nextPos, botLeftMoveBound, topRightMoveBound);

        //     transform.position = nextPos;
        // }
    }

    public void PlayerMovementHandler()
    {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveInput.x * baseMoveSpeed, moveInput.y * baseMoveSpeed);
    }
    private bool CanMove()
    {
        return !isStunned;
    }

    private void TakeDamage()
    {
        if (isInvuln)
        {
            return;
        }

        AudioManager.Play("Player Damaged");

        CameraShake.Shake(0.25f, 1);

        isInvuln = true;
        invulnTimeLeft = invulnLength;
        StunSelf();

        if (hasPaper)
        {
            SetPaper(false);
        }
    }

    private void StunSelf()
    {
        AudioManager.Play("Stun");

        isStunned = true;
        stunTimeLeft = stunLength;
    }

    private void SetPaper(bool value)
    {
        // losing paper
        if (hasPaper && !value)
        {
            hasPaper = false;

            SpawnPaper();
            paperTimer.SetActive(true);
        }
        // picking up
        else if (!hasPaper && value)
        {
            hasPaper = true;

            AudioManager.Play("Paper Pickup");

            paperTimer.SetActive(false);
            playerSpotlightAnimator.SetBool("isOn", false);
            paperSpotlightAnimator.SetBool("isOn", false);
            TimeManager.ResumeTimeSmooth();
        }
    }



    private void SpawnPaper()
    {
        Vector3 randPos = new Vector3(Random.Range(botLeftPaperBound.x, topRightPaperBound.x),
                                      Random.Range(botLeftPaperBound.y, topRightPaperBound.y));

        int c = 0;
        while ((randPos - transform.position).magnitude < minPaperSpawnDist && c < 100000)
        {
            randPos = new Vector3(Random.Range(botLeftPaperBound.x, topRightPaperBound.x),
                                  Random.Range(botLeftPaperBound.y, topRightPaperBound.y));
            c += 1;
        }
        if (c == 100000)
        {
            Debug.LogError("Couldn't spawn paper");
        }
        //Debug.Log("Found paper spawn in " + c + " tries!");

        GameObject paperObj = Instantiate(paperPrefab, transform.position, Quaternion.identity);
        paperObj.GetComponent<Paper>().SetTarget(randPos);
        paperTimer.SetPaper(paperObj, paperSpotlightAnimator.gameObject);

        playerSpotlightAnimator.SetBool("isOn", true);
        paperSpotlightAnimator.SetBool("isOn", true);

        TimeManager.StartSlowMotion();
    }

    private Vector3 CheckBounds(Vector3 pos, Vector3 botLeftBound, Vector3 topRightBound)
    {
        if (pos.x < botLeftBound.x)
        {
            pos.x = botLeftBound.x;
        }
        else if (pos.x > topRightBound.x)
        {
            pos.x = topRightBound.x;
        }

        if (pos.y < botLeftBound.y)
        {
            pos.y = botLeftBound.y;
        }
        else if (pos.y > topRightBound.y)
        {
            pos.y = topRightBound.y;
        }

        return pos;
    }

    public void ResetPlayer()
    {
        hasPaper = true;
        isInvuln = false;
        invulnTimeLeft = 0;

        speed = baseMoveSpeed;
        isStunned = false;
        stunTimeLeft = 0;

        // cross scene issues?
        if (paperSpotlightAnimator == null)
        {
            paperSpotlightAnimator = Instantiate(paperSpotlightPrefab).GetComponent<Animator>();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Paper")
        {
            SetPaper(true);
        }
        else if (collision.tag == "Enemy" || collision.tag == "Bullet")
        {
            TakeDamage();
        }
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 topLeft = new Vector3(botLeftMoveBound.x, topRightMoveBound.y);
        Vector3 botRight = new Vector3(topRightMoveBound.x, botLeftMoveBound.y);
        Gizmos.DrawLine(botLeftMoveBound, topLeft);
        Gizmos.DrawLine(topLeft, topRightMoveBound);
        Gizmos.DrawLine(topRightMoveBound, botRight);
        Gizmos.DrawLine(botRight, botLeftMoveBound);

        Gizmos.color = Color.red;
        topLeft = new Vector3(botLeftPaperBound.x, topRightPaperBound.y);
        botRight = new Vector3(topRightPaperBound.x, botLeftPaperBound.y);
        Gizmos.DrawLine(botLeftPaperBound, topLeft);
        Gizmos.DrawLine(topLeft, topRightPaperBound);
        Gizmos.DrawLine(topRightPaperBound, botRight);
        Gizmos.DrawLine(botRight, botLeftPaperBound);
    }

    #endregion
}
