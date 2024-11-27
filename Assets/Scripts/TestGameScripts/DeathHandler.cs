using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public void HandleDeath()
    {
        Debug.Log("Player has died. Handle death process here.");
        // 例: アニメーション、サウンド再生
    }

    public void Respawn()
    {
        Debug.Log("Player is respawning.");
        transform.position = respawnPoint.position;
        // その他リセット処理
    }
}
