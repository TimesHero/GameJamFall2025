using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [SerializeField] private int weight;
    [SerializeField] private int fill_value;
    private GameObject player_object;
    private bool consumed = false;
    private float current_consume_counter;
    private float consume_speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player_object = GameObject.FindWithTag("Player");

        if (weight <= 0) { weight = 10; }
        if (fill_value <= 0) { fill_value = 100; }

        current_consume_counter = 0f;
        consume_speed = 20f * (1f / Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (consumed == true) {
            if (current_consume_counter < consume_speed) {
                transform.position = Vector3.MoveTowards(transform.position, player_object.transform.position, 10f * Time.deltaTime);
                    current_consume_counter += (1f / Time.deltaTime);
                    // Debug.Log("Current Consume Counter: " + current_consume_counter);
                    // Debug.Log("Cosnume Speed: " + consume_speed);
        }
            else {
                Destroy(gameObject);
            }
        }
    }

    public void consumeObstacle() {

        Destroy(gameObject.GetComponent<Collider2D>());
        consumed = true;

    }

    public int getWeight() {
        return weight;
    }

    public int getFillValue() {
        return fill_value;
    }
}
