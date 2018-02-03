using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Utils;

public class PlayerShip : MonoBehaviour {
	public float ForwardThrust;
	public float RotationalThrust;
	public IShipStats ShipStats;

	void Start () {
		ShipStats = new GalacticaStats ();
		var body = GetComponentInChildren<Rigidbody>();
		body.mass = ShipStats.Mass;
	}
	
	void Update () {
		var body = GetComponentInChildren<Rigidbody>();

		#region Rotate
		if (Input.GetKey (KeyCode.A)) {
			RotationalThrust -= ShipStats.RotationalThrustMax / 100;

			if (RotationalThrust < -ShipStats.RotationalThrustMax)
				RotationalThrust = -ShipStats.RotationalThrustMax;
		}

		if (Input.GetKey (KeyCode.D)) {
			RotationalThrust += ShipStats.RotationalThrustMax / 100;

			if (RotationalThrust > ShipStats.RotationalThrustMax)
				RotationalThrust = ShipStats.RotationalThrustMax;
		}

		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
			if (RotationalThrust > 0) {
				if (RotationalThrust - (ShipStats.RotationalThrustMax / 100) < 0)
					RotationalThrust = 0;
				else
					RotationalThrust -= (ShipStats.RotationalThrustMax / 100);
			}
			else if (RotationalThrust < 0) {
				if (RotationalThrust + (ShipStats.RotationalThrustMax / 100) > 0)
					RotationalThrust = 0;
				else
					RotationalThrust += (ShipStats.RotationalThrustMax / 100);
			}
			else {
				if (body.angularVelocity.magnitude > 0.01)
					body.AddRelativeTorque(-body.angularVelocity.normalized * ShipStats.RotationalThrustMax);
				else
					body.angularVelocity = Vector3.zero;
			}
		}

		body.AddRelativeTorque(Vector3.forward * RotationalThrust);
		#endregion

		#region Move
		if (Input.GetKey (KeyCode.W))
			ForwardThrust += ShipStats.ForwardThrustMax / 100;

		if (Input.GetKey (KeyCode.S))
			ForwardThrust -= ShipStats.ForwardThrustMax / 100;

		ForwardThrust = MathHelper.Clamp(ForwardThrust, 0, ShipStats.ForwardThrustMax);

		if (ForwardThrust > 0)
			body.AddForce(transform.up * ForwardThrust);
		else {
			if (body.velocity.magnitude > 0.01)
				body.AddForce(-body.velocity.normalized * ShipStats.ForwardThrustMax);
			else
				body.velocity = Vector3.zero;
		}
		#endregion
	}
}