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
        if (playerDis < 20.0f)//���󰡰�
        {
            //Debug.Log("Rotation: " + transform.rotation.eulerAngles.x + ", " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z);
            //�ٶ󺸴� ���� ���� ���
            //���� x���� 90�� ȸ�� -> -90�� ȸ���Ͽ� ���ϴ� �� ��� ����
            //�׷���, ���� x���� �̹� 90�� ȸ���Ʊ⶧����, LookAt()�� �ع����� ���� �Ĵٺ��Ե�
            //���ϴ� ���� up���Ͱ� forward�����̱⶧����
            //�ٶ󺸴� ���⺤�͸� -90�Ͽ� ���� ���� forward������ ���� ��� up���Ϳ� ���߱� ���� ȸ��

            Quaternion lookVector = Quaternion.LookRotation(targetDir) * Quaternion.Euler(-90.0f, 0f, 0f);//ȸ�� ���� �躤��
            //Debug.Log("Rotation: " + lookVector.eulerAngles.x + ", " + lookVector.eulerAngles.y + ", " + lookVector.eulerAngles.z);

            float dotResRight = Vector3.Dot(transform.right, Vector3.Normalize(targetDir));//�� �� �Ǵ�
            float dotResForward = Vector3.Dot(transform.forward, Vector3.Normalize(targetDir));//�� �� �Ǵ�
                                                                                               //+ + left
                                                                                               //+ - right
                                                                                               //- + left
                                                                                               //- - right
                                                                                               //Debug.Log("dotRes: " + dotRes);
            direction = 0.5f;//������ �� �ش��� �ȵȴٸ� right

            if (Mathf.Abs(dotResRight) < 0.1f && dotResForward > 0)//�ڻ����� 90���϶�, 0���� �����µ�, 0�� �Ÿ��� 0.1f�϶�, �ٷ� �ٶ󺸰� ����� �ִϸ��̼� ����
            {
                if (playerDis < 5.0f)
                    animator.SetBool("IsMove", false);
                else animator.SetBool("IsMove", true);
                direction = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f);
            }
            else if (dotResRight > 0)//���� ��ü�� ����Ʈ ���Ϳ� ����������, ������ �����ʿ� ��ġ�ϰ� ����
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
        else if (Vector3.Distance(playerObject.transform.position, transform.position) < 200.0f)//�ٶ󺸱�
        {
            //Debug.Log("Rotation: " + transform.rotation.eulerAngles.x + ", " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z);
            //Vector3 targetDir = playerObject.transform.position - transform.position;
            //�ٶ󺸴� ���� ���� ���
            //���� x���� 90�� ȸ�� -> -90�� ȸ���Ͽ� ���ϴ� �� ��� ����
            //�׷���, ���� x���� �̹� 90�� ȸ���Ʊ⶧����, LookAt()�� �ع����� ���� �Ĵٺ��Ե�
            //���ϴ� ���� up���Ͱ� forward�����̱⶧����
            //�ٶ󺸴� ���⺤�͸� -90�Ͽ� ���� ���� forward������ ���� ��� up���Ϳ� ���߱� ���� ȸ��

            Quaternion lookVector = Quaternion.LookRotation(targetDir) * Quaternion.Euler(-90.0f, 0f, 0f);//ȸ�� ���� �躤��
                                                                                                          //Debug.Log("Rotation: " + lookVector.eulerAngles.x + ", " + lookVector.eulerAngles.y + ", " + lookVector.eulerAngles.z);

            float dotResRight = Vector3.Dot(transform.right, Vector3.Normalize(targetDir));//�� �� �Ǵ�
            float dotResForward = Vector3.Dot(transform.forward, Vector3.Normalize(targetDir));//�� �� �Ǵ�
                                                                                               //Debug.Log("dotRes: " + dotRes);
            direction = 0.5f;//�⺻ right

            if (Mathf.Abs(dotResRight) < 0.1f && dotResForward > 0)//�ڻ����� 90���϶�, 0���� �����µ�, 0�� �Ÿ��� 0.1f�϶�, �ٷ� �ٶ󺸰� ����� �ִϸ��̼� ����
            {
                animator.SetBool("IsMove", false);
                direction = 0;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookVector, 10.0f);
            }
            else if (dotResRight > 0)//���� ��ü�� ����Ʈ ���Ϳ� ����������, ������ �����ʿ� ��ġ�ϰ� ����
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
