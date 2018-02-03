using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Utils;

public class UIManager : MonoBehaviour {
	public PlayerShip PlayerShip;

	public Slider RThrustSlider;
	public Text RThrustText;

	public Slider FWThrustSlider;
	public Text FWThrustText;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		FWThrustSlider.value = PlayerShip.ForwardThrust / PlayerShip.ShipStats.ForwardThrustMax;

		var fThrustStr = ((PlayerShip.ForwardThrust / 1000)).ToString ();
		if (fThrustStr.ToString ().Length > 4)
			fThrustStr = fThrustStr.Substring (0, 4);
		FWThrustText.text = fThrustStr + " kN";

		RThrustSlider.value = (PlayerShip.RotationalThrust + PlayerShip.ShipStats.RotationalThrustMax) / (2 * PlayerShip.ShipStats.RotationalThrustMax);

		//TODO: Broken
		var rThrust = /*MathHelper.AdjustSI(*/Mathf.Abs (PlayerShip.RotationalThrust)/*, 4)*/;

		var rThrustStr = (rThrust *= 1000).ToString();

		if (rThrustStr.ToString ().Length > 4)
			rThrustStr = rThrustStr.Substring (0, 4);

		RThrustText.text = rThrustStr + (PlayerShip.RotationalThrust > 0 ? " mN CCW" : " mN CW");
	}
}
