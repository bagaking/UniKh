using System;
using System.Diagnostics;
using Boo.Lang.Environments;
using UniKh.core;
using UniKh.extensions;
using UniKh.utils;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace UniKh.editor {
    public class EditorMenuItem {
        [MenuItem("UniKh/Tools/Open Terminal %#`")]
        public static void OpenTerminal() {
            var path = ToolPreference.GetTerminalPath();
            if (path.Exists()) {
                Process.Start(path);
            } else {
                Debug.Log("Terminal path are not set, please set it in Edit/Preferences/UniKh/Tool");
            }
        }

        [MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<KhPreferenceStatic>();
            AssetDatabase.CreateAsset(so, $"Assets/Resources/{KhPreferenceStatic.assetName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = so;
        }

        [MenuItem("UniKh/Utils/Merge Meshes Under Active Object")]
        public static void MergeSelectedMeshes() {
            if (!Selection.activeGameObject) {
                Debug.Log("Please Select The Root Node.");
            }

            var mergedRenderer = MeshMerger.Batch(
                Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>(),
                Selection.activeGameObject.transform
            );
            var assetName = "new-mesh@" + DateTime.Now.GetTimeStamp();
            var prefabObj = new GameObject(assetName);
            prefabObj.transform.position = Selection.activeGameObject.transform.position;
            mergedRenderer.ForEach(
                (mr, i) => {
                    var mf = mr.GetComponent<MeshFilter>();
                    var fullAssetName = assetName + "_" + i ;
                    AssetDatabase.CreateAsset(mf.sharedMesh, "Assets/" + fullAssetName + ".asset");
                    AssetDatabase.SaveAssets();
                    var mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/" + fullAssetName + ".asset");
                    mf.sharedMesh = mesh;
                    mr.transform.parent = prefabObj.transform;
                }
            );
            PrefabUtility.SaveAsPrefabAsset(prefabObj, "Assets/" + assetName + ".prefab");
            var obj = PrefabUtility.InstantiatePrefab(PrefabUtility.LoadPrefabContents("Assets/" + assetName + ".prefab")) as GameObject;
            obj.transform.position = prefabObj.transform.position;
            Object.DestroyImmediate(prefabObj);
        }
    }
}