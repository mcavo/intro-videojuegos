using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public Color selectedColor = new Color(0.960784f, 0.486274f, 0.960784f, 1f);
	public Color clickedColor = new Color(0.831373f, 0.419608f, 0.639216f, 1f);

	public Text text;

	private Color originalColor;

	void Start() {
		originalColor = text.color;
	}

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter (PointerEventData eventData) 
	{
		text.color = selectedColor;
	}

	//Do this when the cursor exits the rect area of this selectable UI object.
	public void OnPointerExit (PointerEventData eventData) 
	{
		text.color = originalColor;
	}

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown (PointerEventData eventData) 
	{
		text.color = clickedColor;
	}
}