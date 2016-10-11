/*===============================================================================
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved.
Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GazebuttonTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        TOP_TRIGGER,
        BOTTOM_TRIGGER,
        UNDER_TRIGGER,
        RESET_TRIGGER,
        CASE1_TRIGGER,
        CASE2_TRIGGER
    }

    #region PUBLIC_MEMBER_VARIABLES
    public TriggerType triggerType = TriggerType.TOP_TRIGGER;
    public float activationTime = 1.5f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;
    public bool Focused { get; set; }
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    private float mFocusedTime = 0;
	public DistanceCalc distCalc; // = new DistanceCalc();
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        mFocusedTime = 0;
        Focused = false;
        GetComponent<Renderer>().material = nonFocusedMaterial;
    }

    void Update()
    {

        UpdateMaterials(Focused);

        bool startAction = false;
        if (Input.GetMouseButtonUp(0))
        {
            startAction = true;
        }

        if (Focused)
        {
            // Update the "focused state" time
            mFocusedTime += Time.deltaTime;
            if ((mFocusedTime > activationTime) || startAction)
            {
                mFocusedTime = 0;
                
                switch (triggerType)
                {
                    //TODO: Do the calls to DistanceCalc
                    case TriggerType.TOP_TRIGGER:
                        Debug.Log("TOP TRIGGER");
                        break;
                    case TriggerType.BOTTOM_TRIGGER:
                        Debug.Log("BOTTOM TRIGGER");
                        break;
                    case TriggerType.UNDER_TRIGGER:
                        Debug.Log("UNDER TRIGGER");
                        break;
                    case TriggerType.RESET_TRIGGER:
                        Debug.Log("RESET TRIGGER");
                        distCalc.RestartGame();
                        break;
                    case TriggerType.CASE1_TRIGGER:
                        Debug.Log("CASE1 TRIGGER");
                        break;
                    case TriggerType.CASE2_TRIGGER:
                        Debug.Log("CASE2 TRIGGER");
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            // Reset the "focused state" time
            mFocusedTime = 0;
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS
    private void UpdateMaterials(bool focused)
    {
        Renderer meshRenderer = GetComponent<Renderer>();
        if (focused)
        {
            if (meshRenderer.material != focusedMaterial)
                meshRenderer.material = focusedMaterial;
        }
        else
        {
            if (meshRenderer.material != nonFocusedMaterial)
                meshRenderer.material = nonFocusedMaterial;
        }

        float t = focused ? Mathf.Clamp01(mFocusedTime / activationTime) : 0;
        foreach (var rnd in GetComponentsInChildren<Renderer>())
        {
            if (rnd.material.shader.name.Equals("Custom/SurfaceScan"))
            {
                rnd.material.SetFloat("_ScanRatio", t);
            }
        }
    }

    private IEnumerator ResetAfter(float seconds)
    {
        Debug.Log("Resetting View trigger after: " + seconds);

        yield return new WaitForSeconds(seconds);

        Debug.Log("Resetting View trigger: " + this.gameObject.name);

        // Reset variables
        mFocusedTime = 0;
        Focused = false;
        UpdateMaterials(false);
    }
    #endregion // PRIVATE_METHODS
}

