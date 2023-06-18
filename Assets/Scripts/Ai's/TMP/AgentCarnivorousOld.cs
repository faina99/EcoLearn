using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentCarnivorousOld : Agent
{
    // Vitesse de rotation de l'agent
    public float TurnSpeed = 180f;

    // Vitesse de déplacement de l'agent
    public float MoveSpeed = 0.7f;
    
    // Position par défaut de l'agent
    public Vector3 _defaultPosition;

    // Le rigidbody de l'agent
    private Rigidbody _agentRb;


    // On récupère les observations de l'agent
    public override void CollectObservations(VectorSensor sensor)
    {
        if (sensor != null)
        {
            // On récupère la vitesse de l'agent
            var localVelocity = transform.InverseTransformDirection(_agentRb.velocity);

            // On ajoute la vitesse de l'agent aux observations
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D))
        {
            continuousActionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            continuousActionsOut[1] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            continuousActionsOut[0] = 2f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            continuousActionsOut[1] = 2f;
        }
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;

    }

    // Lorsque l'action est reçue    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers);
    }

    public override void OnEpisodeBegin()
    {
        // On remet l'agent à sa position initiale
        transform.localPosition = _defaultPosition;

        // On remet la rotation de l'agent à 0
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 0f));

        // On remet la vitesse de l'agent à 0
        _agentRb.velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Animal"))
        {
            SetReward(1f);
        }
        else if (collision.gameObject.CompareTag("Food"))
        {
            // L'agent a marcher sur la nourriture et la détruit
            Destroy(collision.gameObject);
        }
    }

    public void MoveAgent(ActionBuffers actionBuffers)
    {
        // On récupère les actions de l'agent
        var continuousActions = actionBuffers.ContinuousActions;

        // On récupère la rotation de l'agent
        var rotation = transform.rotation.eulerAngles;

        // On récupère la vitesse de l'agent
        var velocity = _agentRb.velocity;

        // On récupère la direction de l'agent
        var direction = transform.forward;

        // On récupère la rotation de l'agent
        rotation.y += continuousActions[0] * TurnSpeed * Time.fixedDeltaTime;

        // On récupère la direction de l'agent
        direction = Quaternion.Euler(rotation) * direction;

        // On récupère la vitesse de l'agent
        velocity = direction * MoveSpeed;

        // On applique la rotation à l'agent
        transform.rotation = Quaternion.Euler(rotation);

        // On applique la vitesse à l'agent
        _agentRb.velocity = velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        // On récupère le rigidbody de l'agent
        _agentRb = GetComponent<Rigidbody>(); 
        _defaultPosition = transform.localPosition;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
