using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class automata3D : Agent
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

        float posObjX = Random.Range(-5.0f, 5.0f);
        if (posObjX > -0.5f && posObjX < 0.0f)
        {
            posObjX = posObjX - 1;
        }
        else if (posObjX < 0.5f && posObjX > 0.0f)
        {
            posObjX = posObjX + 1;
        }

        float posObjZ = Random.Range(-5.0f, 5.0f);
        if (posObjZ > -0.5f && posObjZ < 0.0f)
        {
            posObjZ = posObjZ - 1;
        }
        else if (posObjZ < 0.5f && posObjZ > 0.0f)
        {
            posObjZ = posObjZ + 1;
        }
        comida.transform.position = new Vector3(posOriginal.x + posObjX, 1, posOriginal.z + posObjZ);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(comida.transform.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int EjeX = actions.DiscreteActions[0];
        int EjeZ = actions.DiscreteActions[1];

        int x = 0;
        int z = 0;

        if (EjeX == 1)
        {
            x = 1;
        }
        else if(EjeX==2)
        {
            x = -1;
        }

        if (EjeZ == 1)
        {
            z = 1;
        }
        else if (EjeZ == 2)
        {
            z = -1;
        }

        Vector3 Vm = new Vector3(x, 0, z);
        transform.position = transform.position + Vm.normalized * velocidad * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "pared")
        {
            Debug.Log("Pierde");
            AddReward(-1.0f);
        }
        else if (collision.gameObject.tag == "comida")
        {
            Debug.Log("Gana");
            AddReward(1.0f);
        }

        EndEpisode();
    }
}