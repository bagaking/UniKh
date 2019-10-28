/** == ContextMenuUI.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/26 00:22:14
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;

namespace UniKh.editor {
    public class ContextMenuUI {
        internal static string GetNewTransformName(Transform parent, string title = "c", string defaultTag = "00") {
            if (parent == null) {
                return title;
            }

            var children = parent.GetDirectChildren(n => n.name.StartsWith(title + ":"));
            if (children.Count <= 0) return title + (defaultTag.Exists() ? ":" + defaultTag : defaultTag);
            var t = children[children.Count - 1];
            return t.name.Replace(' ', '_').IncNumberTail(2);
        }

        internal static GameObject CreateNewUIObject(Transform transParent, string title, string defaultTag = "00") {
            var go = new GameObject(GetNewTransformName(transParent, title, defaultTag));
            if (null != transParent) {
                go.transform.SetParent(transParent);
            }
            go.transform.localPosition = Vector3.zero;
            go.transform.eulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            if (transParent is RectTransform) {
                go.AddComponent<RectTransform>();
            }

            Selection.activeObject = go;
            return go;
        }

        internal static RectTransform checkUIContext(GameObject goParent, string opration = "Context") {
            if (goParent == null) throw new Exception(opration + " Failed: MUST given parent");
            var rectTransParent = goParent.transform as RectTransform;
            if (rectTransParent == null) throw new Exception(opration + " Failed: MUST be created in a UI Context");
            return rectTransParent;
        }
        
        [MenuItem("GameObject/Kh UI Components/i <Image>", false, 0)]
        internal static void CreateUINodeImage(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewUIObject(null != goParent ? goParent.transform : null, "i");
            go.AddComponent<KhImage>();
        }

        [MenuItem("GameObject/Kh UI Components/p <Panel>", false, 0)]
        internal static void CreateUINodePanel(MenuCommand mc) {
            var goParent = (mc.context as GameObject);

            var go = CreateNewUIObject(null != goParent ? goParent.transform : null, "p");

            go.AddComponent<CanvasGroup>();
            go.AddComponent<KhPanel>();
            go.AddComponent<KhImage>();

            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        [MenuItem("GameObject/Kh UI Components/toast <KhToast>", false, 0)]
        internal static void CreateUINodeToast(MenuCommand mc) {
            var goParent = (mc.context as GameObject);

            var go = CreateNewUIObject(null != goParent ? goParent.transform : null, "toast");

            go.AddComponent<CanvasGroup>();
            go.AddComponent<KhToast>();
            go.AddComponent<KhImage>();
            var cg = go.GetComponent<CanvasGroup>();
            var khToast = go.GetComponent<KhToast>();
            khToast.cg = cg;
        }

        [MenuItem("GameObject/Kh UI Components/a <Text>", false, 0)]
        internal static void CreateUINodeText(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewUIObject(null != goParent ? goParent.transform : null, "a");

            go.AddComponent<KhText>();
        }

        [MenuItem("GameObject/Kh UI Components/btn <Button>", false, 0)]
        internal static void CreateUINodeBtn(MenuCommand mc) {
            var goParent = (mc.context as GameObject);
            var go = CreateNewUIObject(null != goParent ? goParent.transform : null, "btn");

            go.AddComponent<KhBtn>();
            go.AddComponent<KhImage>();
        }
    }
}