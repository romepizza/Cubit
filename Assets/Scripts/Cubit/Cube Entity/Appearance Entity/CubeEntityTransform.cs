﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntityTransform : MonoBehaviour
{
    [Header("----- SETTINGS -----")]
    //public Transform m_transform;
    //public BoxCollider m_boxCollider;
    //public SphereCollider m_sphereCollider;
    [Header("----- DEBUG -----")]

    public CubeEntitySystem m_entitySystemScript;


    public void setTransform(GameObject transformObject)
    {
        Mesh mesh = /*Instantiate(*/transformObject.GetComponent<MeshFilter>().sharedMesh/*)*/;
        GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.localScale = transformObject.transform.localScale;

        BoxCollider[] boxColliders = GetComponents<BoxCollider>();
        SphereCollider[] shpereColliders = GetComponents<SphereCollider>();
        CapsuleCollider[] capsuleColliders = GetComponents<CapsuleCollider>();

        for (int i = 0; i < boxColliders.Length; i++)
            Destroy(boxColliders[i]);
        for (int i = 0; i < shpereColliders.Length; i++)
            Destroy(shpereColliders[i]);
        for (int i = 0; i < capsuleColliders.Length; i++)
            Destroy(capsuleColliders[i]);

        if (transformObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.AddComponent<BoxCollider>();
            GetComponent<BoxCollider>().size = transformObject.GetComponent<BoxCollider>().size;
            GetComponent<BoxCollider>().center = transformObject.GetComponent<BoxCollider>().center;
            GetComponent<BoxCollider>().material = transformObject.GetComponent<BoxCollider>().sharedMaterial;
        }
        if (transformObject.GetComponent<SphereCollider>() != null)
        {
            gameObject.AddComponent<SphereCollider>();
            GetComponent<SphereCollider>().radius = transformObject.GetComponent<SphereCollider>().radius;
            GetComponent<SphereCollider>().center = transformObject.GetComponent<SphereCollider>().center;
            GetComponent<SphereCollider>().material = transformObject.GetComponent<SphereCollider>().sharedMaterial;
        }
        if (transformObject.GetComponent<CapsuleCollider>() != null)
        {
            gameObject.AddComponent<CapsuleCollider>();
            GetComponent<CapsuleCollider>().radius = transformObject.GetComponent<CapsuleCollider>().radius;
            GetComponent<CapsuleCollider>().direction = transformObject.GetComponent<CapsuleCollider>().direction;
            GetComponent<CapsuleCollider>().center = transformObject.GetComponent<CapsuleCollider>().center;
            GetComponent<CapsuleCollider>().height = transformObject.GetComponent<CapsuleCollider>().height;
            GetComponent<CapsuleCollider>().material = transformObject.GetComponent<CapsuleCollider>().sharedMaterial;
        }

        Rigidbody rb = transformObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Rigidbody myRb = GetComponent<Rigidbody>();
            if(myRb != null)
            {
                myRb.mass = rb.mass;
                myRb.drag = rb.drag;
                myRb.angularDrag = rb.angularDrag;
                myRb.isKinematic = rb.isKinematic;
                myRb.interpolation = rb.interpolation;
                myRb.collisionDetectionMode = rb.collisionDetectionMode;
            }
        }
    }
}