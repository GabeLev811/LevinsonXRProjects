using UnityEngine;
using UnityEngine.XR;

/*

 */




public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject objectPrefab; //object to spawn
    public Transform spawnPoint; //where it spawns
    public XRNode controllerNode = XRNode.RightHand;
    public float spawnCooldown = 1.0f; // Need a coroutine
    public bool canSpawn = true;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
