using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    // Health
    public bool hasPaper = true;

    public bool isInvuln;
    private bool isDodging;
    private bool canDodge;
    private bool canFire;
    public float invulnLength = 0.5f;
    private float invulnTimeLeft;


    // Movement
    public PlayerControls playerControls;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]
    private float baseMoveSpeed;
    private float speed;
    private Vector3 inputDir;

    [SerializeField]
    private Vector3 botLeftMoveBound;
    [SerializeField]
    private Vector3 topRightMoveBound;


    public GameObject stunEffectObj; // stun visual indicator
    private bool isStunned;
    public float stunLength = 0.25f;
    private float stunTimeLeft;
    private bool held = false;

    // Projectiles
    public GameObject projectile1;
    public GameObject projectile2;

    // Paper
    public GameObject paper_scroll; // visual indicator if you carry the paper or not
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
    public ColorStrobe colorStrobe;

    void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canFire = true;
        canDodge = true;

        playerControls.Player.Fire1.performed += ctx => held = true;
        playerControls.Player.Fire1.canceled += ctx => held = false;

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
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -7f, 7f), transform.position.z);
        if (playerControls.Player.Dodge.triggered && canDodge)
        {
            Dodge();
        }
        if (held && canFire)
        {
            FireWeapon1();
        } else if (playerControls.Player.Fire2.triggered && canFire)
        {
            FireWeapon2();
        }
    }

    private void Dodge()
    {
        anim.Play("pigun_dodge");
        isDodging = true;
        isInvuln = true;
        canDodge = false;
        StartCoroutine(IFrameStart());
    }

    IEnumerator IFrameStart()
    {
        yield return new WaitForSeconds(1f);
        isDodging = false;
        isInvuln = false;
        StartCoroutine(DodgeCooldown());
    }

    IEnumerator DodgeCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        canDodge = true;
    }

    IEnumerator FireCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
        
    }

    private void FireWeapon2()
    {
        canFire = false;
        AudioManager.Play("Player Shoot");
        CameraShake.Shake(.1f,5);

        for (int i = 0; i < 15; i++)
        {
            Instantiate(projectile1, this.transform.position, Quaternion.Euler(0f,0f,i * 360/15));
        }
        
        anim.Play("pigun_fire");
        StartCoroutine(FireCooldown(0.5f));
    }

    private void FireWeapon1()
    {
        canFire = false;
        AudioManager.Play("Player Shoot");
        CameraShake.Shake(.1f,2);

        Instantiate(projectile2, this.transform.position, Quaternion.identity);
        anim.Play("pigun_fire");
        StartCoroutine(FireCooldown(0.1f));
    }

    private void FixedUpdate()
    {
        if (CanMove())
        {
            PlayerMovementHandler();
        }
        
        if (isInvuln && !isDodging)
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
                stunEffectObj.SetActive(false);

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

        CameraShake.Shake(0.25f, 5);
        //colorStrobe.StrobeWhite((int)(invulnLength / 0.25f));
        colorStrobe.StrobeAlpha((int)(invulnLength / 0.25f), 0.5f);

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
        stunEffectObj.SetActive(true);

        isStunned = true;
        stunTimeLeft = stunLength;
    }

    private void SetPaper(bool value)
    {
        // losing paper
        if (hasPaper && !value)
        {
            hasPaper = false;
            paper_scroll.SetActive(false);
            SpawnPaper();
            paperTimer.SetActive(true);
        }
        // picking up
        else if (!hasPaper && value)
        {
            hasPaper = true;
            paper_scroll.SetActive(true);
            AudioManager.Play("Paper Pickup");

            paperTimer.SetActive(false);
            playerSpotlightAnimator.SetBool("isOn", false);
            paperSpotlightAnimator.SetBool("isOn", false);
            TimeManager.ResumeTimeSmooth();
        }
    }



    private void SpawnPaper()
    {
        Vector3 randPos = new Vector3(UnityEngine.Random.Range(botLeftPaperBound.x, topRightPaperBound.x),
                                      UnityEngine.Random.Range(botLeftPaperBound.y, topRightPaperBound.y));

        int c = 0;
        while ((randPos - transform.position).magnitude < minPaperSpawnDist && c < 100000)
        {
            randPos = new Vector3(UnityEngine.Random.Range(botLeftPaperBound.x, topRightPaperBound.x),
                                  UnityEngine.Random.Range(botLeftPaperBound.y, topRightPaperBound.y));
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
