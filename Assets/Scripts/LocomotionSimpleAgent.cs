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
    public float speed = 1.0f;
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
        anim.SetFloat("velx", velocity.x);
        
        float shiftMod = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f;
        float y = 0;
        if (velocity.y > 0.5)
            y = shiftMod;
        else
        {
            y = velocity.y;
        }
        anim.SetFloat("vely", y);

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
