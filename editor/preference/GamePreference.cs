/** == GamePreference.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/11/01 15:18:47
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using System.Reflection;
using UniKh.core;
using UnityEngine;
using UnityEditor;

namespace UniKh.editor {
    public class GamePreference : MonoBehaviour {
        public static string GetTerminalPath() {
            return EditorPrefs.GetString("terminalPath", "");
        }

        [PreferenceItem("UniKh/Preferences")]
        public static void DrawUniKhPreferences() {
            var configList =
                AssetDatabase.LoadAssetAtPath<KhPreferenceStatic>($"Assets/Resources/{KhPreferenceStatic.assetName}.asset");
            if (configList == null) {
                EditorGUILayout.HelpBox("Cannot Found Config List !", MessageType.Warning);
                if (!GUILayout.Button("Create Config File")) return;

                var so = ScriptableObject.CreateInstance<KhPreferenceStatic>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources")) {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateAsset(so, $"Assets/Resources/{KhPreferenceStatic.assetName}.asset");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = so;
                return;
            }

            EditorGUILayout.ObjectField("config object", configList, typeof(KhPreferenceStatic), false);
            var soConfig = new SerializedObject(configList);
            // display serializedProperty with selected mode
            var type = typeof(SerializedObject);
            var propertyInfo = type.GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo != null) {
                propertyInfo.SetValue(soConfig, InspectorMode.Normal, null);
            }
            // _targetObject.inspectorMode = this.m_InspectorMode;

//                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            var iterator = soConfig.GetIterator();
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
            }

            soConfig.ApplyModifiedProperties();


//                EditorGUILayout.EndScrollView();
        }
    }
}