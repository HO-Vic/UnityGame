using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update    
    [SerializeField] private float moveSpeed = 10.0f;
    //파티클
    public ParticleSystem fireParticleObject;
    public ParticleSystem hitParticleObject;
    public ParticleSystem obstacleHitParticleObject;
    public ParticleSystem obstacleHitParticleObject2;
    public ParticleSystem floorHitParticleObject;
    //컴포넌트
    private Animator animator;
    private CharacterController characterCtrl;

    //레이어
    private int FloorLayer = 8;
    private int ObstacleLayer = 8;
    private int enemyLayer = 8;
    private int mouseRayMask = 0;
    private int floorRayMask = 0;//중력 적용

    //crossHair
    private UnityEngine.UI.Image crossHair;
    private Color defaultCrossHairColor = Color.green;
    private Color targetCrossHairColor = Color.red;

    //블랜드트리
    private float LEFT = -1.0f;
    private float RIGHT = 1.0f;
    private float FORWARD = 0.0f;

    //사격 파티클 위치 오프셋
    private GameObject particleStartObject;
    //keyDown
    private bool[] KeyDown = new bool[4];//WASD
    //fire
    private float fireTime;
    const float fireRate = 0.5f;
    //카메라
    //Camera Rotate
    private float cameraXAngle;
    private GameObject cameraOffset;
    //플레이어 y오프셋
    private float playerYOffset = 0.515f;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterCtrl = GetComponent<CharacterController>();
        particleStartObject = GameObject.Find("PlayerParticleOffset");
        cameraOffset = GameObject.Find("CameraOffset");
        cameraXAngle = cameraOffset.transform.rotation.eulerAngles.x;

        FloorLayer = LayerMask.NameToLayer("Floor");
        ObstacleLayer = LayerMask.NameToLayer("Obstacle");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        floorRayMask = 1 << FloorLayer;
        mouseRayMask = 1 << FloorLayer;
        mouseRayMask += 1 << ObstacleLayer;
        mouseRayMask += 1 << enemyLayer;

        //init
        KeyDown[0] = false;
        KeyDown[1] = false;
        KeyDown[2] = false;
        KeyDown[3] = false;

        if (animator.layerCount >= 2)
            animator.SetLayerWeight(1, 1);

        crossHair = GetComponentInChildren<UnityEngine.UI.Image>();
        crossHair.color = defaultCrossHairColor;

        fireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offSet = Vector3.zero;
        float direction = FORWARD;
        if (Input.GetKeyUp(KeyCode.W)) KeyDown[0] = false;
        if (Input.GetKeyUp(KeyCode.A)) KeyDown[1] = false;
        if (Input.GetKeyUp(KeyCode.S)) KeyDown[2] = false;
        if (Input.GetKeyUp(KeyCode.D)) KeyDown[3] = false;

        if (Input.GetKeyDown(KeyCode.W)) KeyDown[0] = true;
        if (Input.GetKeyDown(KeyCode.A)) KeyDown[1] = true;
        if (Input.GetKeyDown(KeyCode.S)) KeyDown[2] = true;
        if (Input.GetKeyDown(KeyCode.D)) KeyDown[3] = true;

        if (KeyDown[1])//a
        {
            offSet -= characterCtrl.transform.right * Time.deltaTime * moveSpeed;
            if (!animator.GetBool("IsRunBack"))
                animator.SetBool("IsRunFront", true);
            direction = LEFT;
            if (KeyDown[0] || KeyDown[2])
                direction = -0.5f;
        }
        if (KeyDown[3])//d
        {
            offSet += characterCtrl.transform.right * Time.deltaTime * moveSpeed;
            if (!animator.GetBool("IsRunBack"))
                animator.SetBool("IsRunFront", true);
            direction = RIGHT;
            if (KeyDown[0] || KeyDown[2])
                direction = +0.5f;
        }
        if (KeyDown[0])//w
        {
            offSet += characterCtrl.transform.forward * Time.deltaTime * moveSpeed;
            //direction = FORWARD;
            animator.SetBool("IsRunFront", true);
            animator.SetBool("IsRunBack", false);
        }
        if (KeyDown[2])//s
        {
            offSet -= characterCtrl.transform.forward * Time.deltaTime * moveSpeed;
            animator.SetBool("IsRunBack", true);
            animator.SetBool("IsRunFront", false);
            // direction = FORWARD;
        }
        if (!(KeyDown[0] || KeyDown[1] || KeyDown[2] || KeyDown[3]))
        {
            animator.SetBool("IsRunBack", false);
            animator.SetBool("IsRunFront", false);
            direction = FORWARD;
        }


        RaycastHit rH;
        Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        bool resRayCast = Physics.Raycast(r, out rH, 1000.0f, mouseRayMask);
        crossHair.color = defaultCrossHairColor;
        if (resRayCast && rH.collider.gameObject.layer == enemyLayer)//enemy일때
        {
            crossHair.color = targetCrossHairColor;
        }

        //shot
        if (Input.GetMouseButtonDown((int)UnityEngine.UIElements.MouseButton.LeftMouse))
        {
            animator.SetBool("IsShoot", true);
        }
        if (Input.GetMouseButton((int)UnityEngine.UIElements.MouseButton.LeftMouse))
        {
            if (fireTime < Time.time)
            {
                if (resRayCast)
                {
                    BotParent hitMineBot = rH.collider.GetComponent<BotParent>();
                    if (hitMineBot != null)
                    {
                        hitMineBot.Attacked();
                        ParticleSystem hitParticle;
                        hitParticle = ParticleSystem.Instantiate(hitParticleObject, rH.point, Quaternion.identity);
                        hitParticle.Play();
                        ParticleSystem hitParticle2 = ParticleSystem.Instantiate(obstacleHitParticleObject, rH.point, Quaternion.identity);
                        hitParticle2.Play();
                        Destroy(hitParticle.gameObject, 2.0f);
                        Destroy(hitParticle2.gameObject, 2.0f);
                    }

                    else if (rH.collider.gameObject.layer == FloorLayer)
                    {
                        ParticleSystem hitParticle;
                        hitParticle = ParticleSystem.Instantiate(floorHitParticleObject, rH.point, Quaternion.identity);
                        Destroy(hitParticle.gameObject, 2.0f);
                        hitParticle.Play();
                    }
                    else
                    {
                        ParticleSystem hitParticle;
                        hitParticle = ParticleSystem.Instantiate(obstacleHitParticleObject, rH.point, Quaternion.identity);
                        Destroy(hitParticle.gameObject, 2.0f);
                        ParticleSystem hitParticle2 = ParticleSystem.Instantiate(obstacleHitParticleObject2, rH.point, Quaternion.identity);
                        Destroy(hitParticle2.gameObject, 2.0f);
                        hitParticle.Play();
                        hitParticle2.Play();
                    }
                }
                ParticleSystem shootParticle = ParticleSystem.Instantiate(fireParticleObject, particleStartObject.transform.position, Quaternion.identity);
                shootParticle.Play();
                Destroy(shootParticle.gameObject, 1.0f);
                fireTime = Time.time + fireRate;
            }
        }

        if (Input.GetMouseButtonUp((int)UnityEngine.UIElements.MouseButton.LeftMouse))
        {
            animator.SetBool("IsShoot", false);
        }

        animator.SetFloat("MoveDirection", direction, 0.25f, Time.deltaTime);

        Ray gravityRay = new Ray(characterCtrl.transform.position, Vector3.down);
        RaycastHit gravityRayHit;
        if (Physics.Raycast(gravityRay, out gravityRayHit, 100.0f, floorRayMask))
        {
            float floorDis = Mathf.Abs(gravityRayHit.distance - playerYOffset);
            if (floorDis > float.Epsilon)
            {
                offSet += 9.8f * Time.deltaTime * Vector3.down;
            }
        }
        //mouse
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        float cameraXAngleTemp = cameraXAngle - vertical * 1.1f;
        if (cameraXAngleTemp < 40.0f && cameraXAngleTemp > -20.0f)
        {
            cameraXAngle = cameraXAngleTemp;
            cameraOffset.transform.Rotate(-vertical * 1.1f, 0, 0);
        }

        characterCtrl.transform.Rotate(0, horizontal * 1.1f, 0);
        characterCtrl.Move(offSet);
    }
}
