using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

//Need to INHERIT our corefeatures script
public class DoorFeatures : CoreFeatures
{
    [Header("Door Configuration")]
    [SerializeField]
    private Transform doorPivot; //Specifically the Y rotation

    [SerializeField]
    private float maxAngle = 90.0f; //Probably will need to be 90 degrees, but okay starting point

    [SerializeField]
    private bool reverseAngleDirection = false; //Flips Direction

    [SerializeField]
    private float doorspeed = 2.0f;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private bool MakeKinematicOnOpen = false;

    [Header("Interactions Configuration")]
    [SerializeField]
    private XRSocketInteractor socketInteractor;

    [SerializeField]
    private XRSimpleInteractable simpleInteractable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //When key gets close to the socket, add a listener
        //s = Shorthand,  SelectEnterEvents
        socketInteractor?.selectEntered.AddListener((s) => //ABSCTRACTION - hiding complexity
        {
            OpenDoor();
            PlayOnStart();

        });

        socketInteractor?.selectExited.AddListener((s) => 
        {
            PlayOnEnd();
            socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true;
        
        });

        //Doors with Simple Interactors may not require a "key". Also good for cabinets, drawers...
        simpleInteractable?.selectEntered.AddListener((s) =>
        {
           OpenDoor();

        });

        //Testing Only
        //OpenDoor();
    }


    public void OpenDoor()
    {
        //If the door is not open, Play the OnStart Sound
        if (!open)
        {
            PlayOnStart();
            open = true;

            StartCoroutine(ProcessMotion());

        }
    }

    private IEnumerator ProcessMotion()
    {
        //Keep Looking for whether door is open or not
        while(open)
        {
            var angle = doorPivot.localEulerAngles.y < 180 ? doorPivot.localEulerAngles.y : doorPivot.localEulerAngles.y - 360;

            angle = reverseAngleDirection ? Mathf.Abs(angle) : angle;

            if (angle <= maxAngle)
            {
                doorPivot?.Rotate(Vector3.up, doorspeed * Time.deltaTime * (reverseAngleDirection ? -1 : 1));
            }

            else
            {
                //When done with opening, turn off the rigidbody
                open = false;
                var featureRigidBody = GetComponent<Rigidbody>();
                if (featureRigidBody != null && MakeKinematicOnOpen) featureRigidBody.isKinematic = true;
            }

                yield return null;


        }
    }

}
