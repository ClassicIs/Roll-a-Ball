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


    private Rigidbody rb;
    List<Collider> coins = new List<Collider>();

    public Action OnEscape;
    void Awake()
    {
        jumpKey = KeyState.Off;
        isJumping = false;
        isStunned = false;
        canUseMagnit = true;
        isRushing = false;
        increaseForce = 1f;
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
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

        if (Input.GetButtonDown("Jump"))
        {
            jumpKey = KeyState.Pressed;
        }
    }

    enum KeyState
    {
        Pressed,
        Off
    }
    KeyState jumpKey;
    void FixedUpdate()
    {
        if(rb.velocity.y < 0)
        {
            rb.AddForce(new Vector3(0, -300));
        }
        if (!isStunned)
        {
            rb.AddForce(Vector3.forward * forwardForce * increaseForce * Time.deltaTime);
            
            
            /*if(Input.GetKeyDown(KeyCode.A))
            {
                rb.MovePosition(transform.position + moveForce * -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                rb.MovePosition(transform.position + moveForce * 1);
            }*/
            if (isGrounded)
            {
                rb.AddForce(Vector3.right * sidewayForce * Input.GetAxisRaw("Horizontal") * Time.deltaTime, ForceMode.VelocityChange);
                if (jumpKey == KeyState.Pressed)
                {
                    Debug.Log("Is jumping");
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    jumpKey = KeyState.Off;
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isRushing = true;
                    increaseForce = maxIncreaseForce;
                }
                else
                {
                    isRushing = false;
                    increaseForce = 1f;
                }
                /*
                if(Input.GetKeyDown(KeyCode.I) && canUseMagnit)
                {
                    Debug.Log("Using Magnit");
                    canUseMagnit = false;
                    StartCoroutine(AttractingCoins());
                    this.Invoke(delegate { 
                        canUseMagnit = true;
                        StopCoroutine(AttractingCoins());
                        Debug.Log("Stopping coroutine");
                    }, timeForMagnit);
                }*/
            }
            else
            {
                rb.AddForce(Vector3.right * sidewayForce / 2f * Input.GetAxisRaw("Horizontal") * Time.deltaTime, ForceMode.VelocityChange);
            }
        }              
    }

    IEnumerator AttractingCoins()
    {
        List<Collider> coinsToChoose = new List<Collider>();
        while (true)
        {
            IEnumerator tmpCoroutine;
            coinsToChoose.Clear();
            coinsToChoose.AddRange(Physics.OverlapSphere(transform.position, radiusOfMagnit, whatToAttract));

            foreach (Collider chooseCoin in coinsToChoose)
            {
                //Debug.LogFormat("Coin to be attracted {0}", coin.gameObject.name);
                bool isItThere = false;


                if (isItThere)
                {
                    coins.Add(chooseCoin);

                }
                /*tmpCoroutine = CoroutineToAttract(coin.transform);
                StartCoroutine(tmpCoroutine);
                this.Invoke(delegate { StopCoroutine(tmpCoroutine); }, timeForMagnit);*/
            }
            yield return null;
        }
    }

    List<Collider> Subtract(List<Collider> original, List<Collider> newOnes)
    {
        List<Collider> objectsThatAreInBothLists = new List<Collider>();
        Collider objectToAddToTheList;
        foreach (Collider objectOriginal in original)
        {
            objectToAddToTheList = null;

            foreach (Collider objectNew in newOnes)
            {
                objectToAddToTheList = objectNew;
                if (objectOriginal == objectNew)
                {
                    objectToAddToTheList = null;
                    break;
                }
            }
            if (objectToAddToTheList != null)
            {
                objectsThatAreInBothLists.Add(objectToAddToTheList);
            }
        }
        return objectsThatAreInBothLists;
    }

    bool AttractCoins(Transform coinToAttract)
    {
        if (coinToAttract == null)
        {
            Debug.LogFormat("Coin {0} is null.", coinToAttract.gameObject.name);
            return false;
        }
        Debug.LogFormat("Lerping coin {0}", coinToAttract.gameObject.name);
        coinToAttract.position = Vector3.Lerp(coinToAttract.position, transform.position, speedToAttract);
        return true;
    }
    /*
    IEnumerator StopCoroutineAfter(IEnumerator cor, float delay)
    {
        yield return 
    }*/

    IEnumerator CoroutineToAttract(Transform coin)
    {
        while (true)
        {
            if (AttractCoins(coin))
            {
                continue;
            }
            else
            {
                break;
            }
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
