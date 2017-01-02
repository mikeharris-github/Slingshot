using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {


    public GameObject ballPrefab;
    public GameObject slingGO;
    private bool ready = true; //variable to tell us whether our slinghshot is loaded
    private GameObject currBall;
    private SteamVR_TrackedObject trackedController;
    private bool inSlingShot = false;
    private Vector3 slingShotStart; // starting position of slingshot band

	// Use this for initialization
	void Start () {

        slingShotStart = slingGO.transform.position; //sets the slingShotStart transform position
	}

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            currBall = Instantiate(ballPrefab); //spawn ball and make it current ball
            currBall.transform.parent = slingGO.transform; //place ball transform at slingGO's transform, which is also parent
            currBall.transform.localPosition = Vector3.zero; //resets position to local position 0,0,0.
            ready = false; //prevents duplicate balls from getting created
        }

        if (trackedController != null)
        {
            var device = SteamVR_Controller.Input((int)trackedController.index); // tracks controller 

            if (inSlingShot) // if inSlingShot = true
            {
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)) //when controller is released, have it reset (touchUp)
                {
                    inSlingShot = false; //
                    slingGO.transform.position = slingShotStart;
                }

                else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger)) // if trigger is pressed
                {
                    slingGO.transform.position = trackedController.transform.position; // slingshot now follows controller
                }
            }
        }
    }
    

    void OnTriggerEnter(Collider other) // tells that controller is in the slingshot. if pressed,
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>(); //gets the controller
        if (trackedController != null) //if controller is not null
        {
            inSlingShot = true;
        }
    }

    void OnTriggerExit(Collider other) // tells us that the controller is not in the slingshot
    {
        trackedController = other.GetComponent<SteamVR_TrackedObject>(); //gets the controller
        if (trackedController != null) //if controller is not null
        {
            inSlingShot = false;
        }
    }
}
