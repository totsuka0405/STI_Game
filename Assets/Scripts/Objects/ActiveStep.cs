using UnityEngine;

namespace Game.Objects
{
    /// <summary>
    /// 特定のPrefabインスタンスが存在する間だけ targetObject をアクティブにする制御。
    /// </summary>
    public class ActiveStep : MonoBehaviour
    {
        [Header("参照設定")]
        [SerializeField] private GameObject prefabToTrack;   // 監視対象のPrefab
        [SerializeField] private GameObject targetObject;    // Active/Inactiveを切り替える対象

        private GameObject trackedInstance; // 実際に生成されたPrefabのインスタンス

        private void Update()
        {
            // 参照が切れている場合は再探索
            if (trackedInstance == null)
            {
                trackedInstance = GameObject.Find(prefabToTrack.name + "(Clone)");
            }

            // 対象オブジェクトの状態を切り替え
            if (targetObject != null)
            {
                targetObject.SetActive(trackedInstance != null);
            }
        }

        /// <summary>
        /// インスタンス生成時に追跡対象を設定。
        /// </summary>
        public void SetTrackedInstance(GameObject instance)
        {
            trackedInstance = instance;
        }

        /// <summary>
        /// 追跡対象が破壊されたときに参照を解除。
        /// </summary>
        public void ClearTrackedInstance()
        {
            trackedInstance = null;
        }
    }
}
