using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityStandardAssets._2D;

public class ArmRotation : MonoBehaviour
{

    public int rotationOffset = 90;
    private bool facingRight = true;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);

        float armAngle = rotZ + rotationOffset;

        if(armAngle < 90 && armAngle >-90 && !facingRight)
        {
            FlipY();
        }
        else if ((armAngle > 90 && facingRight) || (armAngle<-90 && facingRight))
        {
            FlipY();
        }

        /*if ((rotZ  >= 90) || (rotZ <= 270))
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }*/
    }

    void FlipY()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }
}


