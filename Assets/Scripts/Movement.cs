using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] protected float speed = 5;
    [SerializeField] AnimationStateChanger animationStateChanger;
    public Transform body;
    protected Rigidbody2D rb;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Start(){

    }

    public void MoveRB(Vector3 vel){
        rb.velocity = vel * speed;
        if(vel.magnitude > 0){
            animationStateChanger?.ChangeAnimationState("Walk",speed/5);

            if(vel.x > 0){
                body.localScale = new Vector3(1,1,1);
            }else if(vel.x < 0){
                body.localScale = new Vector3(-1,1,1);
            }

        }else{
            animationStateChanger?.ChangeAnimationState("Idle");
        }
    }

    public void MoveToward(Vector3 targetPosition){
        Vector3 direction = targetPosition - transform.position;
        direction = Vector3.Normalize(direction);
        MoveRB(direction);
    }

    public void Stop(){
        MoveRB(Vector3.zero);
    }

    // public void StepMove(Vector3 direction){
    //     transform.position += direction;
    // }

    // public void MoveTransform(Vector3 vel){
    //     transform.position += vel * speed * Time.deltaTime;
    // }

}
