using UnityEngine;
public enum MouseCode
{
    Left = 0,
    Right = 1,
    Middle = 2
}
public class PlayerInput : MonoBehaviour
{    
    [Header("Assignables")]
    [SerializeField] private Player _player;

    [Header("Keybinds")]
    [SerializeField] private MouseCode  _shootKey   = MouseCode.Left;
    [SerializeField] private KeyCode    _jumpKey    = KeyCode.Space;
    [SerializeField] private KeyCode    _dashKey    = KeyCode.LeftShift;
    [SerializeField] private KeyCode    _restartKey = KeyCode.R;
    [SerializeField] private KeyCode    _freeCamKey = KeyCode.LeftControl;
    [SerializeField] private MouseCode  _railAttachKey = MouseCode.Left;

    //Input variables (Used by other scripts to run their actions)
    public float  x;
    public float  y;
    public bool   jumping;
    public bool   stoppedJumping;
    [HideInInspector] public bool   dashing;
    [HideInInspector] public bool   shooting;
    [HideInInspector] public bool   restart;
    [HideInInspector] public bool   freecam;
    [HideInInspector] public bool   isMoving;
    [HideInInspector] public bool   attaching;

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        isMoving = x != 0 || y != 0;
        jumping = Input.GetKeyDown(_jumpKey);
        stoppedJumping = Input.GetKeyUp(_jumpKey);
        dashing = Input.GetKeyDown(_dashKey);
        shooting = Input.GetMouseButton((int)_shootKey);
        restart = Input.GetKeyDown(_restartKey);
        freecam = Input.GetKey(_freeCamKey);
        attaching = Input.GetMouseButtonDown((int)_railAttachKey);
    }
}
