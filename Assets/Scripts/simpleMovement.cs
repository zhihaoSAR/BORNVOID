using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleMovement : MonoBehaviour {
    private int runF;
    private int idle;
    private int death;
    private int attacking;
    private int dash;

    //Geting a CharacterController component.
    [SerializeField] CharacterController controller;
    //Empty GameObject to use to check the values of the raycast.
    //[SerializeField] GameObject raycastChecker;
    //Getting the animator component.
    [SerializeField] Animator animator;
    //Getting the speed of the player from the editor.
    [SerializeField] Stats stats;
    //The game Object of the Projectile Prefab.
    [SerializeField] GameObject projectile;
    //The game Object of the weapon.
    [SerializeField] GameObject weapon;


    //The camera is Orthographic.
    public bool isOrthographic;
    //Getting the speed of the player from the editor.
    private float speed;
    //Getting the rotation speed of the player from the editor.
    public float rotationSpeed;

    float turnSmoothVelocity;
    //Bool to check if the player is moving.
    private bool isMoving = false;
    //Bool to check if the player is attacking.
    private bool isAttacking = false;
    private bool canAttack = true;
    //Is dead bool.
    public bool isDead = false;


    //habilidades
    protected BasicAttack swordSkill;

    //Vector3 to store the dash direction.
    private Vector3 dashDirection;


    // Start is called before the first frame update
    void Start() {
        //Getting the animator component.
        animator = GetComponent<Animator>();
        runF = Animator.StringToHash("RunFrontal");
        idle = Animator.StringToHash("Idle");
        death = Animator.StringToHash("Death");
        attacking = Animator.StringToHash("Attacking");
        dash = Animator.StringToHash("Dash");
        //Get the weapon a swordSkill script, with the serialized field.
        swordSkill = weapon.GetComponent<BasicAttack>();
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

    private Vector3 getObjectiveDirection(Ray ray) {
        float distance = 0;
        Vector3 direction = Vector3.zero;
        if (plane.Raycast(ray, out distance)) {
            //Get the position of the ground.
            Vector3 worldPosition = ray.GetPoint(distance);
            //Debug.Log(worldPosition);
            Vector3 targetDir = (worldPosition - transform.position);
            targetDir.y = 0f;
            direction = targetDir.normalized;
        }
        return direction;
    }


    //Get the objective rotation of the player.
    private Quaternion getObjectiveRotation(Vector3 objectivePoint) {
        float targetAngle = Mathf.Atan2(objectivePoint.x, objectivePoint.z) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, targetAngle, 0f);
    }

    private Plane plane = new Plane(Vector3.up, 0);
    // Update is called once per frame
    void Update() {
        
        //If the player is dead, do nothing.
        if (isDead) {
            return;
        }

        if (!stats.inDash && stats.dashTimer == 0) {
            //Check if the player do a dash.
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                stats.inDash = true;
                stats.dashLeftTime = stats.dashTime;
                stats.dashTimer = stats.dashCooldown;

                //Calculate the direction of the dash.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 direction = getObjectiveDirection(ray);
                Quaternion rotation = getObjectiveRotation(direction);
                transform.rotation = rotation;
                float step = speed * stats.dashSpeedMultiplier;
                dashDirection = direction * step;
                controller.Move(dashDirection * Time.deltaTime);
                //isMoving = true;
            }
        } else {
            //If the player is dashing, do nothing.
            if (stats.dashLeftTime > 0) {
                controller.Move(dashDirection * Time.deltaTime);
                isMoving = true;
                stats.dashLeftTime -= Time.deltaTime;
                if (stats.dashLeftTime <= 0) {
                    stats.dashLeftTime = 0;
                    stats.inDash = false;
                }
            } else if (stats.dashTimer > 0) {
                stats.dashTimer -= Time.deltaTime;
                if (stats.dashTimer <= 0) {
                    stats.dashTimer = 0;
                }
            }
        }

        if(!stats.inDash) {
            speed = stats.speed;
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

                Vector3 targetDir = getObjectiveDirection(ray);

                float step = speed * Time.deltaTime;

                float targetAngle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                controller.Move(targetDir.normalized * step);

                if (step > 0.05f) {
                    isMoving = true;
                }
                
            } else {
                //If not right click, check the pressed keys of AWSD.
                //Set a Vector3 to add the directions of the keys.
                Vector3 move = new Vector3(0, 0, 0);
                //Check if the key S is pressed.
                if (Input.GetKey(KeyCode.S)) {
                    move += new Vector3(-1, 0, -1);
                }
                //Check if the key D is pressed.
                if (Input.GetKey(KeyCode.D)) {
                    move += new Vector3(1, 0, -1);
                }
                //Check if the key W is pressed.
                if (Input.GetKey(KeyCode.W)) {
                    move += new Vector3(1, 0, 1);
                }
                //Check if the key A is pressed.
                if (Input.GetKey(KeyCode.A)) {
                    move += new Vector3(-1, 0, 1);
                }
                if (move.magnitude > 0) {
                    //Set the player is moving.
                    float step = speed * Time.deltaTime;

                    float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed);

                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    controller.Move(move.normalized * step);

                    isMoving = true;
                }
            }

            //Check if the player is attacking.
            if (Input.GetMouseButton(0) && canAttack) {
                canAttack = false;
                isAttacking = true;
                //Active the collider of the weapon.
                weapon.GetComponent<BoxCollider>().enabled = true;
                swordSkill.useSkill(damage: stats.damage);

                //Set the stats of the attack.
                stats.attackTime = stats.durationAttack;
                stats.attackTimeP = stats.attackTimer;

                //Rotate the player to the direction of the mouse.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 targetDir = getObjectiveDirection(ray);
                Quaternion rotation = getObjectiveRotation(targetDir);
                transform.rotation = rotation;
            } else {
                //reduce the attack time.
                if (stats.attackTime > 0) {
                    stats.attackTime -= Time.deltaTime;
                    if (stats.attackTime <= 0) {
                        stats.attackTime = 0;
                        //Disable the collider of the weapon.
                        weapon.GetComponent<BoxCollider>().enabled = false;
                        isAttacking = false;
                        swordSkill.cancelSkill();
                    }
                }
                if (stats.attackTimeP > 0) {
                    stats.attackTimeP -= Time.deltaTime;
                    if (stats.attackTimeP <= 0) {
                        stats.attackTimeP = 0;
                        canAttack = true;
                    }
                }
            }
            if (!Input.GetMouseButton(0) && Input.GetKeyDown(KeyCode.Space)) {
                //Rotate the player to the direction of the mouse.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 targetDir = getObjectiveDirection(ray);
                Quaternion rotation = getObjectiveRotation(targetDir);
                transform.rotation = rotation;


                //Create a new instance of the projectile.
                GameObject projectileInst = (GameObject)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
                //Set the projectile direction.
                ProjectileSkill projectileSkill = projectileInst.GetComponent<ProjectileSkill>();
                projectileSkill.direction = transform.forward;
                //Set the projectile damage.
                projectileSkill.damage = stats.projectileDamage;

                Debug.Log("Shooting");
            }
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stats.inDash) {
            animator.Play(dash);
        } else {
            if (isAttacking) {
                //Set the animator to Attack Animations.
                animator.Play(attacking);
            } else {
                if (isMoving) {
                    //Set the animator to Walk Animations.
                    animator.Play(runF);
                } else {
                    //Set the animator to Idle Animations.
                    animator.Play(idle);
                }
            }
        }

        //get the scale.y of the player.
        //Set the position to Y = 0.
        transform.position = new Vector3(transform.position.x, transform.localScale.y , transform.position.z);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("enemy_atk")) {
            // You are attacked. Extract the damage from the enemy skill
            //Totalmente provisional, podemos hacer un append de las propias stats en el collider dede el script de skill.
            GameObject enemy = other.gameObject;
            //Get the enemy skill script.
            Skill enemySkill = enemy.GetComponent<Skill>();
            // Get the damage from the enemy skill.
            float damage = enemySkill.Damage;
            stats.getDamage(damage);
            Debug.Log("You are attacked. Health: " + stats.health);
            //Check if the player is dead.
            if (stats.isDead()) {
                Debug.Log("You are dead.");
                //Trigger the function.
                deadState();
            }

        }
    }

    protected void deadState() {
        //Trigger the function.
        animator.Play(death);
        //Set the player is dead.
        isDead = true;
        //Set the player is quiet.
        isMoving = false;
        //Set the player is quiet.
        //Change the tag of the player to dead.
        gameObject.tag = "Player";
        // Change the Layer of the player to dead.
        gameObject.layer = 0;
    }

}
