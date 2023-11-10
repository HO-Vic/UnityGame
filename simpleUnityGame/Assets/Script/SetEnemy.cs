using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mechBot;
    public GameObject mineBot;
    void Start()
    {
        Vector3[] vertices = NavMesh.CalculateTriangulation().vertices;//네비메쉬의 점
        int[] indices = NavMesh.CalculateTriangulation().indices;

        Vector3[] triangleData = new Vector3[indices.Length / 3];
        for (int i = 0; i < indices.Length / 3; i++)
        {
            triangleData[i] = (vertices[indices[3 * i]] + vertices[indices[3 * i + 1]] + vertices[indices[3 * i + 2]]) / 3.0f;//네비 메쉬를 이루는 삼각형의 중점
        }

        for (int i = 0; i < 8; i++)
        {
            int randIdx;
            randIdx = UnityEngine.Random.Range(0, triangleData.Length - i);
            GameObject mech = GameObject.Instantiate(mechBot, triangleData[randIdx], mechBot.transform.rotation);//같은 위치에 생성되지 않게 설정

            //맨뒤 인덱스와 바꿔서 중복 제거
            Vector3 temp = triangleData[randIdx];
            triangleData[randIdx] = triangleData[triangleData.Length - i - 1];
            triangleData[randIdx] = temp;
        }
        for (int i = 0; i < 8; i++)
        {
            int randIdx;
            randIdx = UnityEngine.Random.Range(0, triangleData.Length - i);
            GameObject mine = GameObject.Instantiate(mineBot, triangleData[randIdx], mechBot.transform.rotation);//같은 위치에 생성되지 않게 설정

            //맨뒤 인덱스와 바꿔서 중복 제거
            Vector3 temp = triangleData[randIdx];
            triangleData[randIdx] = triangleData[triangleData.Length - i - 1];
            triangleData[randIdx] = temp;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
