using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public Camera playerCamera;    // プレイヤーカメラ
    public float fallDuration = 1f; // カメラが倒れるまでの時間
    public Vector3 fallRotation = new Vector3(90f, 0f, 0f); // カメラが倒れる角度
    public Vector3 fallPositionOffset = new Vector3(0f, -1f, 0f); // カメラが倒れるときの位置のオフセット
    public CanvasGroup deathScreen; // 画面エフェクト用のCanvasGroup
    public float fadeDuration = 1f; // フェードイン/アウトの時間

    private bool isDead = false;

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (GameManager.instance.isFirstEarthDie)
        {
            Die();
        }

        if (GameManager.instance.isSecondEarthDie)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        StartCoroutine(DeathAnimation());
        yield return StartCoroutine(FadeToBlack());
        // 死亡後の追加演出やゲームオーバー画面への遷移をここに追加できます
        GameManager.instance.isPlayerDead = true;
    }

    private IEnumerator DeathAnimation()
    {
        Vector3 initialPosition = playerCamera.transform.localPosition;
        Quaternion initialRotation = playerCamera.transform.localRotation;

        Vector3 finalPosition = initialPosition + fallPositionOffset;
        Quaternion finalRotation = Quaternion.Euler(fallRotation);

        float elapsed = 0f;
        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            playerCamera.transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, t);
            playerCamera.transform.localRotation = Quaternion.Slerp(initialRotation, finalRotation, t);

            yield return null;
        }

        playerCamera.transform.localPosition = finalPosition;
        playerCamera.transform.localRotation = finalRotation;
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            deathScreen.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        deathScreen.alpha = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Die();
            GameManager.instance.isFireDie = true;
        }

        if (other.CompareTag("FirstEarthEventPos"))
        {
            GameManager.instance.isFirstEarthDontDie = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FirstEarthEventPos"))
        {
            GameManager.instance.isFirstEarthDontDie = false;
        }
    }
}
