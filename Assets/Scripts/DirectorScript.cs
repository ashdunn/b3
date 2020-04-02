// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DirectorScript : MonoBehaviour
// {

//     private GameObject selected;
//     private Rigidbody selectedMover;
//     private Rigidbody rb;

//     public float movementSpeed = 10f;
//     public float fastMovementSpeed = 100f;
//     public float freeLookSensitivity = 3f;
//     public float zoomSensitivity = 10f;
//     /// Set to true when free looking (on right mouse button).
//     private bool looking = false;

//     // Start is called before the first frame update
//     void Start()
//     {
//         selected = null;
//         selectedMover = null;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             RaycastHit hit;

//             if (Physics.Raycast(ray, out hit/*, 100*/))
//             {

//                 if (hit.transform.tag == "Agent")
//                 {
//                     if (selected != null)
//                         selected.GetComponent<ClickToMove>().Deselect();
//                     selected = hit.transform.gameObject;
//                     selected.GetComponent<ClickToMove>().Select();
//                     print("fksdljfkdsjflkjlkfsjdklfjsdkfljsklfjdskldj");
//                 }

//                 else if (hit.transform.tag == "Mover")
//                 {
//                     rb = hit.transform.gameObject.GetComponent<Rigidbody>();
//                     rb.isKinematic = false;
//                     rb.detectCollisions = true;

//                     selectedMover = rb;



//                 }

//                 else if (selected != null)
//                 {

//                     selected.GetComponent<ClickToMove>().Destination(hit.transform.gameObject.GetComponent<Collider>().ClosestPointOnBounds(hit.point));
//                 }
//             }


//         }

//         if (selectedMover != null)
//         {
//             /*float moveHorizontal = Input.GetAxis("Horizontal");
//             float moveVertical = Input.GetAxis("Vertical");

//             Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
//             rb.AddForce(movement * 3);*/

//             Vector3 position = selectedMover.transform.position;

//             if (Input.GetKeyDown(KeyCode.LeftArrow))
//             {
//                 position.x--;
//                 selectedMover.transform.position = position;
//             }
//             if (Input.GetKeyDown(KeyCode.RightArrow))
//             {
//                 position.x++;
//                 selectedMover.transform.position = position;
//             }
//             if (Input.GetKeyDown(KeyCode.UpArrow))
//             {
//                 position.z++;
//                 selectedMover.transform.position = position;
//             }
//             if (Input.GetKeyDown(KeyCode.DownArrow))
//             {
//                 position.z--;
//                 selectedMover.transform.position = position;
//             }



//         }



//         if (Input.GetKey(KeyCode.A))
//             Camera.main.transform.position = Camera.main.transform.position + (-Camera.main.transform.right * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.D))
//             Camera.main.transform.position = Camera.main.transform.position + (Camera.main.transform.right * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.W))
//             Camera.main.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.S))
//             Camera.main.transform.position = Camera.main.transform.position + (-Camera.main.transform.forward * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.Q))
//             Camera.main.transform.position = Camera.main.transform.position + (Camera.main.transform.up * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.E))
//             Camera.main.transform.position = Camera.main.transform.position + (-Camera.main.transform.up * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.R))
//             Camera.main.transform.position = Camera.main.transform.position + (Vector3.up * movementSpeed * Time.deltaTime);

//         if (Input.GetKey(KeyCode.F))
//             Camera.main.transform.position = Camera.main.transform.position + (-Vector3.up * movementSpeed * Time.deltaTime);

//         if (looking)
//         {
//             float newRotationX = Camera.main.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
//             float newRotationY = Camera.main.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
//             Camera.main.transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
//         }

//         float axis = Input.GetAxis("Mouse ScrollWheel");
//         if (axis != 0)
//         {
//             var zoomSensitivity = this.zoomSensitivity;
//             Camera.main.transform.position = Camera.main.transform.position + Camera.main.transform.forward * axis * zoomSensitivity;
//         }

//         if (Input.GetKeyDown(KeyCode.Mouse1))
//         {
//             StartLooking();
//         }
//         else if (Input.GetKeyUp(KeyCode.Mouse1))
//         {
//             StopLooking();
//         }



//     }

//     void OnDisable()
//     {
//         StopLooking();
//     }

//     public void StartLooking()
//     {
//         looking = true;
//         Cursor.visible = false;
//         Cursor.lockState = CursorLockMode.Locked;
//     }

//     public void StopLooking()
//     {
//         looking = false;
//         Cursor.visible = true;
//         Cursor.lockState = CursorLockMode.None;
//     }
// }
