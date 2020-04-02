using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;


    private bool _traversingLink;
    private UnityEngine.AI.OffMeshLinkData _currLink;

    public Slider slider;
    public Toggle checkbox;
    public float speed = 5.0f;
    public bool run = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Don’t update position automatically
        agent.updatePosition = false;

        //agent.autoTraverseOffMeshLink = false;
    }

    public void OnValueChanged(float newValue)
    {
        speed = newValue;
        agent.speed = speed * 5;
    }

    public void OnRunValueChanged(bool check)
    {
        run = check;
        Debug.Log(check);
    }

    void Update()
    {
        UnityEngine.AI.NavMeshHit hit;
        int JumpMask = 4; // IDK why, thought it should be 2

        // Check all areas one length unit ahead.
        if (!agent.SamplePathPosition(UnityEngine.AI.NavMesh.AllAreas, 0.0F, out hit))
            if ((hit.mask & JumpMask) != 0)
            {
                // Water detected along the path...
                Debug.Log("OffMeshLink");
                anim.SetFloat("Jump4Blend", 1);
            }
            else
            {
                anim.SetFloat("Jump4Blend", 0);
            }

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        float vx =  velocity.x;
        float vy =  velocity.y;
        if (!run)
        {
            // Dump Speed down
            if(vx > .3f)
            {
                vx = .3f;
            }
            if (vy > .3f)
            {
                vy = .3f;
            }
            agent.speed = speed * 1.5f;
        }
        if (vx < 0.001 | vy < 0.001)
        {
            anim.SetFloat("velx", vx);
            anim.SetFloat("vely", vy);
        }
        else
        {
            if (!run)
            {
                anim.SetFloat("velx", vx / Mathf.Abs(vx)  * speed * .3f);
                anim.SetFloat("vely", vy / Mathf.Abs(vy)  * speed * .3f);
            }
            else
            {
                anim.SetFloat("velx", vx / Mathf.Abs(vx)  * speed);
                anim.SetFloat("vely", vy / Mathf.Abs(vy)  * speed);
            }

        }

        Debug.Log(vx);
        Debug.Log(vy); // At some time speed will wrongly made as 1.5???


        if (GetComponent<LookAt>() != null)
        {
            LookAt lookAt = GetComponent<LookAt>();
            if (lookAt)
                lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
        }

        if (worldDeltaPosition.magnitude > agent.radius)
            transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        //}
    }

    void OnAnimatorMove()
    {
        // Update position based on animation movement using navigation surface height
        //Vector3 position = anim.rootPosition;
        //position.y = agent.nextPosition.y;
        //transform.position = position;
        transform.position = agent.nextPosition;
    }
}
