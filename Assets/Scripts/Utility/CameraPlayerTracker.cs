using UnityEngine;

public class CameraPlayerTracker : MonoBehaviour
{

    [SerializeField] private Transform player;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
