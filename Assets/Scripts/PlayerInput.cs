using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{
	PlayerController player;
	Vector3 tilt;
	float x;
	float y;

	public AnimationCurve tiltCurveX;
	public AnimationCurve tiltCurveY;


	public AnimationCurve  animCurve = null;
	// these are comfortable numbers for sitting with phone at around 45deg
	// need min < rest < max
	// difference betweem min and rest should be less than diff between rest and max
	// because harder to pull phone back towards you
	// in my opinion at least
	public float minTiltY = -0.65f;
	public float maxTiltY = -0.05f;
	public float restTiltY = -0.4f;

	private void Start()
	{
		player = GetComponent<PlayerController>();


		// this appears to work
		Keyframe[] ks = new Keyframe[5];

		ks[0] = new Keyframe( 0, -5 );
		ks[0].outTangent = 0;          // 90 degrees up

		// add 1 to not give negative numbers
		//ks[1] = new Keyframe( 0.35f, -5 );
		ks[1] = new Keyframe( 1 + minTiltY, -5 );
		ks[1].outTangent = 0;                // straight

		//ks[2] = new Keyframe( 0.6f, 0 );
		ks[2] = new Keyframe( 1 + restTiltY, 0 );
		ks[2].outTangent = 0;         // 90 degrees down

		//ks[3] = new Keyframe( 0.95f, 5 );
		ks[3] = new Keyframe( 1 + maxTiltY, 5 );
		ks[3].outTangent = 0;                // straight

		ks[4] = new Keyframe( 2, 5 );
		ks[4].outTangent = 0;         // 90 degrees down -1.5708f

		animCurve = new AnimationCurve( ks );
	}



	private void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		if (Application.platform == RuntimePlatform.Android) {
			tilt = Input.acceleration;
			x = tilt.x;
			y = tilt.y;

			// make so the default is having the phone tilted 90deg to you, not level
			//y += 0.5f;

			//x = inputCurve (x);
			//y = inputCurve (y);

			// curves have hard coded default tilt.
			// some people might like to have it flat or straight up, etc.
			// I don't know how to use animation curves without hard coding
			// like this probably
			// tiltCurveX.AddKey(new Keyframe(1 - preferredTilt(currently 0.4), 0);

			// add 1.0f to not give the animation curve negative values of time
			x = tiltCurveX.Evaluate (x + 1.0f);
			y = tiltCurveY.Evaluate (y + 1.0f);

			player.move (x, y);
			player.displayTilt (tilt.x, tilt.y, tilt.z, x, y);
		} else {	
			player.move (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		}
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(Vector3.up, new Vector3(0f, this.player.ballSpawn.transform.position.y, 0f));
		float distance;
		if (plane.Raycast(ray, out distance))
		{
			Vector3 point = ray.GetPoint(distance);
			this.player.LookAt(point);
		}
		if (Input.GetButtonDown("Fire1") )
		{
			// Fire1 default left mouse button
			this.player.spellCast(0);
		}
		if (Input.GetButtonDown("Fire2") ) //|| Input.touchCount == 1
		{
			// Fire2 default right mouse button
			this.player.spellCast(1);
		}
		if(Input.GetButtonDown("Fire3"))
		{
			// Fire3 default left shift
			this.player.spellCast (2);
		}
	}

	float inputCurve(float input)
	{

		// 5 is the height of the curve
		// this is the max speed you want for your player kinda
		//
		// 25 is the curve steepness
		// higher and it will return 0 or 5 except for input between 0.2 and 0.3
		// lower and it will return 0 or 5 except for input between -0.2 and 1.4 for example
		// controls the spread of the input variable
		//
		// -0.25 controls the y intercept
		// 0.25 with 25 appears to be a coincidence
		// does not hold for 0.4 and 40. Should be around 0.2 or less with 40

		if (input > 0) {
			return (5f / (1 + (Mathf.Exp (-25 *(input - 0.25f)))));
		} else {
			// -5/(1+ e^(25(x+0.25)))
			// change:
			// 5 to -5
			// -25 to 25
			// -0.25 to 0.25
			return (-5f / (1 + (Mathf.Exp (25 *(input + 0.25f)))));
		}
	}


}
