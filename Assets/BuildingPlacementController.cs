using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{

    [SerializeField]
    private GameObject placeableObjectPrefab;
    [SerializeField]
    private Material isPlaceable;
    [SerializeField]
    private Material isNotPlaceable;
    [SerializeField]
    private Material materialWhenPlaced;

    public GameObject uiBranch;


    [SerializeField]

    private GameObject currentPlaceableObject;
    private float mouseWheelRotation;


    public bool toggle = false;

    // Update is called once per frame

    public void Start()
    {
        materialWhenPlaced = placeableObjectPrefab.GetComponentInChildren<Renderer>().sharedMaterial;

    }

    private void Update()
    {
        // HandleNewObjectHotKey();
        HandleCancelObject();
        if (currentPlaceableObject != null)
        {
            // if (currentPlaceableObject.transform.localRotation.x != 0 || currentPlaceableObject.transform.localRotation.z != 0)
            //  {
            //    currentPlaceableObject.GetComponentInChildren<Renderer>().sharedMaterial = isNotPlaceable;
            //   }

            MoveCurrentPlaceableObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();

        }
    }



    private void ReleaseIfClicked()
    {

        if (currentPlaceableObject.GetComponentInChildren<Renderer>().sharedMaterial == isPlaceable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPlaceableObject.GetComponentInChildren<Renderer>().sharedMaterial = materialWhenPlaced;
                currentPlaceableObject.GetComponent<InstantiateMaterials>().placed = true;
                currentPlaceableObject = null;
                CloseBranch();

            }
        }
        else if (currentPlaceableObject.GetComponentInChildren<Renderer>().sharedMaterial == isNotPlaceable)
        {
            print("This object cannot be placed here");

        }


    }

    private void RotateFromMouseWheel()
    {

        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 90f);
    }
    private void MoveCurrentPlaceableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {

            //Vector3 roundUp = new Vector3(Mathf.RoundToInt(hitInfo.point.x), Mathf.RoundToInt(hitInfo.point.y), Mathf.RoundToInt(hitInfo.point.z));
            currentPlaceableObject.transform.position = hitInfo.point;

            //the object will rotate to fit if the ground is uneven
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }

    }

    public void HandleNewObjectHotKey()
    {
        if (toggle == false)
        {
            toggle = true;
            if (currentPlaceableObject == null)
            {
                currentPlaceableObject = Instantiate(placeableObjectPrefab);
                currentPlaceableObject.GetComponentInChildren<Renderer>().sharedMaterial = isPlaceable;



            }
            else
            {
                Destroy(currentPlaceableObject);
            }
        }
        else
        {
            toggle = false;

            Destroy(currentPlaceableObject);

        }




    }
    private void HandleCancelObject()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
                CloseBranch();

            }
        }


    }

    private void CloseBranch()
    {
        toggle = false;
        uiBranch.SetActive(false);

    }
}

