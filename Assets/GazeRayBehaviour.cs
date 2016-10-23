/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class GazeRayBehaviour : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    public GazebuttonTrigger[] gazebuttonTriggers;
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        //Setting focus mode to contiuous auto
        bool focusModeSet = CameraDevice.Instance.SetFocusMode(
        CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

        if (!focusModeSet)
        {
            Debug.Log("Failed to set focus mode (unsupported mode).");
        }

        // Check if the Head gaze direction is intersecting any of the ViewTriggers
        RaycastHit hit;
        Ray cameraGaze = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
        foreach (var trigger in gazebuttonTriggers)
        {
            trigger.Focused = hit.collider && (hit.collider.gameObject == trigger.gameObject);
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS
}

