using UnityEngine.InputSystem;

public static class InputManager
{
    public static PowerInputActions PlayerInput;

    public static InputAction PlayerMove;
    //public static InputAction PlayerDodge;
    public static InputAction PlayerParry;
    public static InputAction PlayerAttack;
    public static InputAction PlayerChangeSpreadAngle;
    public static InputAction PreciseMovement;
    public static InputAction HideHelp;
    static InputManager()
    {
        PlayerInput = new PowerInputActions();

        PlayerMove = PlayerInput.Player.Move;
        PlayerMove.Enable();

        //PlayerDodge = PlayerInput.Player.Dodge;
        //PlayerDodge.Enable();

        PlayerParry = PlayerInput.Player.Parry;
        PlayerParry.Enable();

        PlayerAttack = PlayerInput.Player.Attack;
        PlayerAttack.Enable();

        PlayerChangeSpreadAngle = PlayerInput.Player.ChangeSpreadAngle;
        PlayerChangeSpreadAngle.Enable();

        PreciseMovement = PlayerInput.Player.PreciseMovement;
        PreciseMovement.Enable();

        HideHelp = PlayerInput.Player.HideHelp;
        HideHelp.Enable();

    }
}
