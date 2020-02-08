using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class hitEffectManual : MonoBehaviour
{
    public Texture2D bloodTexture;
    private bool hit = false;
    private float opacity = 0.0f;

    void  OnGUI () {
		if (Input.GetKey (KeyCode.P)) {
			hit = true;
			opacity = 1.0f;
		}

		if(hit) {
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, opacity);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bloodTexture, ScaleMode.ScaleToFit);
			StartCoroutine("waitAndChangeOpacity");
		}

		if (opacity <= 0) {
			hit = false;
		}
	}

	IEnumerator waitAndChangeOpacity()
	{
		yield return new WaitForEndOfFrame();
		opacity -= 0.05f;
	}
}