using UnityEngine;

public class PlayerDebugDevTools : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("Keybinds")]
    [SerializeField] private KeyCode invincibilityKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode addExpAttack = KeyCode.Alpha2;
    [SerializeField] private KeyCode addExpDefense = KeyCode.Alpha3;
    [SerializeField] private KeyCode addExpMobility = KeyCode.Alpha4;
    [SerializeField] private KeyCode changeAmbientColor = KeyCode.Tab;

    private bool _invincible;
    public bool Invincible { get => _invincible; set => _invincible = value; }

    private void Update()
    {
        DebugInput();
    }

    private void DebugInput()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(invincibilityKey))
            {
                Debug.Log("Debug Tools: Invincible Toggle");
                Invincible = Invincible.Toggle();
            }

            if (Input.GetKeyDown(addExpAttack))
            {
                Debug.Log("Debug Tools: Add Exp Attack");
                player.AddExp(PlayerLevel.ExpType.Attack, 5);
            }

            if (Input.GetKeyDown(addExpDefense))
            {
                Debug.Log("Debug Tools: Add Exp Defense");
                player.AddExp(PlayerLevel.ExpType.Defense, 5);
            }

            if (Input.GetKeyDown(addExpMobility))
            {
                Debug.Log("Debug Tools: Add Exp Mobility");
                player.AddExp(PlayerLevel.ExpType.Mobility, 5);
            }
        }

        if (Input.GetKeyDown(changeAmbientColor))
        {
            ChangeAmbientColor.i.IterateThroughColors();
            
        }
        
    }
}