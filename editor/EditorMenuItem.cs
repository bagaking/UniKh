using System.Collections.Generic;
using System.Diagnostics;

using UniKh.comp.ui;
using UniKh.core;
using UniKh.extensions;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class EditorMenuItem {

        [MenuItem("UniKh/Tools/Open Terminal")]
        public static void OpenTerminal() {
            var path = EditorPreference.GetTerminalPath();
            if (path.Exists()) {
                Process.Start(path);
            }
        }
        
        [MenuItem("UniKh/Create/Config List")]
        public static void CreateLst() {
            var so = ScriptableObject.CreateInstance<ConfigList>();
            
            UnityEditor.AssetDatabase.CreateAsset(so, $"Assets/Resources/{ConfigList.assetName}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow ();
            UnityEditor.Selection.activeObject = so;
        }

        private static string GetNewTransformName(Transform parent, string title = "c") {
            if (parent == null) {
                return title;
            }
            var children = parent.GetDirectChildren(n => n.name.StartsWith(title + ":"));
            if (children.Count <= 0) return title + ":00";
            var t = children[children.Count - 1];
            return t.name.Replace(' ', '_').IncNumberTail(2);
        }

        private static GameObject CreateNewObject(Transform transParent, string title) { 
            if (null == transParent) { return null; }
            var go = new GameObject(GetNewTransformName(transParent, title));
            go.transform.SetParent(transParent);
            go.transform.localPosition = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;

            Selection.activeObject = go; 
            return go;
        }
                
        [MenuItem("GameObject/Kh UI Component/c <Empty>", false, 0)]
        static void CreateUINodeEmpty(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            CreateNewObject(null != goParent ? goParent.transform : null, "c");
        }
        
        [MenuItem("GameObject/Kh UI Component/i <Image>", false, 0)]
        static void CreateUINodeImage(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "i");
            go.AddComponent<KhImage>();
        }
        
        [MenuItem("GameObject/Kh UI Component/p <Panel>", false, 0)]
        static void CreateUINodePanel(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "p");
            
            go.AddComponent<CanvasGroup>();
            go.AddComponent<KhPanel>();
            go.AddComponent<KhImage>();

            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

        }
        
        [MenuItem("GameObject/Kh UI Component/toast <KhToast>", false, 0)]
        static void CreateUINodeToast(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "toast");
            
            go.AddComponent<CanvasGroup>();
            go.AddComponent<KhToast>();
            go.AddComponent<KhImage>();
            var cg = go.GetComponent<CanvasGroup>();
            var khToast = go.GetComponent<KhToast>();
            khToast.cg = cg;
        }
        [MenuItem("GameObject/Kh UI Component/a <Text>", false, 0)]
        static void CreateUINodeText(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "a");
            
            go.AddComponent<KhText>();
        }
        
        [MenuItem("GameObject/Kh UI Component/btn <Button>", false, 0)]
        static void CreateUINodeBtn(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "btn");
            
            go.AddComponent<KhBtn>();
            go.AddComponent<KhImage>();

        }
        [MenuItem("GameObject/Kh UI Component/tabsItem <TabsItem>",false,0)]
        static void CreateUINodeTabsItem(MenuCommand mc)
        {
            var goParent = (mc.context as GameObject);
            if (null == goParent) return;
            
            var khTabs = goParent.GetComponent<KhTabs>();
            if (null == khTabs) return;
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "tabsItem");
            go.AddComponent<KhTabsItem>();
            go.AddComponent<KhImage>();
            var active = CreateNewObject(null != go ? go.transform : null, "active");
            var unActive = CreateNewObject(null != go ? go.transform : null, "unActive");
            active.SetActive(false);
            unActive.SetActive(false);
            var khTabsItem = go.GetComponent<KhTabsItem>();
            khTabsItem.pActive = active;
            khTabsItem.pUnActive = unActive;

        }
        
        [MenuItem("GameObject/Kh UI Component/tabs <Tabs>",false,0)]
        static void CreateUINodeTabs(MenuCommand mc)
        {
            var goParent = (mc.context as GameObject);
            var go = CreateNewObject(null != goParent ? goParent.transform : null, "tabs");
            go.AddComponent<KhTabs>();
            go.AddComponent<KhImage>();
        }
        
        
    }
}