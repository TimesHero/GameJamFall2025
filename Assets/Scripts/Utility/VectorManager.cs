/*
 * Project: Game Jam Fall 2025
 * File:    VelocityManagement
 * Description: Utility methods for managing vectors
 *
 * Author:  Lirael "El" Khan
 * Student Number: 301511913
 * Created: 2025-12-10
 *
 * Index:
 *  - Custom Methods
 *
 *
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class VectorManager : MonoBehaviour
{

    public Vector3 getMousCoord() {

        return Input.mousePosition();

    }

    public Vector2 vector2ScalarMultiply(Vector2 input_vector, float scalar_num) {

        Vector2 result_vector = new Vector2(input_vector.x * scalar_num,
                                            input_vector.y * scalar_num);

        return result_vector;
    }

    public Vector3 vector3ScalarMultiply(Vector2 input_vector, float scalar_num) {


        Vector3 result_vector = new Vector2(input_vector.x * scalar_num,
                                            input_vector.y * scalar_num,
                                            input_vector,z * scalar_num);

        return result_vector;
    }

    public Vector3 addToVector3(Vector3 input_vector, int x_inc, int y_inc, int z_inc) {

        Vector3 result_vector = new Vector3(input_vector.x + x_inc,
                                            input_vector.y + y_inx,
                                            input_vector.z + z_inc);

        return result_vector;

    }

    public void addToVector2(Vector2 input_vector, int x_inc, int y_inc) {

    }

    public bool isNumSmaller(int main_num, int reference_num) {

        is_main_smaller = true;

        if (main_num > reference_num) {
            is_main_smaller = false;
        }

        return is_main_smaller;

    }

    /*
    * @Brief: Add two integers with a limit on maximum possible result
    *
    * - Adds two numbers
    * - If sum exceeds the limit we reduce back to limit
    *
    * @Arg: num_a => First integer to be added
    * @Arg: num_b => Second integer to be added
    * @Arg: limit => Max sum we allow (resuce to limit if sum esceeds it)
    *
    * @Return: Number after adding and adjusting for limit
    */
    public int addIntWithLimit(int num_a, int num_b, int limit) {

        int result_num = num_a + num_b;

        if (result_num > limit) {
            result_num = limit;
        }

        return result_num

    }

    /*
    * @Brief: Add two floats with a limit on maximum possible result
    *
    * - Adds two numbers
    * - If sum exceeds the limit we reduce back to limit
    *
    * @Arg: num_a => First number to be added
    * @Arg: num_b => Second number to be added
    * @Arg: limit => Max sum we allow (resuce to limit if sum esceeds it)
    *
    * @Return: Number after adding and adjusting for limit
    */
    public int addFloatWithLimit(float num_a, float num_b, float limit) {

        float result_num = num_a + num_b;

        if (result_num > limit) {
            result_num = limit;
        }

        return result_num

    }

}
