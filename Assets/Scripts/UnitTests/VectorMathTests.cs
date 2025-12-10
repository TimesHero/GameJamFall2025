using System;
using UnityEngine;

public class VectorMathTests : MonoBehaviour
{

    Vector3 test_float_vec3_a = new Vector3(4f, 2f, 1.5f);
    Vector3 test_float_vec3_b = new Vector3(11.2f, 2.2f, 3f);
 
    Vector3 test_int_vec3_a = new Vector3(4, 8, 16);
    Vector3 test_int_vec3_b = new Vector3(10, 2, 22);

    Vector3 test_float_vec2_a = new Vector2(4f, 2f);
    Vector3 test_float_vec2_b = new Vector2(11.2f, 2.2f);

    Vector3 test_int_vec2_a = new Vector2(4, 8);
    Vector3 test_int_vec2_b = new Vector2(10, 2);

    string runTestStr(string expected_result, string test_result) {

        bool success_state = false;

        if (expected_result == test_result) { success_state = true; }

        return "The expected result: " + expected_result + Environment.NewLine +
                "The attained result: " + test_result + Environment.NewLine +
                "Test succeeded: " + success_state;

    }

    string testVec2ScalarMult(Vector2 input_vector, float input_num) {
         Vector2 expected = new Vector2(input_vector.x * input_num, input_vector.y * input_num);
         Vector2 actual = VectorMath.vector2ScalarMultiply(input_vector, input_num);

        return runTestStr(VectorMath.printVector2(expected), VectorMath.printVector2(actual));
    }

    // Vector2 testVec2ScalarMult(Vector2 input_vector, float input_num) {
    //     return new Vector2(input_vector.x * input_num, input_vector.y * input_num);
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(testVec2ScalarMult(test_int_vec2_a, 4f));
        Debug.Log(testVec2ScalarMult(test_int_vec2_a, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
