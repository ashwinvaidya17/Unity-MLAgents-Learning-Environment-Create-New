using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    Rigidbody rBody;
    public Transform Target;
    public float Speed = 10;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        
    }

    public override void AgentReset()
    {
        if (this.transform.position.y < 0) //if agent falls
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0,0.5f, 0);
        }

        // Move target to new spot
        Target.position = new Vector3(Random.value * 8 - 4,
                                      0.5f,
                                      Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Target.position);
        AddVectorObs(this.transform.position);
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //Action, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * Speed);

        //rewards
        float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        //Fell off platform
        if(this.transform.position.y < 0)
        {
            Done();
        }
    }

}
