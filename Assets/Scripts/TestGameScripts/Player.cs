using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private InteractionHandler interactionHandler;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Movement movement;
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private TestUIManager uiManager;

    private bool isAlive = true;

    private void Start()
    {
        if (!isAlive)
        {
            Respawn();
        }
    }

    private void Update()
    {
        if (!isAlive) return;

        playerCamera.View();
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // 他のスクリプトに指示を渡す
        HandleInput();
        
    }

    private void HandleInput()
    {
        // 入力を取得
        Vector2 movementInput = inputHandler.GetMovementInput();

        // 移動を実行
        movement.Move(movementInput.x, movementInput.y);
    }

    private void HandleInteraction()
    {
        if (inputHandler.IsInteractKeyPressed())
        {
            interactionHandler.Interact();
        }
    }

    public void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        deathHandler.HandleDeath();
        //uiManager.ShowNotification("You have died!");
    }

    public void Respawn()
    {
        isAlive = true;
        deathHandler.Respawn();
        //uiManager.ShowNotification("Respawned!");
    }

    public void AddItemToInventory(Item item)
    {
        inventoryManager.AddItem(item);
        //uiManager.UpdateInventoryUI(inventoryManager.GetInventory());
    }
}
