using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{

    public GameObject ballPrefab;
    public GameObject slingGO; //sling on the slingshot

    private bool ready = true;
    private GameObject currBall;
    private SteamVR_TrackedObject trackedController; //controller
    private bool inSlingShot = false; //controller is in slingshot
    private Vector3 slingShotStart; //starting position of sling

    // Use this for initialization
    void Start()
    {
        slingShotStart = slingGO.transform.position; //sets the starting point for the sling
    }

    // Update is called once per frame
    void Update()
    {

        if (ready)
        {
            currBall = Instantiate(ballPrefab);
            currBall.transform.parent = slingGO.transform; // assigns transform.position to slingGo so it''s always where the sling is. also makes sling the parent.
            currBall.transform.localPosition = Vector3.zero; //sets ball to transform position 0,0,0 locally on sling
            ready = false; // prevents duplication of balls
        }

        if (trackedController != null) //check to see that controller isn't null
        {
            var device = SteamVR_Controller.Input((int)trackedController.index); // sets controller as device

            if (inSlingShot)
            {
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) // if trigger is NOT touched on controller
                {
                    ready = true; //sets the sling back and gets another ball
                    inSlingShot = false;
                    currBall.transform.parent = null; //this UNPARENTS the ball from the slingshot
                    slingGO.transform.position = slingShotStart;

                    Rigidbody r = currBall.GetComponent<Rigidbody>();
                    r.velocity = Vector3.forward * 5f;
                }
                else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) // if trigger is touched on controller
                {
                    slingGO.transform.position = trackedController.transform.position; // the sling now tracks with the controller
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) //trigger for when controller is colliding with the sling
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>();
        if (trackedController != null)
        {
            inSlingShot = true;
            Debug.LogError("Sling is in");
        }
    }

    void OnTriggerExit(Collider other) //trigger for when controller is not colliding with the sling
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>();
        if (trackedController != null)
        {
            inSlingShot = false;
            Debug.LogError("Sliing is out");
        }
    }
}
