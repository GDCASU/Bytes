/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader: DescriptionSO
{
    GameInput _input;

	public GameInput.GameplayActions Gameplay => _input.Gameplay;
	public GameInput.MenusActions Menus => _input.Menus;

    private void OnEnable()
    {
        if (_input == null)
        {
			_input = new GameInput();
			_input.Enable();
		}
	}

	public void EnableGameplayInput()
	{
		_input.Gameplay.Enable();
		_input.Menus.Disable();
	}

	public void EnableMenuInput()
	{
		_input.Gameplay.Disable();
		_input.Menus.Enable();
	}
}

