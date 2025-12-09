using UnityEngine;

public class MenuRotateAroundPoint : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject centerPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(centerPoint.transform.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }
}
