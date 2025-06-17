using System.Collections.Generic;
using UnityEngine;

public class Tank : BaseUnit
{
    public List<Transform> guns;

    private void Update()
    {
        if (!canMove)
        {
            if (isAttacking)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    Attack();
                }

                transform.Rotate(new Vector3(0, rotationSpeed, 0));
            }
            else if (atDestination)
                isAttacking = true;
        }

        if (isSelected)
            rangeObject.transform.position = transform.position;
    }

    protected override void Attack()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (Physics.Raycast(guns[i].position, guns[i].rotation * guns[i].position, out RaycastHit hit, range))
            {
                if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                    enemy.GotHit(damage);
            }
        }
        
        attackTimer = attackSpeed;
        isAttacking = true;
    }
}