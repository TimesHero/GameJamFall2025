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
 *      - getMouseCoord         :   Get Vector3 coordinates of mouse on screen
 *      - printVector2          :   Get Vector2 string representation
 *      - printVector3          :   Get Vector3 string representation
 *      - vector2ScalarMultiply :   Multiplies Vector2 with scalar value
 *      - vector3ScalarMultiply :   Multiplies Vector3 with scalar value
 *      - addToVector3          :   Add a float increment to each Vector3 value
 *      - addToVector2          :   Add a float increment to each Vector2 value
 *      - isNumSmaller          :   Check if a number is smaller than another reference number
 *      - addIntWithLimit       :   Adds two int numbers but limits sum to a max amount
 *      - addFloatWithLimit     :   Adds two float numbers but limits sum to a max amount
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class VectorMath : MonoBehaviour
{

    /*
    * @Brief: Gets the current mouse position on screen
    *
    *
    * @Return: (Vector3) Coordinates of the mouse
    */
    static public Vector3 getMouseCoord() {

        return Input.mousePosition;

    }

    /*
    * @Brief: Prints the vector in string form
    *
    * @Arg: input_vector => Vector to be printed
    *
    * @Return: string representation of vector
    */
    static public string printVector2(Vector2 input_vector) {

        string vector_string = "[" + input_vector.x.ToString() +
                                input_vector.y.ToString() + "]";

        return vector_string;

    }

    /*
    * @Brief: Prints the vector in string form
    *
    * @Arg: input_vector => Vector to be printed
    *
    * @Return: string representation of vector
    */
    static public string printVector3(Vector3 input_vector) {

        string vector_string = "[" + input_vector.x.ToString() +
                                input_vector.y.ToString() +
                                input_vector.z.ToString() + "]";

        return vector_string;

    }

    /*
    * @Brief: Multiplies a vector2 by a scalar value
    *
    * @Arg: input_vector => Vector being multiplied with the scalar
    * @Arg: scalar_num => Scalar number being used to multiply
    *
    * @Return: Resulting multiplied vector
    */
    static public Vector2 vector2ScalarMultiply(Vector2 input_vector, float scalar_num) {

        Vector2 result_vector = new Vector2(input_vector.x * scalar_num,
                                            input_vector.y * scalar_num);

        return result_vector;
    }

    /*
    * @Brief: Multiplies a vector3 by a scalar value
    *
    * @Arg: input_vector => Vector being multiplied with the scalar
    * @Arg: scalar_num => Scalar number being used to multiply
    *
    * @Return: Resulting multiplied vector
    */
    static public Vector3 vector3ScalarMultiply(Vector3 input_vector, float scalar_num) {


        Vector3 result_vector = new Vector3(input_vector.x * scalar_num,
                                            input_vector.y * scalar_num,
                                            input_vector.z * scalar_num);

        return result_vector;
    }

    /*
    * @Brief: Increases each value in vector3 by desired amount
    *
    * @Arg: input_vector => The original base vector
    * @Arg: x_inc => Value to increase the base vector x coordinate
    * @Arg: y_inc => Value to increase the base vector y coordinate
    * @Arg: z_inc => Value to increase the base vector z coordinate
    *
    * @Return: Resulting summed vector
    */
    static public Vector3 addToVector3(Vector3 input_vector, int x_inc, int y_inc, int z_inc) {

        Vector3 result_vector = new Vector3(input_vector.x + x_inc,
                                            input_vector.y + y_inc,
                                            input_vector.z + z_inc);

        return result_vector;

    }
    /*
    * @Brief: Increases each value in vector2 by desired amount
    *
    * @Arg: input_vector => The original base vector
    * @Arg: x_inc => Value to increase the base vector x coordinate
    * @Arg: y_inc => Value to increase the base vector y coordinate
    *
    * @Return: Resulting summed vector
    */
    static public void addToVector2(Vector2 input_vector, int x_inc, int y_inc) {

    }

    /*
    * @Brief: Check if a number is smaller than another
    *
    * @Arg: main_num => The number being checked for smaller status
    * @Arg: reference_num => The number being compared to
    *
    * @Return: (bool) Whether the main number is smaller than the reference number
    * or not
    */
    static public bool isNumSmaller(int main_num, int reference_num) {

        bool is_main_smaller = true;

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
    static public int addIntWithLimit(int num_a, int num_b, int limit) {

        int result_num = num_a + num_b;

        if (result_num > limit) {
            result_num = limit;
        }

        return result_num;

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
    static public float addFloatWithLimit(float num_a, float num_b, float limit) {

        float result_num = num_a + num_b;

        if (result_num > limit) {
            result_num = limit;
        }

        return result_num;

    }

    /*
    * @Brief: Multiply two floats with a limit on maximum possible result
    *
    * - Multiplies two numbers
    * - If sum exceeds the limit we reduce back to limit
    *
    * @Arg: num_a => First number to be multiplied
    * @Arg: num_b => Second number to be multiplied
    * @Arg: limit => Max product we allow (resuce to limit if product esceeds it)
    *
    * @Return: Number after adding and adjusting for limit
    */
    static public float multFloatWithLimit(float num_a, float num_b, float limit) {

        float result_num = num_a * num_b;

        if (result_num > limit) {
            result_num = limit;
        }

        return result_num;

    }

}
