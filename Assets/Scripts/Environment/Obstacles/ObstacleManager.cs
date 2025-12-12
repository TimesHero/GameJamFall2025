using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [SerializeField] private int weight;
    [SerializeField] private int fill_value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int getWeight() {
        return weight;
    }

    public int getFillValue() {
        return fill_value;
    }
}
