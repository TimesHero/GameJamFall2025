using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [SerializeField] private int weight = 100;
    [SerializeField] private int fill_value = 10;
    [SerializeField] private float delta_weight;
    [SerializeField] private float delta_fill_value;

    [SerializeField] private GameObject player;
    private bool consumed = false;
    private float current_consume_counter;
    private float consume_speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // if (weight <= 0) { weight = 1; }
        // if (fill_value <= 0) { fill_value = 1000; }

        // delta_weight = (float)weight / 100;
        // delta_fill_value = (float)delta_fill_value / 100;

        current_consume_counter = 0f;
        consume_speed = 20f * (1f / Time.deltaTime);

        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (consumed == true) {
            if (current_consume_counter < consume_speed) {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 10f * Time.deltaTime);
                    current_consume_counter += (1f / Time.deltaTime);
                    Debug.Log("Current Consume Counter: " + current_consume_counter);
                    Debug.Log("Cosnume Speed: " + consume_speed);
        }
            else {
                Destroy(gameObject);
            }
        }
    }

    public void consumeObstacle() {

        consumed = true;
        Destroy(gameObject);

    }

    public int getWeight() {
        return weight;
    }

    public int getFillValue() {
        return fill_value;
    }

    public float getDeltaWeight() {
        return delta_weight;
    }

    public float getDeltaFillValue() {
        return delta_fill_value;
    }
}
