using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerMove : MonoBehaviour
{
    public float moveSpeed = 3f;    

    private Vector3 targetPosition; 

    private void Start()
    {
        GenerateRandomTargetPosition();
    }

    private void Update()
    {
        MoveToTargetPosition();
    }

    private void GenerateRandomTargetPosition()
    {
        
        float randomX = Random.Range(10f, 25f);
        float randomZ = Random.Range(10f, 25f);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
    }

    private void MoveToTargetPosition()
    {
        
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.5f)
        {
            GenerateRandomTargetPosition();
        }
    }
}
