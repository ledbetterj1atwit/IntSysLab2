using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using System;

public class MazeAgent : Agent
{
    [Header("Specific to Maze")]
    public GameObject target;
    public GameObject start;
    public bool debug;

    Rigidbody m_rb;
    Vector3 m_lastPos;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        m_rb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(gameObject.transform.position);
        sensor.AddObservation(target.transform.position);

        RaycastHit centerToWall;
        m_rb.SweepTest(Vector3.forward, out centerToWall, Mathf.Infinity, QueryTriggerInteraction.Ignore);
        sensor.AddObservation(centerToWall.distance);

        RaycastHit leftToWall;
        m_rb.SweepTest(new Vector3(-0.70710678f, 0f, -0.70710678f), out leftToWall, Mathf.Infinity, QueryTriggerInteraction.Ignore);
        sensor.AddObservation(leftToWall.distance);

        RaycastHit rightToWall;
        m_rb.SweepTest(new Vector3(0.70710678f, 0f, 0.70710678f), out rightToWall, Mathf.Infinity, QueryTriggerInteraction.Ignore);
        sensor.AddObservation(rightToWall.distance);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var forceZ = Mathf.Clamp(actionBuffers.ContinuousActions[0], -0.1f, 1f)/5f; 
        var rotY = 5f * Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);


        gameObject.transform.Rotate(Vector3.up * rotY);
        gameObject.transform.Translate(Vector3.forward * forceZ);

        AddReward(((gameObject.transform.position - target.transform.position).magnitude * -0.01f) - 0.01f);
        //AddReward(-0.01f);

        m_lastPos = gameObject.transform.position;

    }

    public void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("wall")) AddReward(-0.3f);        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Hit Target");
            AddReward(10000f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        SetReward(0f);
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = start.transform.position;
        if (debug) Debug.Log("Episode began.");
    }
}
