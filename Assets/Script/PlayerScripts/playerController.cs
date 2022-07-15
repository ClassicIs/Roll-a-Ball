using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerController : MonoBehaviour {    
    
    private bool isGrounded;
    [Header("Player's Charactersitics")]    
    [SerializeField]
    float forwardForce;
    [SerializeField]
    float sidewayForce;
    [SerializeField]
    float jumpForce;
    [SerializeField]
    Vector3 moveForce;

    [SerializeField]
    float maxIncreaseForce;

    [SerializeField]
    float radiusOfMagnit;
    [SerializeField]
    float timeForMagnit;
    [SerializeField]
    LayerMask whatToAttract;
    [SerializeField]
    float speedToAttract;
    float increaseForce;
    bool isRushing;

    bool canUseMagnit;

    bool isStunned;
    bool isJumping;
    bool isActive;

    private Rigidbody rb;
    List<Collider> coins = new List<Collider>();

    enum KeyState
    {
        Pressed,
        Off
    }
    KeyState jumpKey;
    KeyState rushKey;
    private IEnumerator CoinAttractorCoroutine;

    public Action OnEscape;
    void Awake()
    {
        jumpKey = KeyState.Off;
        rushKey = KeyState.Off;
        isJumping = false;
        isStunned = false;
        isActive = true;
        canUseMagnit = true;
        isRushing = false;
        increaseForce = 1f;
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
        GameStateManager.Instance.OnGameStateChanged += ChangeState;
    }

    private void ChangeState(GameStates state)
    {
        switch(state)
        {
            case GameStates.Gameplay:
                isActive = true;
                break;
            case GameStates.Paused:
                isActive = false;
                rb.velocity = new Vector3(0f, 0f, 0f);                
                break;
        }
    }

    public void IsInStun()
    {
        isStunned = true;
    }

    public void OnGround()
    {
        isGrounded = true;
    }

    public void OffGround()
    {
        isGrounded = false;
    }

    public bool Rushing()
    {
        return isRushing;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(OnEscape != null)
            {
                OnEscape();
            }
        }

        if (!isActive)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpKey = KeyState.Pressed;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("Left Shift Pressed");
            rushKey = KeyState.Pressed;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            Debug.Log("Left Shift up");
            rushKey = KeyState.Off;
        }
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }

        if (isStunned)
        {
            return;
        }

        if (rb.velocity.y < 0)
        {
            rb.AddForce(new Vector3(0, -300));
        }
            
        rb.AddForce(Vector3.forward * forwardForce * increaseForce * Time.deltaTime);
        /*if(Input.GetKeyDown(KeyCode.A))
        {
            rb.MovePosition(transform.position + moveForce * -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rb.MovePosition(transform.position + moveForce * 1);
        }*/
        if (!isGrounded)
        {
            rb.AddForce(Vector3.right * sidewayForce / 2f * Input.GetAxisRaw("Horizontal") * Time.deltaTime, ForceMode.VelocityChange);
            return;
        }

        rb.AddForce(Vector3.right * sidewayForce * Input.GetAxisRaw("Horizontal") * Time.deltaTime, ForceMode.VelocityChange);
        if (jumpKey == KeyState.Pressed)
        {
            Debug.Log("Is jumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpKey = KeyState.Off;
        }

        if (rushKey == KeyState.Pressed)
        {
            isRushing = true;
            increaseForce = maxIncreaseForce;
        }
        else
        {
            isRushing = false;
            increaseForce = 1f;
        }
        
        if(Input.GetKeyDown(KeyCode.I) && canUseMagnit)
        {
            Debug.Log("Using Magnit");
            canUseMagnit = false;
            CoinAttractorCoroutine = AttractingCoins();
            StartCoroutine(CoinAttractorCoroutine);
            this.Invoke(delegate { 
                canUseMagnit = true;
                StopCoroutine(CoinAttractorCoroutine);
                Debug.Log("Stopping coroutine");
            }, timeForMagnit);
        }
    }

    IEnumerator AttractingCoins()
    {
        List<Collider> coinsToChoose = new List<Collider>();
        while (true)
        {
            coinsToChoose.Clear();
            coinsToChoose.AddRange(Physics.OverlapSphere(transform.position, radiusOfMagnit, whatToAttract));
            
            foreach (Collider chooseCoin in coinsToChoose)
            {
                //Debug.LogFormat("Choosen coins are", chooseCoin.name);
                StartCoroutine(AttractCoins(chooseCoin.transform));
                chooseCoin.gameObject.layer = 0;
            }
            yield return null;
        }
    }
    
    IEnumerator AttractCoins(Transform coinToAttract)
    {
        if (coinToAttract == null)
        {
            Debug.LogFormat("Coin {0} is null.", coinToAttract.gameObject.name);
            yield break;
        }
        float lerping = 0f;
        while (coinToAttract != null)
        {
            lerping += speedToAttract * Time.deltaTime;
            //Debug.LogFormat("Lerping coin {0}, with speed", coinToAttract.gameObject.name, lerping);
            coinToAttract.position = Vector3.Lerp(coinToAttract.position, transform.position, lerping);

            yield return null;
        }
    }
    

    private void OnDrawGizmos()
    {
        if(!canUseMagnit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusOfMagnit);
        }
    }
}
