using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaHitbox : MonoBehaviour {

	private const float aThershold = 0.1f;

	void Start () {
		this.GetComponent<Image> ().alphaHitTestMinimumThreshold = aThershold;
	}
}
