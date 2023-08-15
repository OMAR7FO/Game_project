using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	[SerializeField] Button button;
	[SerializeField] bool isPressedAnimationPlaying = false;

	// Update is called once per frame
	void Update()
	{
		if (menuButtonController.index == thisIndex)
		{
			animator.SetBool("selected", true);
			if (Input.GetAxis("Submit") == 1 || Input.GetMouseButtonDown(0))
			{
				animator.SetBool("pressed", true);
				isPressedAnimationPlaying = true; // Set the variable to true when the animation starts
			}
			else if (animator.GetBool("pressed"))
			{
				animator.SetBool("pressed", false);
				animatorFunctions.disableOnce = true;
				isPressedAnimationPlaying = false; // Set the variable to false when the animation ends
			}
		}
		else
		{
			animator.SetBool("selected", false);
		}

		if (menuButtonController.index == thisIndex && animator.GetBool("pressed"))
		{
			button.onClick.Invoke();
		}
	}
	private void OnKeyDown(KeyCode key)
	{
		// Trigger the onClick event when the Space/Enter key is pressed.
		if (key == KeyCode.Space || key == KeyCode.Return)
		{
			button.onClick.Invoke();
		}
	}
}
