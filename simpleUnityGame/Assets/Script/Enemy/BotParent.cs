using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotParent : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    private CharacterController characterCtrl;
    private GameObject playerObject;
    public ParticleSystem hitParticleObject;
    private int lifeCnt = 3;
    private float findTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterCtrl = GetComponent<CharacterController>();
        playerObject = GameObject.Find("HPCharacter");
        findTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerObject.transform.position, characterCtrl.transform.position) < 20.0f)//따라가게
        {
            if (Time.time > findTime)
            {                
                agent.SetDestination(playerObject.transform.position);
                findTime = Time.time + 0.5f;
            }
        }
    }

    public void Attacked()
    {
        lifeCnt -= 1;
        if (lifeCnt <= 0)
        {
            ParticleSystem hitParticle;
            hitParticle = ParticleSystem.Instantiate(hitParticleObject, transform.position, Quaternion.identity);
            hitParticle.Play();
            Destroy(hitParticle.gameObject, 2.0f);
            Destroy(gameObject);
            return;
        }
    }
}
