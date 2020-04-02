using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Selector : MonoBehaviour
{

    public GameObject agentParent;
    public GameObject axisAgentParent;
    public float controlSpeed = 5;

    private Vector3 rotUp = new Vector3(90, 0, 0);
    private Vector3 rotDown = new Vector3(-90, 0, 0);

    private Transform xForm = null;
    private Vector3 point = Vector3.zero;

    private bool isSelected(Transform tf)
    {
        return tf.Find("Selected").gameObject.activeSelf;
    }

    private void setSelected(Transform tf, bool sel)
    {
        tf.Find("Selected").gameObject.SetActive(sel);
    }

    void Start()
    {
        point = transform.position;
        // selector.tag = "selector";
    }

    void FixedUpdate()
    {
        Vector3 push = Input.GetAxis("Vertical") * forward() + Input.GetAxis("Horizontal") * right();
        foreach (Transform child in axisAgentParent.transform)
        {
            if (isSelected(child))
                child.position += push * Time.fixedDeltaTime * controlSpeed;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // get top most point of selection
            Physics.Raycast(hit.point + vert(1000), Vector3.down, out hit);
            xForm = hit.transform;
            point = hit.point;
        }


        if (xForm != null && (xForm.CompareTag("Agent") || xForm.CompareTag("Mover")))
        {
            transform.position = xForm.position + vert(4);
            transform.eulerAngles = rotDown;
        }
        else
        {
            transform.position = point;
            transform.eulerAngles = rotUp;
        }
    }

    void LateUpdate()
    {
        if (xForm != null)
        {
            if (Input.GetAxis("Click") > 0)
            {
                if (Input.GetAxis("Ctrl") > 0)
                {
                    if (xForm.CompareTag("Agent") || xForm.CompareTag("Mover"))
                    {
                        setSelected(xForm, true);
                    }
                    else
                    {
                        foreach (Transform child in agentParent.transform)
                        {
                            setSelected(child, false);
                        }
                    }
                    if (xForm.CompareTag("Mover"))
                    {
                        setSelected(xForm, true);
                    }
                    else
                    {
                        foreach (Transform child in axisAgentParent.transform)
                        {
                            setSelected(child, false);
                        }
                    }
                }
                else
                {
                    foreach (Transform child in agentParent.transform)
                    {
                        if (isSelected(child))
                            child.GetComponent<ClickToMove>().setDestination(xForm.gameObject.GetComponent<Collider>().ClosestPointOnBounds(point));
                    }
                }
            }
        }
    }
}
