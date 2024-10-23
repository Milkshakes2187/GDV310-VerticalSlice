using UnityEngine;

public class UIPlayerHealthBarController : MonoBehaviour
{

    // Locally assigned Variables
    private Player player;

    [Header("Preassigned Variables")]
    public UIFillController healthBarFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.instance;
        if (!player)
        {
            Debug.LogError("Player not found, disabling health bar");
            enabled = false;
            return;
        }
    }

    // change to events eventually
    void Update()
    {
        if (player == null) return;
        healthBarFill.fillAmount = player.health / player.maxHealth;
    }
}
