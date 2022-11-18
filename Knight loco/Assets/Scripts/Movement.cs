using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

    //Player position
    public Transform player;

    //Mouse
    public Transform mouse;

    //Movement
    public Rigidbody2D rb;
    public Vector3 mousePos;
    Vector3 mouseDir;
    Vector3 moveDir;
    public float speed = 5.0f;

    //Groundecheck
    public LayerMask groundLayer;
    public bool grounded = false;

    //Jump
    public float jumpForce = 20;
    public bool jumping = false;
    public bool jumpContition = false;

    //Dash
    Vector3 dashDir;
    public float dashForce;
    public float _dashTime;
    float dashTime;
    bool dashing = false;
    bool getDir = false;
    float dashCost;
    TrailRenderer dashTrial;
    float _trialTime;
    float trialTime;

    //Boots stamina bar
    public Image bootsBar;

    //Attack
    public GameObject sword;
    GameObject swordReference;
    bool charging = false;
    bool attacking = false;
    //Thrust
    public GameObject thrust;
    public bool thrustGrab = false;
    public float grabTime = 3.0f;
    public Vector3 auxPos;
    public float thrustTime = 0.1f;
    public float t = 0.25f;
    //Slide
    public bool bouncing = false;
    public float bounceForceMult;
    float bounceForce;
    Vector2 bounceDir;
    //Attack Bar
    public Image chargeAttackBar;
    public float attackBarTimer = 0.0f;
    public float chargedTime = 1.0f;
    int wichAttack;
    int regenMult = 1;



    // Use this for initialization
    void Start () {
        player.position = transform.position;
        rb = GetComponent<Rigidbody2D>();
        dashTrial = GetComponent<TrailRenderer>();
        dashCost = 1.0f;
        dashTime = _dashTime;
        _trialTime = dashTrial.time;
        trialTime = _trialTime;
    }
	// Update is called once per frame
	void Update () {
        //Cursor.visible = false;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseDir = (mousePos - transform.position).normalized;
        mouse.position = transform.position + (mouseDir * 4f);
        //Movement
        IsLanding = grounded;
        if (!dashing) {
            moveDir.x = (mousePos - transform.position).normalized.x;
            if (mousePos.x > 0.0f)
            {
                if (mouseDir.x < 0.5f)
                {
                    moveDir.x = 0.5f;
                }
            }
            if (mouseDir.x < -0.0f)
            {
                if (mouseDir.x > -0.5f)
                {
                    moveDir.x = -0.5f;
                }
            }
            print(moveDir);
            if (Input.GetKeyDown(KeyCode.W) && thrustGrab)
            {
                grabTime = 0;
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(moveDir.x * speed, rb.velocity.y);
            }
            if (Input.GetKeyUp(KeyCode.W) || (IsLanding && !Input.GetKey(KeyCode.W) && Mathf.Abs(rb.velocity.x) > 0))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grabTime = 0;
            if (grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumping = true; 
                if(rb.velocity.y > 0)
                {
                    rb.gravityScale = 5.0f;
                }
            }
        }

        //Dash
        if(!dashing && Input.GetKeyDown(KeyCode.E))
        {
            grabTime = 0;
            jumping = false;
            if(bootsBar.fillAmount >= dashCost)
            {
                dashing = true;
                dashTrial.enabled = true;
            }
        }
        if (dashing)
            Dash();
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0 && jumping)
        {
            rb.gravityScale = 5.0f;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            jumping = false;
        }
        if (jumping && rb.velocity.y <= 0)
            rb.gravityScale = 6.0f;
        if(!dashing && rb.velocity.y <= (-25))
            rb.velocity = new Vector2(rb.velocity.x, -25);

        if (dashTrial.enabled)
        {
            trialTime -= Time.deltaTime;
        }
        if(trialTime <= 0)
        {
            trialTime = _trialTime;
            dashTrial.enabled = false;
        }
        //print(rb.velocity.y);

        //Bootsbar
        if (bootsBar.fillAmount < 1)
        {
            if (grounded)
            {
                bootsBar.fillAmount += Time.deltaTime * 5;
            }
            else
            {
                bootsBar.fillAmount += Time.deltaTime * 0.1f;
            }
        }

        //Attack
        if (Input.GetMouseButtonDown(0) && attackBarTimer == 0)
        {
            grabTime = 0;
            charging = true;
        }
        if (charging)
        {
            if(attackBarTimer < chargedTime)
            {
                attackBarTimer += Time.deltaTime*2;
            }
            if (attackBarTimer >= chargedTime)
                attackBarTimer = chargedTime;
        }
        if(Input.GetMouseButtonDown(1))
        {
            charging = false;
            attacking = true;
            wichAttack = 0;
            regenMult = 1;
            swordReference = thrust;
            if (attackBarTimer <= 0.075f)
                attackBarTimer = 0.075f;
             
            swordReference.transform.up = mouseDir;
        }
        if (Input.GetMouseButtonUp(0))
        {
            charging = false;
            attacking = true;
            if (attackBarTimer <= chargedTime)
            {
                wichAttack = 1;
                swordReference = sword;
                if (attackBarTimer <= (1.0f / 4))
                {
                    wichAttack = 1;
                    regenMult = 1;
                }
                else if (attackBarTimer <= (2.0f / 4))
                {
                    regenMult = 3;
                }
                else if (attackBarTimer <= (3.0f / 4))
                {
                    regenMult = 6;
                }
                else if (attackBarTimer <= 1)
                {
                    regenMult = 8;
                }
            }
            swordReference.transform.up = mouseDir;
        }
        //print("bouncing = " + bouncing);
        if (attacking)  
        {
            swordReference.SetActive(true);
            if (bouncing)
            {
                bounceDir = -swordReference.transform.up;
                bounceForce = attackBarTimer * bounceForceMult;
                rb.velocity = bounceDir * bounceForce;
                bouncing = false;
            }
            if (attackBarTimer > 0)
            {
                attackBarTimer -= Time.deltaTime*regenMult;
            }
            if (attackBarTimer <= 0)
            {
                attackBarTimer = 0;
                if (!thrustGrab)
                {
                    swordReference.SetActive(false);
                }
                attacking = false;
            }
        }
        chargeAttackBar.fillAmount = attackBarTimer;
        
    }

    void Dash()
    {
        dashTime -= Time.deltaTime;
        if(!getDir)
        {
            bootsBar.fillAmount -= dashCost;
            dashDir = mouseDir;
            dashDir.z = 0;
            getDir = true;
        }
        rb.velocity = dashDir * dashForce;
        if(dashTime <= 0)
        {
            rb.velocity *= 0.1f;
            dashTime = _dashTime;
            getDir = false;
            dashing = false;
        }
    }

    public bool IsLanding
    {
        get => _isLanding;
        private set
        {
            if (!value)
            {
                _hasLanded = false;
            }
            if (!_hasLanded && value)
            {
                _isLanding = true;
                _hasLanded = true;
            }
            else
            {
                _isLanding = false;
            }
        }
    }bool _isLanding; bool _hasLanded;
}
