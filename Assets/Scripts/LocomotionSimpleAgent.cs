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
    public float speed = 10.0f;

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
        /*
        if (agent.isOnOffMeshLink)
        {
            if (!_traversingLink)
            {
                //This is done only once. The animation's progress will determine link traversal.
                GetComponent<Animation>().CrossFade("M_Jump_Walking", 0.1f, PlayMode.StopAll);
                //cache current link
                _currLink = agent.currentOffMeshLinkData;
                //start traversing
                _traversingLink = true;
            }

            //lerp from link start to link end in time to animation
            Vector3 tlerp = GetComponent<Animation>()["M_Jump_Walking"].normalizedTime;
            //straight line from startlink to endlink
            Vector3 newPos = Vector3.Lerp(_currLink.startPos, _currLink.endPos, tlerp);
            //add the 'hop'
            newPos.y += 2f * Mathf.Sin(Mathf.PI * tlerp);
            //Update transform position
            transform.position = newPos;

            // when the animation is stopped, we've reached the other side. Don't use looping animations with this control setup
            if (!GetComponent<Animation>().isPlaying)
            {
                //make sure the player is right on the end link
                transform.position = _currLink.endPos;
                //internal logic reset
                _traversingLink = false;
                //Tell unity we have traversed the link
                agent.CompleteOffMeshLink();
                //Resume normal navmesh behaviour
                agent.Resume();
            }
        }

        else
        {*/
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
        // anim.SetFloat("velx", vx);
        // anim.SetFloat("vely", vy);
        if (vx < 0.001 | vy < 0.001)
        {
            anim.SetFloat("velx", vx);
            anim.SetFloat("vely", vy);
        }
        else
        {
            anim.SetFloat("velx", vx / Mathf.Abs(vx)  * speed);
            anim.SetFloat("vely", vy / Mathf.Abs(vy)  * speed);
        }


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
