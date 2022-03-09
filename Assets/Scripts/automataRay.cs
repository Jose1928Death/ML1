using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class automataRay : Agent
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

    public override void OnActionReceived(ActionBuffers actions)
    {
        int Mover = actions.DiscreteActions[0];
        int Girar = actions.DiscreteActions[1];

        int m = 0;
        int g = 0;

        if (Mover == 1)
        {
            m = 1;
        }
        else if (Mover == 2)
        {
            m = -1;
        }

        if (Girar == 1)
        {
            g = 1;
        }
        else if (Girar == 2)
        {
            g = -1;
        }

        if (m != 0)
        {
            transform.position = transform.position + transform.forward * m * velocidad * Time.deltaTime;
        }

        if (g != 0)
        {
            transform.Rotate(Vector3.up, g);
        }
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