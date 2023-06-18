using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentCarnivorous : Agent
{
    // Vitesse de rotation
    public float TurnSpeed = 300f;

    // Vitesse de déplacement
    public float MoveSpeed = 1f;

    // Le rigidbody de l'agent
    private Rigidbody _rigidbody;

    // La position par défaut de l'agent
    private Vector3 _defaultPosition;
    
    // La rotation par défaut de l'agent
    private Quaternion _defaultRotation;

    // Collecte les observations de l'agent
    private Animal _animal;

    // Collecte les observations de l'agent
    public override void CollectObservations(VectorSensor sensor)
    {
        if (sensor == null)
        {
            return;
        }

        // On ajoute la position de l'agent
        sensor.AddObservation(transform.position);

        // On ajoute la rotation de l'agent
        sensor.AddObservation(transform.rotation);

        // On ajoute la vitesse de l'agent
        sensor.AddObservation(_rigidbody.velocity);

        // On ajoute la soif de l'animal
        sensor.AddObservation(_animal.Thirst);

        // On ajoute la faim de l'animal
        sensor.AddObservation(_animal.Hunger);

    }

    // Lorsque un épisode commence
    public override void OnEpisodeBegin()
    {
        // On remet l'agent à sa position de départ
        transform.position = _defaultPosition;

        // On remet l'agent à sa rotation de départ
        transform.rotation = _defaultRotation;

        // On remet la vitesse de l'agent à 0
        _rigidbody.velocity = Vector3.zero;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // On récupère les actions de l'agent
        var continuousActions = actionBuffers.ContinuousActions;

        // On récupère la rotation de l'agent
        var rotation = transform.rotation.eulerAngles;

        // On récupère la vitesse de l'agent
        var velocity = _rigidbody.velocity;

        // On récupère la direction de l'agent
        var direction = transform.forward;

        // On fait tourner l'agent
        rotation.y += continuousActions[0] * TurnSpeed * Time.fixedDeltaTime;

        // On récupère la direction de l'agent
        direction = Quaternion.Euler(rotation) * direction;

        // On récupère la vitesse de l'agent
        velocity = direction * MoveSpeed;

        // On applique la rotation à l'agent
        transform.rotation = Quaternion.Euler(rotation);

        // On applique la vitesse à l'agent
        _rigidbody.velocity = velocity;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
        } 
        else if (collision.gameObject.CompareTag("Water"))
        {
            if (_animal.Thirst < 80)
            {
                _animal.Drink();
                AddReward(1f);
            }
            else 
            {
                AddReward(-1f);
                EndEpisode();
            }
        }
        else if (collision.gameObject.CompareTag("AnimalVegetarian"))
        {
            if (_animal.Hunger < 95)
            {
                _animal.Eat();
                AddReward(1f);
            }
            else 
            {
                AddReward(-1f);
            }
        }
        else if (collision.gameObject.CompareTag("AnimalCarnivorous"))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // On récupère le rigidbody de l'agent
        _rigidbody = GetComponent<Rigidbody>();    

        // On récupère la position de l'agent
        _defaultPosition = transform.position;

        // On récupère la rotation de l'agent
        _defaultRotation = transform.rotation;

        // On définit le script Animal
        _animal = GetComponent<Animal>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
