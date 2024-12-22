using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStep : MonoBehaviour
{
    // 対象のPrefab
    public GameObject prefabToTrack;

    // Active/Inactiveを切り替える対象のゲームオブジェクト
    public GameObject targetObject;

    // 追跡中のPrefabインスタンス
    private GameObject trackedInstance;

    void Update()
    {
        // trackedInstance の状態を確認して破壊済みかチェック
        if (trackedInstance == null || trackedInstance.Equals(null))
        {
            // Prefabインスタンスを探す
            trackedInstance = GameObject.Find(prefabToTrack.name + "(Clone)");
        }

        // targetObject の状態を切り替え
        if (targetObject != null)
        {
            targetObject.SetActive(trackedInstance != null && !trackedInstance.Equals(null));
        }
        else
        {
            Debug.LogWarning("targetObject が設定されていません！");
        }
    }

    // InstantiateされたPrefabを追跡するためのメソッド
    public void SetTrackedInstance(GameObject instance)
    {
        trackedInstance = instance;
    }

    // TrackedInstanceが破壊されたときに参照をクリアする
    public void ClearTrackedInstance()
    {
        trackedInstance = null;
    }
}
