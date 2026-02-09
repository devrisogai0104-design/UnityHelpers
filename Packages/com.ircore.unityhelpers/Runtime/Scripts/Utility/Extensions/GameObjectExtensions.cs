using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRCore.UnityHelpers
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// GetComponentを実行し、コンポーネントが存在しない場合はAddComponentする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject target) where T : Component
        {
            var comp = target.GetComponent<T>();
            if (comp == null)
            {
                comp = target.AddComponent<T>();
            }

            return comp;
        }

        /// <summary>
        /// 対象のコンポーネントが付与されているかどうかを返却する
        /// </summary>
        public static bool HasComponent<T>(this GameObject target) where T : Component
        {
            return target.TryGetComponent<T>(out _);
        }

        /// <summary>
        /// ゲームオブジェクトの破棄
        /// </summary>
        public static void Destroy(this GameObject self)
        {
            GameObject.Destroy(self);
        }

        /// <summary>
        /// ゲームオブジェクトを即座に破棄
        /// </summary>
        public static void DestroyImmediate(this Component self)
        {
            GameObject.DestroyImmediate(self);
        }

        /// <summary>
        /// 新しいシーンを読み込む時に自動で破棄されないようにします
        /// </summary>
        public static void DontDestroyOnLoad(this GameObject self)
        {
            GameObject.DontDestroyOnLoad(self);
        }

        /// <summary>
        /// コンポーネントを削除する
        /// </summary>
        public static void RemoveComponent<T>(this GameObject self) where T : Component
        {
            GameObject.Destroy(self.GetComponent<T>());
        }

        /// <summary>
        /// 孫オブジェクトを除くすべての子オブジェクトを返却する
        /// </summary>
        public static GameObject[] GetChildrenWithoutGrandchild(
            this GameObject self
        )
        {
            var result = new List<GameObject>();
            foreach (Transform n in self.transform)
            {
                result.Add(n.gameObject);
            }
            return result.ToArray();
        }

        /// <summary>
        /// すべての子オブジェクトを返却する
        /// </summary>
        /// <param name="self">GameObject 型のインスタンス</param>
        /// <param name="includeInactive">非アクティブなオブジェクトも取得する場合 true</param>
        /// <returns>すべての子オブジェクトを管理する配列</returns>
        public static GameObject[] GetChildren(
            this GameObject self,
            bool includeInactive = false)
        {
            return self.GetComponentsInChildren<Transform>(includeInactive)
                .Where(c => c != self.transform)
                .Select(c => c.gameObject)
                .ToArray();
        }

        /// <summary>
        /// インターフェイスを指定して子オブジェクトから複数のコンポーネントを取得します
        /// </summary>
        public static T[] GetInterfacesOfComponentInChildren<T>(
            this GameObject self,
            bool includeInactive
        ) where T : class
        {
            var result = new List<T>();
            foreach (var n in self.GetComponentsInChildren<Component>(includeInactive))
            {
                var component = n as T;
                if (component != null)
                {
                    result.Add(component);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// インターフェイスを指定して子オブジェクトから複数のコンポーネントを取得します
        /// </summary>
        public static T[] GetInterfacesOfComponentInChildren<T>(this GameObject self) where T : class
        {
            return self.GetInterfacesOfComponentInChildren<T>(false);
        }

        /// <summary>
        /// インターフェイスを指定して子オブジェクトからコンポーネントを取得します
        /// </summary>
        public static T GetInterfaceOfComponentInChildren<T>(
            this GameObject self,
            bool includeInactive
        ) where T : class
        {
            foreach (var n in self.GetComponentsInChildren<Component>(includeInactive))
            {
                var component = n as T;
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }

        /// <summary>
        /// インターフェイスを指定して子オブジェクトからコンポーネントを取得します
        /// </summary>
        public static T GetInterfaceOfComponentInChildren<T>(this GameObject self) where T : class
        {
            return self.GetInterfaceOfComponentInChildren<T>(false);
        }

        /// <summary>
        /// インターフェイスを指定して複数のコンポーネントを取得します
        /// </summary>
        public static T[] GetInterfacesOfComponent<T>(this GameObject self) where T : class
        {
            var result = new List<T>();
            foreach (var n in self.GetComponents<Component>())
            {
                var component = n as T;
                if (component != null)
                {
                    result.Add(component);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// インターフェイスを指定してコンポーネントを取得します
        /// </summary>
        public static T GetInterfaceOfComponent<T>(this GameObject self) where T : class
        {
            foreach (var n in self.GetComponents<Component>())
            {
                var component = n as T;
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, GameObject target)
        {
            self.transform.LookAt(target.transform);
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, Transform target)
        {
            self.transform.LookAt(target);
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, Vector3 worldPosition)
        {
            self.transform.LookAt(worldPosition);
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, GameObject target, Vector3 worldUp)
        {
            self.transform.LookAt(target.transform, worldUp);
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, Transform target, Vector3 worldUp)
        {
            self.transform.LookAt(target, worldUp);
        }

        /// <summary>
        /// 向きを変更します
        /// </summary>
        public static void LookAt(this GameObject self, Vector3 worldPosition, Vector3 worldUp)
        {
            self.transform.LookAt(worldPosition, worldUp);
        }

        /// <summary>
        /// 位置を設定します
        /// </summary>
        public static void SetPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.position = position;
        }

        /// <summary>
        /// X座標を設定します
        /// </summary>
        public static void SetPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetPositionX(x);
        }

        /// <summary>
        /// Y座標を設定します
        /// </summary>
        public static void SetPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetPositionY(y);
        }

        /// <summary>
        /// Z座標を設定します
        /// </summary>
        public static void SetPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetPositionZ(z);
        }

        /// <summary>
        /// X座標に加算します
        /// </summary>
        public static void AddPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddPositionX(x);
        }

        /// <summary>
        /// Y座標に加算します
        /// </summary>
        public static void AddPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddPositionY(y);
        }

        /// <summary>
        /// Z座標に加算します
        /// </summary>
        public static void AddPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddPositionZ(z);
        }

        /// <summary>
        /// ローカル座標系の位置を設定します
        /// </summary>
        public static void SetLocalPosition(this GameObject gameObject, Vector3 localPosition)
        {
            gameObject.transform.localPosition = localPosition;
        }

        /// <summary>
        /// ローカル座標系のX座標を設定します
        /// </summary>
        public static void SetLocalPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalPositionX(x);
        }

        /// <summary>
        /// ローカル座標系のY座標を設定します
        /// </summary>
        public static void SetLocalPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalPositionY(y);
        }

        /// <summary>
        /// ローカルのZ座標を設定します
        /// </summary>
        public static void SetLocalPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalPositionZ(z);
        }

        /// <summary>
        /// ローカル座標系のX座標に加算します
        /// </summary>
        public static void AddLocalPositionX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalPositionX(x);
        }

        /// <summary>
        /// ローカル座標系のY座標に加算します
        /// </summary>
        public static void AddLocalPositionY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalPositionY(y);
        }

        /// <summary>
        /// ローカル座標系のZ座標に加算します
        /// </summary>
        public static void AddLocalPositionZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalPositionZ(z);
        }

        /// <summary>
        /// 回転角を設定します
        /// </summary>
        public static void SetEulerAngle(this GameObject gameObject, Vector3 eulerAngles)
        {
            gameObject.transform.eulerAngles = eulerAngles;
        }

        /// <summary>
        /// X軸方向の回転角を設定します
        /// </summary>
        public static void SetEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetEulerAngleX(x);
        }

        /// <summary>
        /// Y軸方向の回転角を設定します
        /// </summary>
        public static void SetEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetEulerAngleY(y);
        }

        /// <summary>
        /// Z軸方向の回転角を設定します
        /// </summary>
        public static void SetEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetEulerAngleZ(z);
        }

        /// <summary>
        /// X軸方向の回転角を加算します
        /// </summary>
        public static void AddEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddEulerAngleX(x);
        }

        /// <summary>
        /// Y軸方向の回転角を加算します
        /// </summary>
        public static void AddEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddEulerAngleY(y);
        }

        /// <summary>
        /// Z軸方向の回転角を加算します
        /// </summary>
        public static void AddEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddEulerAngleZ(z);
        }

        /// <summary>
        /// ローカル座標系の回転角を設定します
        /// </summary>
        public static void SetLocalEulerAngle(this GameObject gameObject, Vector3 localEulerAngles)
        {
            gameObject.transform.localEulerAngles = localEulerAngles;
        }

        /// <summary>
        /// ローカル座標系のX軸方向の回転角を設定します
        /// </summary>
        public static void SetLocalEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalEulerAngleX(x);
        }

        /// <summary>
        /// ローカル座標系のY軸方向の回転角を設定します
        /// </summary>
        public static void SetLocalEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalEulerAngleY(y);
        }

        /// <summary>
        /// ローカル座標系のZ軸方向の回転角を設定します
        /// </summary>
        public static void SetLocalEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalEulerAngleZ(z);
        }

        /// <summary>
        /// ローカル座標系のX軸方向の回転角を加算します
        /// </summary>
        public static void AddLocalEulerAngleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalEulerAngleX(x);
        }

        /// <summary>
        /// ローカル座標系のY軸方向の回転角を加算します
        /// </summary>
        public static void AddLocalEulerAngleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalEulerAngleY(y);
        }

        /// <summary>
        /// ローカル座標系のX軸方向の回転角を加算します
        /// </summary>
        public static void AddLocalEulerAngleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalEulerAngleZ(z);
        }

        /// <summary>
        /// ローカル座標系の回転角を設定します
        /// </summary>
        public static void SetLocalScale(this GameObject gameObject, Vector3 localScale)
        {
            gameObject.transform.localScale = localScale;
        }

        /// <summary>
        /// X軸方向のローカル座標系のスケーリング値を設定します
        /// </summary>
        public static void SetLocalScaleX(this GameObject gameObject, float x)
        {
            gameObject.transform.SetLocalScaleX(x);
        }

        /// <summary>
        /// Y軸方向のローカル座標系のスケーリング値を設定します
        /// </summary>
        public static void SetLocalScaleY(this GameObject gameObject, float y)
        {
            gameObject.transform.SetLocalScaleY(y);
        }

        /// <summary>
        /// Z軸方向のローカル座標系のスケーリング値を設定します
        /// </summary>
        public static void SetLocalScaleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.SetLocalScaleZ(z);
        }

        /// <summary>
        /// X軸方向のローカル座標系のスケーリング値を加算します
        /// </summary>
        public static void AddLocalScaleX(this GameObject gameObject, float x)
        {
            gameObject.transform.AddLocalScaleX(x);
        }

        /// <summary>
        /// Y軸方向のローカル座標系のスケーリング値を加算します
        /// </summary>
        public static void AddLocalScaleY(this GameObject gameObject, float y)
        {
            gameObject.transform.AddLocalScaleY(y);
        }

        /// <summary>
        /// Z軸方向のローカル座標系のスケーリング値を加算します
        /// </summary>
        public static void AddLocalScaleZ(this GameObject gameObject, float z)
        {
            gameObject.transform.AddLocalScaleZ(z);
        }

        /// <summary>
        /// 親オブジェクトを設定します
        /// </summary>
        public static void SetParent(this GameObject gameObject, Transform parent)
        {
            gameObject.transform.parent = parent;
        }

        /// <summary>
        /// 親オブジェクトを設定します
        /// </summary>
        public static void SetParent(this GameObject gameObject, GameObject parent)
        {
            gameObject.transform.parent = parent.transform;
        }
    }
}