using System;
using UnityEngine;

public class ForceController : MonoBehaviour
{
    public Camera cam;
    public bool pullForce;
    public float pullStrength, pushStrength;
    public float pullRange = 25, pullRadius = 4, pushRadius = 3;

    public Collider[] targetObjects;

    public Transform holdPosition, pushPosition;

   // public Animator animator;

    private void Update()
    {
        // Button Down (Pull - Gather Objects)
        if (Input.GetButtonDown("Fire2"))
        {
            //animator.SetTrigger("Pull");
            pullForce = true;
            GetPullObjects();

        }
        // Holding Button (Pulling)
        if (Input.GetButton("Fire2"))
        {
            if (pullForce)
            {
                PullForce();
            }
        }
        // Release (Let It Fall)
        if (Input.GetButtonUp("Fire2"))
        {
            pullForce = false;
        }
        // Throw (LMB - Down)
        if (Input.GetButtonDown("Fire1"))
        {
            if (pullForce)
            {
                // Throw
                //animator.SetTrigger("Throw");
                ThrowForce();
                pullForce = false;
            }
            else
            {
                // Push
                //animator.SetTrigger("Push");
                PushForce();
            }
        }
    }

    /*
    public void AnimPull()
    {
        pullForce = true;
        GetPullObjects();
    }
    public void AnimThrow()
    {
        pullForce = true;
        ThrowForce();
    }
    public void AnimPush()
    {
        PushForce();
    }

    */

    public void PushForce()
    {
        GetPushObjects();
        ThrowForce();
    }

    public void ThrowForce()
    {
        if(targetObjects != null && targetObjects.Length > 0)
        {
            foreach(Collider col in targetObjects)
            {
                if(col.GetComponent<Rigidbody>())
                {
                    col.GetComponent<Rigidbody>().AddForce(cam.transform.TransformDirection(Vector3.forward) * pushStrength, ForceMode.Impulse);
                }
            }
        }
    }

    public void PullForce()
    {
        if(targetObjects != null && targetObjects.Length > 0)
        {
            foreach(Collider col in targetObjects)
            {
                if(col.GetComponent<Rigidbody>())
                {
                    col.GetComponent<Rigidbody>().linearVelocity = (holdPosition.position - col.transform.position) * pullStrength * Time.deltaTime;
                }
            }
        }
    }

    public void GetPullObjects()
    {
        targetObjects = null;

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, pullRange))
        {
            targetObjects = Physics.OverlapSphere(hit.point, pullRadius);
        }
    }

    public void GetPushObjects()
    {
        targetObjects = null;
        targetObjects = Physics.OverlapSphere(pushPosition.position, pushRadius);
    }
}
