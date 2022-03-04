using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class automata : Agent
{
    public GameObject comida;
    public int velocidad;
    private Vector3 posOriginal;
    // Start is called before the first frame update
    void Start()
    {
        posOriginal = transform.position;
    }
    public override void OnEpisodeBegin()
    {
        transform.position = posOriginal;

        float posObj = Random.Range(-3.5f, 3.5f);
        if(posObj > -0.5f && posObj < 0.0f)
        {
            posObj = posObj - 1;
        }else if (posObj < 0.5f && posObj > 0.0f)
        {
            posObj = posObj + 1;
        }
        comida.transform.position = new Vector3(posOriginal.x + posObj, 1, posOriginal.z);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(comida.transform.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float movX = actions.ContinuousActions[0];
        //Debug.Log(movX);
        Vector3 Vm = new Vector3(movX, 0, 0);
        transform.position = transform.position + Vm * velocidad * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pared")
        {
            Debug.Log("Pierde");
            AddReward(-1.0f);
        }else if (collision.gameObject.tag == "comida")
        {
            Debug.Log("Gana");
            AddReward(1.0f);
        }

        EndEpisode();
    }
}
