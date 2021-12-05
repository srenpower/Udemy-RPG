    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // added to be able to access the tile map

public class CameraController : MonoBehaviour
{
    // Varliables
    public Transform target; // used to target player to follow with camera

    public Tilemap theMap; // references the map
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight; // used to calculate where the camera should stop
    private float halfWidth; // used to calculate where the camera should stop

    public int musicToPlay; // set what music should play in scene
    private bool musicStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //target = PlayerController.instance.transform;
        // searches objects in scene, finds any object that has a player controller script and recognizes it as the target
        target = PlayerController.instance.transform;

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0); // keeps camera from going off the scene by stopping inside the bounds
        topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight, 0); // keeps camera from going off the scene by stopping inside the bounds

        // passes map bounds to player controller object
        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
        // does the same thing as the above code but on an object PlayerController and not the instance
        // FindObjectOfType<PlayerController>().SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        /* 
         * we don't want z position to be target position - want to keep it as the transform position so
         * the camera stays above the scene and player
         */

        // keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        // set music if it hasn't been started
        if(!musicStarted)
        {
            Debug.Log("Call PlayBGM");
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}
