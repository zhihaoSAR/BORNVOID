using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleMovement : MonoBehaviour {
    int runF = Animator.StringToHash("runFrontal");
    int idle = Animator.StringToHash("Idle");

    //Geting a CharacterController component.
    [SerializeField] CharacterController controller;
    //Getting a gameObject with Mesh Collider component to use for the ground from the editor.
    [SerializeField] GameObject ground;
    //Empty GameObject to use to check the values of the raycast.
    [SerializeField] GameObject raycastChecker;
    //Getting the animator component.
    [SerializeField] Animator animator;
    //Getting the speed of the player from the editor.
    [SerializeField] Stats stats;

    //The camera is Orthographic.
    public bool isOrthographic;
    //Getting the speed of the player from the editor.
    private float speed;
    //Getting the rotation speed of the player from the editor.
    public float rotationSpeed;
    //Bool to check if add Spheres.
    public bool addSpheres;

    float turnSmoothVelocity;
    //Bool to check if the player is moving.
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start() {
        //Getting the animator component.
        animator = GetComponent<Animator>();

        //Add to the GameObject a sphere Object with a radius of 0.1f, and a red color.
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, -1f, 0);
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.GetComponent<Renderer>().material.color = Color.red;

        //Append the sphere to the GameObject.
        sphere.transform.parent = transform;
    }

    private Ray getOrtographicScreenPointToRay(Vector3 screenPoint) {
        //Mouse position in the windows.
        Vector2 relative = new Vector2(screenPoint.x / Screen.width-0.5f, screenPoint.y / Screen.height-0.5f);

        Vector3 worldUnits = relative * Camera.main.orthographicSize * 2f;
        worldUnits *= Camera.main.aspect;

        Vector3 origin = Camera.main.transform.rotation * worldUnits;
        origin += Camera.main.transform.position;
        
        return new Ray(origin, Camera.main.transform.forward);
    }

    private Plane plane = new Plane(Vector3.up, 0);
    // Update is called once per frame
    void Update() {
        speed = stats.speed;
        
        float distance;
        //Set the player is quiet.
        isMoving = false;

        // Check if you right click on the ground.
        if (Input.GetMouseButton(1)) {
            //Raycast depending on the camera type.
            Ray ray;
            if (isOrthographic) {
                ray = getOrtographicScreenPointToRay(Input.mousePosition);
            } else {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            if (plane.Raycast(ray, out distance)) {
                //Get the position of the ground.
                Vector3 worldPosition = ray.GetPoint(distance);
                //Debug.Log(worldPosition);
                Vector3 targetDir = (worldPosition - transform.position);
                targetDir.y = 0f;

                float step = Mathf.Min(speed, targetDir.magnitude) * Time.deltaTime;

                float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //Debug.Log(worldPosition);
                controller.Move(targetDir.normalized * step);

                if (step > 0.05f) {
                    isMoving = true;
                }

                if (addSpheres) {    
                    //Append a green sphere to raycastChecker in the position of hit.
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = worldPosition;
                    //Set the position Y of the sphere to 0.
                    sphere.transform.position = new Vector3(sphere.transform.position.x, 0, sphere.transform.position.z);
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    sphere.GetComponent<Renderer>().material.color = Color.green;
                    sphere.transform.parent = raycastChecker.transform;
                }
            }
        }


        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Check if the player is moving.
        if (isMoving) {
            //Set the animator to moving.
            animator.Play(runF);
        } else {
            //Set the animator to quiet.
            animator.Play(idle);
        }
        //Set the position to Y = 0.
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
}
