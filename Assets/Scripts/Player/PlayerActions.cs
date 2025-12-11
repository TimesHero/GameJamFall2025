/*
 * Project: Game Jam Fall 2025
 * File:    PlayerActions
 * Description: Sets up player behaviour and controls (+ behaviour methods)
 *
 * Author:  Lirael "El" Khan
 * Student Number: 301511913
 * Created: 2025-12-09
 *
 * Index:
 *  - Attributes
 *  - Unity Methods
 *      - OnEnable: Set up input event triggers
 *      - OnEnable: Remove input event triggers
 *      - Start: Set up player object variables
 *      - Update: Manage movement and size changes
 *  - Custom Methods
 *      - triggerMove: Handles mouse click input event for movement
 *      - updateCurrentDirection: Updates normalized direction vector based on mouse position
 *      - updateVelocity: update velocity (accounting for acceleration and max velocity)
 *      - updateMaxVelocity: Set max velocity player can reach
 *      - setPlayerSize:Set player sprite and collider size
 *      - increasePlayerSize:Increase player sprite and collider size
 *      - decreasePlayerSize: Decrease player sprite and collider size
 *      - addDamage: Collides with an item and loses size
 *      - consumeItem: Consumes the collided item and gains size
 *      - triggerIFrames: Activates iframes for fixed period with fixes flashing
 *
 * Class:
 *  - Handles movement of player
 *  - Handles size increase/decrease of player
 *  - Handles player changes on special collisions
 *  - Handles player damage reactions
 *
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimalPlayerActions : MonoBehaviour
{

    // Camera needed to generate direction vector
    [SerializeField] private Camera m_main_camera;

    // ATTRIBUTES: PLAYER ENTITY
    [SerializeField] private GameObject m_player_object;
    private Rigidbody2D m_player_body;
    private CircleCollider2D m_player_collider;
    private SpriteRenderer m_player_sprite;

    // ATTRIBUTES: MOVEMENT

    // The max speed of the player IGNORING DIRECTION!
    // Used to calculate the max velocity in each direction the player can have
    [SerializeField] private float m_max_speed = 0f;
    [SerializeField] private float m_max_speed_upper_bound = 0f;
    [SerializeField] private float m_max_speed_lower_bound = 0f;
    // The player accelerates a certain amount each fram
    // until it reaches max_velocity in that axis direction
    [SerializeField] private float m_acceleration = 0f;
    [SerializeField] private float m_max_acceleration = 0f;
    [SerializeField] private float m_min_acceleration = 0f;
    // The current velocity is tracked and modified by acceleration/direction changes
    private Vector2 m_current_velocity;
    // The current velocity is limited by max velocity
    // The max velocity is dictated by current travel direction and max speed
    private Vector2 m_max_velocity;
    // Evaluating mouse position for direction gives a Vector3 to be handled
    private Vector2 m_current_direction;

    // ATTRIBUTES: SIZE

    // The player is constantly shrinking to encourage them to hunt obstacles
    [SerializeField] private float m_max_decay_rate = 0f;
    [SerializeField] private float m_min_decay_rate = 0f;
    [SerializeField] private float m_size_decay_rate = 0f;
    [SerializeField] private float m_loss_size = 0f;

    private float m_current_power;

    // ATTRIBUTES: INPUTS
    [SerializeField] private InputActionReference m_move_confirm;

    // We want to set up event listeners whenever the object is active
    void OnEnable() {
        m_move_confirm.action.started += triggerMove;
    }

    // We disable event triggers when object is disabled
    // This prevents double triggers when the object is enabled again
    void OnDisable() {
        m_move_confirm.action.started -= triggerMove;
    }

    // Key attributes are initialized or corrected
    void Start() {

        // Set default values for the player movement if not set appropriately (i.e. if zero or below)
        if (m_max_speed <= 0f) { m_max_speed = 5f; }
        if (m_max_speed_upper_bound <= 0f) { m_max_speed_upper_bound = 6f; }
        if (m_max_speed_lower_bound <= 0f) { m_max_speed_upper_bound = 3f; }


        m_acceleration = setDefaultValuePerSecond(m_acceleration, 2f);
        m_max_acceleration = setDefaultValuePerSecond(m_max_acceleration, 6f);
        m_min_acceleration = setDefaultValuePerSecond(m_max_acceleration, 2f);

        m_size_decay_rate = setDefaultValuePerSecond(m_size_decay_rate, 0.1f);
        m_max_decay_rate = setDefaultValuePerSecond(m_max_decay_rate, 2f);
        m_min_decay_rate = setDefaultValuePerSecond(m_min_decay_rate, 0.1f);

        m_current_power = 100;

        if (m_loss_size <= 0f) { m_loss_size = 0.4f; }

        m_current_direction = new Vector2(0, 0);
        m_current_velocity = new Vector2(0, 0);

        //Obtain essential components for player
        m_player_body = m_player_object.GetComponent<Rigidbody2D>();
        m_player_collider = m_player_object.GetComponent<CircleCollider2D>();
        m_player_sprite = m_player_object.GetComponent<SpriteRenderer>();


    }

    // Every frame we want to:
    // Adjust velocity for direction change and acceleration
    // Update the force moving the player
    void Update() {

        updateVelocity();
        m_player_body.linearVelocity = m_current_velocity;
        decayPlayerSize();
        m_current_power = convertScaleToPower();
        Debug.Log("New Player Scale: " + VectorMath.printVector3(m_player_object.transform.localScale));
        Debug.Log("New Player Power: " + m_current_power);

        // increasePlayerSize(0.01f);
    }

    void OnCollisionEnter2D(Collision2D obstacle) {
        m_max_velocity = new Vector2(0, 0);
        if (obstacle.gameObject.tag == "Obstacle") {
            if ( obstacle.gameObject.GetComponent<ObstacleManager>().getWeight() > m_current_power ) {
                isDamageState(true);
                m_acceleration = -(m_acceleration/4);
                limitVelocity(m_max_velocity);
                m_size_decay_rate *= 1.5f;
            }
        }
    }

    void OnCollisionExit2D(Collision2D obstacle) {
        m_max_velocity = new Vector2(0, 0);
        if (obstacle.gameObject.tag == "Obstacle") {
            if ( obstacle.gameObject.GetComponent<ObstacleManager>().getWeight() > m_current_power ) {
                isDamageState(false);
                m_acceleration = -(m_acceleration*4);
                limitVelocity(m_max_velocity);
                m_size_decay_rate = (m_size_decay_rate / 1.5f);
            }
        }

    }

    float setDefaultValuePerSecond(float current_value, float default_value) {
        float new_value;
        if (current_value <= 0) {
            new_value = setValuePerSecond(default_value);
        }
        else {
            new_value = setValuePerSecond(current_value);
        }
        return new_value;
    }

    /*
    * @Brief: Handles modifying movement on move confirm imput with mouse
    *
    * - updates direction
    * - updates max velocity
    *
    * @Arg: obj => The input object that calls this function
    *
    * @Return: N/A
    */
    private void triggerMove(InputAction.CallbackContext obj) {

        Debug.Log("recieved click");

        updateCurrentDirection();

        updateMaxVelocity();


    }

    /*
    * @Brief: Update the player direction vector (NORMALIZED)
    *
    * - Updates the player direction based on mouse position relative to player position
    *
    * @Return: N/A
    */
    private void updateCurrentDirection() {

        Vector3 current_mouse_pos = m_main_camera.ScreenToWorldPoint(VectorMath.getMousePos());

        m_current_direction = VectorMath.getDifferenceVector3(current_mouse_pos, transform.position);
        m_current_direction.Normalize();
        // m_current_direction = m_current_direction.Normalize();


        // Debug.Log("Current Direction on Click: " + VectorMath.printVector2(m_current_direction));

    }

    /*
    * @Brief: Handles player speed every frame accounting for acceleration and max velocity
    *
    * - It accounts for when the player is moving in negative/positive coordinate directions
    *
    * @Return: N/A
    */
    private void updateVelocity() {

        // The amount of change in velocity in each coordinate direction
        // Used to decide positiove/negative acceleration and max velocity as well
        float delta_x = m_current_direction.x * m_acceleration;
        float delta_y = m_current_direction.y * m_acceleration;

        m_current_velocity.x += delta_x;
        m_current_velocity.y += delta_y;

        // We must LIMIT the velocity in both axis directions by max velocity in each axis
        // Max velocity can be positive ("north"/"east" movement)
        // Max velocity can be negative ("south"/"west" movement)
        // Velocity in each axis direction is handled seperately (x and y axes, no z axis)

        // If we are accelerating negatively we are capping when we got BELOW negative max speed coord in that axis
        // If we are accelerating positively we are capping when we got ABOVE positive max speed coord in that axis
        // Do this for both x and y axis velocity measurements
        // if (delta_x < 0) {
        //     if (m_current_velocity.x < m_max_velocity.x) { m_current_velocity.x = m_max_velocity.x; }
        // }
        // else if (delta_x > 0) {
        //
        //     if (m_current_velocity.x > m_max_velocity.x) { m_current_velocity.x = m_max_velocity.x; }
        // }
        //
        // if (delta_y < 0) {
        //     if (m_current_velocity.y < m_max_velocity.y) { m_current_velocity.y = m_max_velocity.y; }
        // }
        // else if (delta_y > 0) {
        //
        //     if (m_current_velocity.y > m_max_velocity.y) { m_current_velocity.y = m_max_velocity.y; }
        // }
        limitVelocity(m_max_velocity);

        Debug.Log("Current Velocity: " + VectorMath.printVector2(m_current_velocity));

    }

    /*
    * @Brief: Handles the max velocity vector for the player
    *
    * - The max velocity vector is used to limit the player velocity
    * - This prevents infinite acceleration in a direction
    * - The max velocity accounts for positive and negative directions
    *
    * @Return: N/A
    */
    private void updateMaxVelocity() {

        m_max_velocity.x = m_current_direction.x * m_max_speed;
        m_max_velocity.y = m_current_direction.y * m_max_speed;

    }

    /*
    * @Brief: Set player sprite and collider size
    *
    * @Arg: new_size => Contains new width (x value) and height (y value) for the player
    */
    private void setPlayerSize(Vector3 new_size) {

        m_player_object.transform.localScale += new_size;

    }

    /*
    * @Brief: Increase player sprite and collider size by fixed amount
    *
    * - We want to keep the player "square shaped" so we only use a single float
    * - The float makes an equal width and height increase
    *
    * @Arg: increase_amount => The scalar value to increase width and height by
    */
    private void increasePlayerSize(float increase_amount) {

        // transform.localScale needs a vector so we make a vector using increase amount
        Vector3 size_increase = new Vector3(increase_amount, increase_amount, 0);
        m_player_object.transform.localScale += size_increase;
        // Debug.Log("New Player Size: " + VectorMath.printVector2(m_player_sprite.size));

    }

    /*
    * @Brief: Decrease player sprite and collider size by fixed amount
    *
    * - We want to keep the player "square shaped" so we only use a single float
    * - The float makes an equal width and height decrease
    *
    * @Arg: increase_amount => The scalar value to decrease width and height by
    */
    private void decreasePlayerSize(float decrease_amount) {

        // transform.localScale needs a vector so we make a vector using decrease amount
        Vector3 size_decrease = new Vector3(decrease_amount, decrease_amount, 0);
        m_player_object.transform.localScale -= size_decrease;
        // Debug.Log("New Player Size: " + VectorMath.printVector2(m_player_sprite.size));

    }

    void decayPlayerSize() {

        decreasePlayerSize(m_size_decay_rate);
        limitSize(m_loss_size);
        checkForLoss();


    }

    void checkForLoss() {

        if (m_player_object.transform.localScale.x <= m_loss_size) {
            Debug.Log("The tyrany of the storm is vanquished!");
        }

    }

    public void consumeItem(Collider2D obstacle) {
        m_current_power += obstacle.gameObject.GetComponent<ObstacleManager>().getFillValue();
        increasePlayerSize(convertPowerToScale());
        // obstacle.gameObject.GetComponent<Transform>().position = ;


    }

    private void isDamageState(bool new_state) {

        if (new_state == true) {

            m_player_sprite.color = new Color(0.8f, 0, 0, 1f);

        }
        else {

            m_player_sprite.color = Color.white;

        }

    }

    /*
    * @Brief: Activates iframes for specified duration then deactivates
    *
    * - Activates iframes (disable collisions with enemies)
    * - Flash red desired times
    * - Deactivate iframes
    * - Uses player and enemy layers to toggle collisions on/off
    *
    * @Arg: iframe_duration_sec => Duration to keep iframes active (IN SECONDS)
    * @Arg: iframe_opacity => The player sprite opacity while iframes are active
    * @Arg: total_flashes => Times player sprite flashes during iframes
    * @Arg: player_layer => Layer used for player
    * @Arg: enemy_layer => Layer used for enemies
    *
    * @WARNING: Enemies and Player MUST be on different layers
    * @WARNING: USES yeild -> WaitForSeconds (Requires subroutine)
    *
    * @Return: N/A
    */
    private IEnumerator triggerIFrames(int iframe_duration_sec = 3,
                                        float iframe_opacity = 0.6f, int total_flashes = 1,
                                        int player_layer = 9, int enemy_layer = 10) {

        // Disable then Enable Collisions between layers
        // Player and Enemy layers must be separate
        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, true);

        // If we need N total flashes
        // We need iframe_duration_sec/N for each (to make them equal duration)
        float flash_duration = ((float) iframe_duration_sec)/((float) total_flashes);
        // We want it to flash red then normal EACH for half the duration of a flash
        float half_flash_duration = flash_duration/2;

        // We Make the model non-opaque red then white again to cause a flash
        for (int current_flash = 0; current_flash < total_flashes; current_flash++) {

            // Flash Mechansism
            // (1) Add sprite red tint
            // (2) wait half flash
            // (3)turn sprite normal color
            // (4) wait half flash
            m_player_sprite.color = new Color(1, 0, 0, iframe_opacity);
            yield return new WaitForSeconds(half_flash_duration);
            m_player_sprite.color = Color.white;
            yield return new WaitForSeconds(half_flash_duration);

        }

        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, false);

    }

    void limitVelocity(Vector2 limit) {


        // Direction Component Positive => Moving "North"/"East"
        // Moving "North"/"East" => Velocity going "up" in direction
        // Velocity going "up" in direction => Lock it from going above limit

        // Direction Component Negative => Moving "South"/"West"
        // Moving "South"/"West" => Velocity "lowering" in direction
        // Velocity "lowering" in direction => Lock it from going below limit

        if (m_current_direction.x < 0) {
            if (m_current_velocity.x < limit.x) { m_current_velocity.x = limit.x; }
        }
        else if (m_current_direction.x > 0) {

            if (m_current_velocity.x > limit.x) { m_current_velocity.x = limit.x; }
        }

        if (m_current_direction.y < 0) {
            if (m_current_velocity.y < limit.y) { m_current_velocity.y = limit.y; }
        }
        else if (m_current_direction.y > 0) {

            if (m_current_velocity.y > limit.y) { m_current_velocity.y = limit.y; }
        }

    }

    float setValuePerSecond(float initial_value) {
        return initial_value * Time.deltaTime;
    }

    void setAcceleration(float new_accel_value) {
        m_acceleration = new_accel_value;
    }

    void increaseAcceleration(float accel_inc_value) {
        m_acceleration += accel_inc_value;
    }

    void decreaseAcceleration(float accel_dec_value) {
        m_acceleration -= accel_dec_value;
    }

    void setDecayRate(float new_decay_value) {
        m_size_decay_rate = new_decay_value;
    }

    void increaseDecayRate(float decay_inc_value) {
        m_size_decay_rate += setValuePerSecond(decay_inc_value);
    }

    void decreaseDecayRate(float decay_dec_value) {
        m_size_decay_rate -= setValuePerSecond(decay_dec_value);
    }

    void limitSize(float limit) {
        if (m_player_object.transform.localScale.x <= limit) {
            m_player_object.transform.localScale = new Vector3(limit, limit, m_player_object.transform.localScale.z);
        }
    }

    float limitInBounds(float current_value, float lower_limit, float upper_limit) {

        float result_value = current_value;


        if (current_value < lower_limit) {
            result_value = lower_limit;
        }

        if (current_value >= upper_limit) {
            result_value = upper_limit;
        }

        return result_value;

    }

    void limitDecayRate() {

        m_size_decay_rate = limitInBounds(m_size_decay_rate, m_min_decay_rate, m_max_decay_rate);

    }

    void limitMaxSpeed() {

        m_max_speed = limitInBounds(m_max_speed, m_max_speed_lower_bound, m_max_speed_upper_bound);

    }

    void limitAcceleration() {

        m_max_speed = limitInBounds(m_acceleration, m_min_acceleration, m_max_acceleration);

    }

    // void setNewPowerDecayRate() {
    //      m_power_decay_rate = ( (m_player_body.transform.localScale.x - m_loss_size) / m_size_decay_rate );
    // }

    float convertScaleToPower() {

        return ( ( m_player_object.transform.localScale.x - m_loss_size ) * 100 );

    }

    float convertPowerToScale() {

        return ((m_current_power / 100) + m_loss_size);

    }

}
