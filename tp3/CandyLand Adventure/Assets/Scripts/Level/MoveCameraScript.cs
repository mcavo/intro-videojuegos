using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MoveCameraScript : MonoBehaviour {

	private Vector3 RotationVector = new Vector3(0,1f,0);
	public float RotationSpeed = 50f;
	public float MovementSpeed = 10f;
	private float SpeedMultiplier = 10f;
	private float ShakeDuration = 0.5f;
	private BlurOptimized blurEffect;
	private Shake shakeEffect;
	private int TrapsApplyCount = 0;

	void Start() 
	{
		blurEffect = (BlurOptimized)GetComponent <BlurOptimized>(); 
		blurEffect.enabled = false;
		shakeEffect = (Shake)GetComponent <Shake>(); 
		shakeEffect.enabled = false;
	}
		
	void Update()
	{
		RaycastHit hit;

		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.localEulerAngles += RotationVector * RotationSpeed * Time.deltaTime;
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.localEulerAngles -= RotationVector * RotationSpeed * Time.deltaTime;
		}

		if (Input.GetKey (KeyCode.UpArrow)) {
			if (Physics.Raycast (transform.localPosition, transform.forward, out hit, 4 * MovementSpeed * Time.deltaTime)) {
				if (hit.collider.tag == "Finish") {
					GameManager.instance.WinGame ();
				}
			} else {
				transform.localPosition = transform.localPosition + transform.forward * MovementSpeed * Time.deltaTime;
			}
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			if (Physics.Raycast (transform.localPosition, -1 * transform.forward, out hit, 4 * MovementSpeed * Time.deltaTime)) {
				if (hit.collider.tag == "Finish") {
					GameManager.instance.WinGame ();
				}
			} else {
				transform.localPosition = transform.localPosition - transform.forward * MovementSpeed * Time.deltaTime;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Trap") {
			trapped (col.GetComponent<Trap>());	
		} else if (col.tag == "Bonus") {
			giveBonus (col.GetComponent <Bonus> ());
		}	
	}

	void giveBonus(Bonus bonus) {
		StartCoroutine (GiveBonus (bonus));
	}

	public IEnumerator GiveBonus(Bonus bonus)
	{
		MovementSpeed  = MovementSpeed + bonus.incrementSpeed * SpeedMultiplier;
		RotationSpeed = RotationSpeed + (bonus.incrementSpeed * SpeedMultiplier) * 5;
		yield return new WaitForSeconds (5 * bonus.duration);
		MovementSpeed = MovementSpeed - bonus.incrementSpeed * SpeedMultiplier;
		RotationSpeed = RotationSpeed - (bonus.incrementSpeed * SpeedMultiplier) * 5;
	}

	void trapped(Trap trap) {
		StartCoroutine (MakeItBlur (trap));
		StartCoroutine (MakeItShake (trap));
	}

	public IEnumerator MakeItBlur(Trap trap)
	{
		TrapsApplyCount++;
		blurEffect.enabled = true;
		MovementSpeed  = MovementSpeed - trap.reduceSpeed * SpeedMultiplier;
		RotationSpeed = RotationSpeed - (trap.reduceSpeed * SpeedMultiplier) * 5;
		yield return new WaitForSeconds (5 * trap.duration);

		TrapsApplyCount--;
		if (TrapsApplyCount == 0) 
		{
			blurEffect.enabled = false;
		}
		MovementSpeed = MovementSpeed + trap.reduceSpeed * SpeedMultiplier;
		RotationSpeed = RotationSpeed + (trap.reduceSpeed * SpeedMultiplier) * 5;
	}
		
	public IEnumerator MakeItShake(Trap trap)
	{
		shakeEffect.shakeDuration = ShakeDuration;
		shakeEffect.enabled = true;
		yield return new WaitForSeconds (ShakeDuration);

		shakeEffect.enabled = false;
	}
}
