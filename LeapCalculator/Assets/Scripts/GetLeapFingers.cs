using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class GetLeapFingers : MonoBehaviour {

    LeapHandController handController;
    HandModel hand_model;
    Hand leap_hand;

    // Use this for initialization
    void Start () {
        
        /*hand_model = GetComponent<HandModel>();
        if (hand_model == null) Debug.LogError("No hand_model founded");
        else leap_hand = hand_model.GetLeapHand();
        if (leap_hand == null) Debug.LogError("No leap_hand founded");*/
    }
	
	// Update is called once per frame
	void Update () {
        hand_model = GetComponent<HandModel>();
        //if (hand_model == null) Debug.LogError("No hand_model founded");
        //else leap_hand = hand_model.GetLeapHand();
        if (hand_model != null) Debug.Log("hand_model founded");
        //if (leap_hand == null) Debug.LogError("No leap_hand founded");

        /*for (int i = 0; i < HandModel.NUM_FINGERS; i++)
        {
            FingerModel finger = hand_model.fingers[i];
            // draw ray from finger tips (enable Gizmos in Game window to see)
            Debug.DrawRay(finger.GetTipPosition(), finger.GetRay().direction, Color.red);
        }*/
    }
}
