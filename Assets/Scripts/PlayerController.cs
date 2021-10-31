using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public Rigidbody2D theRB;
    public float moveSpeed; // use to increase moveSpeed by multiplying it against Vector2 for theRB.velocity

    public Animator myAnim;

    public static PlayerController instance; // used to ensure only one player can be in the scene at any time
    // "static" is what makes this possible

    public string areaTransitionName; // stores which scene the player is in

    private Vector3 bottomLeftLimit; // used to calculate boundaries of map
    private Vector3 topRightLimit; // used to calculate boundaries of map 

    public bool canMove = true; // controls whether or not a player should be able to move
    // Start is called before the first frame update
    // Use for initialization
    void Start()
    {
        if (instance == null)
        {
            // if no player character exists when game starts instance value will be set to this player 
            instance = this;
        } 
        // instance exists
        else
        {
            // check instance - if it is not "this" instance, destroy it
            if (instance != this)
            {
               Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if canMove = true player can move as usual
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        // else do not allow movement
        else
        {
            // this could be blank instead but explicitly coding no movement gives no room for bugs
            theRB.velocity = Vector2.zero;
        }

         myAnim.SetFloat("moveX", theRB.velocity.x);
         myAnim.SetFloat("moveY", theRB.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        // keep the player inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    // sets bottomLeftLimit and topRightLimit to bounds of map when called
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, .7f, 0f); // map edge + offset so character position isn't based on its center
        topRightLimit = topRight + new Vector3(-.5f, -.7f, 0); // map edge + offset so character position isn't based on its center
    }
}
