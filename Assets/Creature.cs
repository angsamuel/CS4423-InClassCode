using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Creature : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 0f;
    [SerializeField] float jumpForce = 10;
    [SerializeField] int health = 3;
    [SerializeField] int stamina = 3;

    public enum CreatureMovementType { tf, physics };
    [SerializeField] CreatureMovementType movementType = CreatureMovementType.tf;
    public enum CreaturePerspective { topDown, sideScroll };
    [SerializeField] CreaturePerspective perspectiveType = CreaturePerspective.topDown;

    [Header("Physics")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpOffset = -.5f;
    [SerializeField] float jumpRadius = .25f;

    [Header("Flavor")]
    [SerializeField] string creatureName = "Meepis";
    public GameObject body;
    [SerializeField] private List<AnimationStateChanger> animationStateChangers;

    [Header("Tracked Data")]
    [SerializeField] Vector3 homePosition = Vector3.zero;
    [SerializeField] CreatureSO creatureSO;


    Rigidbody2D rb;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(health);

    }



    // Update is called once per frame
    void Update()
    {
        if(creatureSO != null){
            creatureSO.health = health;
            creatureSO.stamina = stamina;
        }
    }

    void FixedUpdate(){

    }

    public void MoveCreature(Vector3 direction)
    {

        if (movementType == CreatureMovementType.tf)
        {
            MoveCreatureTransform(direction);
        }
        else if (movementType == CreatureMovementType.physics)
        {
            MoveCreatureRb(direction);
        }

        //set animation
        if(direction != Vector3.zero){
            foreach(AnimationStateChanger asc in animationStateChangers){
                asc.ChangeAnimationState("Walk",speed);
            }
        }else{
            foreach(AnimationStateChanger asc in animationStateChangers){
                asc.ChangeAnimationState("Idle");
            }
        }
    }

    public void MoveCreatureToward(Vector3 target){
        Vector3 direction = target - transform.position;
        MoveCreature(direction.normalized);
    }

    public void Stop(){
        MoveCreature(Vector3.zero);
    }

    public void MoveCreatureRb(Vector3 direction)
    {
        Vector3 currentVelocity = Vector3.zero;
        if(perspectiveType == CreaturePerspective.sideScroll){
            currentVelocity = new Vector3(0, rb.velocity.y, 0);
            direction.y = 0;
        }

        rb.velocity = (currentVelocity) + (direction * speed);
        if(rb.velocity.x < 0){
            body.transform.localScale = new Vector3(-1,1,1);
        }else if(rb.velocity.x > 0){
            body.transform.localScale = new Vector3(1,1,1);
        }
        //rb.AddForce(direction * speed);
        //rb.MovePosition(transform.position + (direction * speed * Time.deltaTime))
    }

    public void MoveCreatureTransform(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * speed;
    }

    public void Jump()
    {
        if(Physics2D.OverlapCircleAll(transform.position + new Vector3(0,jumpOffset,0),jumpRadius,groundMask).Length > 0){
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

    }

}
