using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public GameObject start;//where laser will come from
	public GameObject end;//target
	public GameObject laser;//cube

	private BoxCollider2D lineCollider;
	private Renderer renderer;

	public float laserLength = 10f;
	public float laserWidth = 0.05f;
	public float fadeTime = 3f;

	public float cooldown =3f;//time between shots(no laser or aiming)
	public float aimTime = 3f;//duration of aiming
	public float shotTime = 3f;//duration of shot

	private int i;
	private float[] action = new float[3];//0-cooldown, 1-aimTime, 2-shotTime
	float timeRemaining;

	private Color blastColor;
	private float t, timer;

	void Start()
	{
		action[0] = cooldown;
		action[1] = aimTime;
		action[2] = shotTime;
		lineCollider = laser.gameObject.GetComponent<BoxCollider2D>();
		renderer = laser.gameObject.GetComponent<Renderer>();
		laser.transform.localScale = new Vector3(1f, 0.1f, 1f);
		blastColor = renderer.material.color;

		renderer.enabled = true;
		lineCollider.enabled = false;
		timeRemaining = action[i];
	}

	// Update is called once per frame
	void Update()
	{
		
		timeRemaining -= Time.deltaTime;
        t += Time.deltaTime/fadeTime;

		
		if (i == 0)//cooldown
		{
			
            renderer.material.color = Color.Lerp(blastColor, new Color(blastColor.r, blastColor.g, blastColor.b, 0.0f), t*2);
            lineCollider.enabled = false;
			//Debug.Log("cooldown");
		}
		else if (i == 1)//aim
		{
		
			follow(start, end, laserLength, laserWidth);
			renderer.material.color = Color.Lerp(new Color(blastColor.r, blastColor.g, blastColor.b, 0.0f), new Color(blastColor.r, blastColor.g, blastColor.b, 0.3f), t);
			//Debug.Log("aim");
		
		}
		else//shoot
		{
            renderer.material.color = Color.Lerp(new Color(blastColor.r, blastColor.g, blastColor.b, 0.3f),blastColor, t);
			lineCollider.enabled = true;
			//Debug.Log("shoot");
		}

		if (timeRemaining <= 0)
		{
			if (i < action.Length-1)
				i++;
			else
				i = 0;
			timeRemaining = action[i]; // set timer for next task
            t = 0;//reset timer
		}
		


		//follow(start, end, laserLength, laserWidth);

	
	}

	float getAngle(GameObject shooter, GameObject target)
	{
		double x = shooter.transform.position.x - target.transform.position.x;//x distance
		double y = shooter.transform.position.y - target.transform.position.y;//y distance
		return Mathf.Atan((float)y / (float)x) * Mathf.Rad2Deg;
	}

	void follow(GameObject shooter, GameObject target, float lineLength, float lineWidth)
	{
		float angle = getAngle(shooter, target);
		laser.transform.localScale = new Vector3(lineLength, lineWidth, 1f);//resize cube
		laser.transform.parent = shooter.transform;//makes the shooter the cube's 0,0,0

		if(target.transform.position.x <= shooter.transform.position.x)
			laser.transform.localPosition = new Vector3(Mathf.Cos(angle*Mathf.Deg2Rad)*-lineLength / 2, Mathf.Sin(angle*Mathf.Deg2Rad)*-lineLength / 2, 1f);
		else
			laser.transform.localPosition = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * lineLength / 2, Mathf.Sin(angle * Mathf.Deg2Rad) * lineLength / 2, 1f);

		laser.transform.rotation = Quaternion.Euler(0, 0, angle);//rotates cube
	}

}
