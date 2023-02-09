using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using VehicleBehaviour;
using System.IO.Compression;

public class CarAgent : Agent
{
    public WheelVehicle wheelVehicle;
    public Rigidbody rb;

    public Transform ball;
    public Rigidbody ball_rb;
    public Transform zone;

    public Lidar lidar;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-30.0f, 30.0f), 0, Random.Range(-30.0f, 30.0f));
        transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 ballpos = new Vector3(Random.Range(-30.0f, 30.0f), 1.5f, Random.Range(-30.0f, 30.0f));
        ball.transform.localPosition = ballpos;
        ball_rb.velocity = Vector3.zero;
        ball_rb.angularVelocity = Vector3.zero;

        Vector3 zonepos = new Vector3(Random.Range(-30.0f, 30.0f), 0, Random.Range(-30.0f, 30.0f));
        while(Vector3.Distance(ballpos, zonepos) < 10.0f)
        {
            zonepos = new Vector3(Random.Range(-30.0f, 30.0f), 0, Random.Range(-30.0f, 30.0f));
        }
        zone.transform.localPosition = zonepos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(transform.InverseTransformDirection(rb.velocity).z);
        sensor.AddObservation(transform.InverseTransformPoint(ball.position).x);
        sensor.AddObservation(transform.InverseTransformPoint(ball.position).z);
        sensor.AddObservation(transform.InverseTransformPoint(zone.position).x);
        sensor.AddObservation(transform.InverseTransformPoint(zone.position).z);

        for (int i = 0; i < lidar.rays; i++)
        {
            sensor.AddObservation(lidar.hits[i].distance);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Throttle");
        continuousActions[1] = Input.GetAxis("Steering");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        wheelVehicle.throttle = actions.ContinuousActions[0];
        wheelVehicle.steeringInput = actions.ContinuousActions[1];

        if (rb.velocity.magnitude < 0.1f)
        {
            AddReward(-rb.velocity.magnitude * 0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ball"))
        {
            AddReward(3f);
        }
    }
}
