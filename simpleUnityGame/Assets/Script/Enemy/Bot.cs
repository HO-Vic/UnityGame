using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playerObject;
    private Animator animator;
    private float direction;
    void Start()
    {
        playerObject = GameObject.Find("HPCharacter");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = playerObject.transform.position - transform.position;
        float playerDis = Vector3.Distance(playerObject.transform.position, transform.position);
        if (playerDis < 20.0f)//따라가게
        {
            //Debug.Log("Rotation: " + transform.rotation.eulerAngles.x + ", " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z);
            //바라보는 방향 벡터 계산
            //모델이 x축이 90도 회전 -> -90도 회전하여 원하는 모델 모습 만듦
            //그런데, 모델이 x축이 이미 90도 회전됐기때문에, LookAt()을 해버리면 땅을 쳐다보게됨
            //원하는 모델의 up벡터가 forward벡터이기때문에
            //바라보는 방향벡터를 -90하여 원래 모델의 forward벡터인 현재 모습 up벡터에 맞추기 위해 회전

            Quaternion lookVector = Quaternion.LookRotation(targetDir) * Quaternion.Euler(-90.0f, 0f, 0f);//회전 계산된 룩벡터
            //Debug.Log("Rotation: " + lookVector.eulerAngles.x + ", " + lookVector.eulerAngles.y + ", " + lookVector.eulerAngles.z);

            float dotResRight = Vector3.Dot(transform.right, Vector3.Normalize(targetDir));//좌 우 판단
            float dotResForward = Vector3.Dot(transform.forward, Vector3.Normalize(targetDir));//앞 뒤 판단
                                                                                               //+ + left
                                                                                               //+ - right
                                                                                               //- + left
                                                                                               //- - right
                                                                                               //Debug.Log("dotRes: " + dotRes);
            direction = 0.5f;//나머지 다 해당이 안된다면 right

            if (Mathf.Abs(dotResRight) < 0.1f && dotResForward > 0)//코사인은 90도일때, 0값을 가지는데, 0과 거리가 0.1f일때, 바로 바라보게 만들고 애니메이션 정지
            {
                if (playerDis < 5.0f)
                    animator.SetBool("IsMove", false);
                else animator.SetBool("IsMove", true);
                direction = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f);
            }
            else if (dotResRight > 0)//현재 객체의 라이트 벡터와 내적했을때, 양수라면 오른쪽에 위치하고 있음
            {
                if (dotResForward > 0)
                    direction = -0.5f;//left                 
                animator.SetBool("IsMove", true);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f * Time.deltaTime);
            }
            else if (dotResRight < 0)
            {
                if (dotResForward > 0)
                    direction = -0.5f;//left       
                animator.SetBool("IsMove", true);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f * Time.deltaTime);
            }
            animator.SetBool("IsMove", true);


        }
        else if (Vector3.Distance(playerObject.transform.position, transform.position) < 200.0f)//바라보기
        {
            //Debug.Log("Rotation: " + transform.rotation.eulerAngles.x + ", " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z);
            //Vector3 targetDir = playerObject.transform.position - transform.position;
            //바라보는 방향 벡터 계산
            //모델이 x축이 90도 회전 -> -90도 회전하여 원하는 모델 모습 만듦
            //그런데, 모델이 x축이 이미 90도 회전됐기때문에, LookAt()을 해버리면 땅을 쳐다보게됨
            //원하는 모델의 up벡터가 forward벡터이기때문에
            //바라보는 방향벡터를 -90하여 원래 모델의 forward벡터인 현재 모습 up벡터에 맞추기 위해 회전

            Quaternion lookVector = Quaternion.LookRotation(targetDir) * Quaternion.Euler(-90.0f, 0f, 0f);//회전 계산된 룩벡터
                                                                                                          //Debug.Log("Rotation: " + lookVector.eulerAngles.x + ", " + lookVector.eulerAngles.y + ", " + lookVector.eulerAngles.z);

            float dotResRight = Vector3.Dot(transform.right, Vector3.Normalize(targetDir));//좌 우 판단
            float dotResForward = Vector3.Dot(transform.forward, Vector3.Normalize(targetDir));//앞 뒤 판단
                                                                                               //Debug.Log("dotRes: " + dotRes);
            direction = 0.5f;//기본 right

            if (Mathf.Abs(dotResRight) < 0.1f && dotResForward > 0)//코사인은 90도일때, 0값을 가지는데, 0과 거리가 0.1f일때, 바로 바라보게 만들고 애니메이션 정지
            {
                animator.SetBool("IsMove", false);
                direction = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f);
            }
            else if (dotResRight > 0)//현재 객체의 라이트 벡터와 내적했을때, 양수라면 오른쪽에 위치하고 있음
            {
                if (dotResForward > 0)
                    direction = -0.5f;//left  
                animator.SetBool("IsMove", true);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f * Time.deltaTime);
            }
            else if (dotResRight < 0)
            {
                if (dotResForward > 0)
                    direction = -0.5f;//left  
                animator.SetBool("IsMove", true);
                direction = -1;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f * Time.deltaTime);
            }

            animator.SetFloat("MoveDirection", direction, 0.25f, Time.deltaTime);
        }
        Vector3 t = transform.localPosition;
        t.y = 0.0f;
        transform.localPosition = t;
    }
}
