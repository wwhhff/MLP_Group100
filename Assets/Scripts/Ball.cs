using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public CarAgent carAgent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zone"))
        {
            carAgent.AddReward(10);
            carAgent.EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("wall"))
        {
            //carAgent.AddReward(-10);
            carAgent.EndEpisode();
        }
    }
}
