using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // if we end up with more than one level we'll need to make it persist in some way but this works for now
    int m_objects_consumed;
    // add more of these for each different obstacle type we get
    int m_obstacle1_consumed;
    float m_largest_size;
    float m_time_survived;
    float m_starting_time;

    void Start()
    {
        // grabs time at start of scene to offset time in menu
        m_starting_time = Time.time;
    }

    public void setConsumeStats(int[] count_array)
    {
        m_objects_consumed = count_array[0];
        m_obstacle1_consumed = count_array[1];
    }
    public void setLargestSize(float size)
    {
        m_largest_size = size;
    }
    public void setTimeSurvived()
    {
        m_time_survived = Time.time - m_starting_time;
    }
}
