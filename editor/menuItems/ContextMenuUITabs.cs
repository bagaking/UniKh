/** == ContextMenuUITabs.cs ==
 *  Author:         bagaking <kinghand@foxmail.com>
 *  CreateTime:     2019/10/26 00:31:33
 *  Copyright:      (C) 2019 - 2029 bagaking, All Rights Reserved
 */

using System;
using UniKh.extensions;
using UniKh.comp.ui;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.editor {
    public class ContextMenuUITabs : ContextMenuUI {
        private static float itemWidth = 160;
        private static float itemHeight = 100;
        private static float initItemCount = 2;
        private static Vector2 spacing = new Vector2(8, 8);


        static RectTransform CreateUINodeTabs(MenuCommand mc, string appendName = "") {
            var rectTransParent = checkUIContext(mc.context as GameObject, "Create UI Node Tabs");
            var go = CreateNewUIObject(rectTransParent, "tabs" + (appendName.Exists() ? (":" + appendName) : ""));
            go.AddComponent<KhTabs>();
            var img = go.AddComponent<KhImage>();
            img.color = new Color(0.2f, 0.6f, 0.85f);

            var rectTrans = go.transform as RectTransform;
            if (null == rectTrans) {
                throw new Exception("Create UI Node Tabs Failed: try create as a UI component failed");
            }

            return rectTrans;
        }

        static void CreateUINodeTabsItem(RectTransform transTabs) {
            if (null == transTabs) return;

            var go = CreateNewUIObject(transTabs, "tabsItem");
            go.AddComponent<KhTabsItem>();
            var img = go.AddComponent<KhImage>();
            img.color = new Color(0.85f, 0.5f, 0.2f);

            var rectTabsItem = go.transform as RectTransform;
            if (null != rectTabsItem) rectTabsItem.sizeDelta = new Vector2(itemWidth, itemHeight);

            var active = CreateNewUIObject(null != go ? go.transform : null, "active");
            var unActive = CreateNewUIObject(null != go ? go.transform : null, "unActive");
            active.SetActive(false);
            unActive.SetActive(false);
            var khTabsItem = go.GetComponent<KhTabsItem>();
            khTabsItem.pActive = active;
            khTabsItem.pUnActive = unActive;

            Selection.activeObject = transTabs;
        }

        [MenuItem("GameObject/Kh UI Components/tabs <Tabs>/Custom", false, 0)]
        static void CreateUINodeTabsGeneral(MenuCommand mc) {
            var rectTabs = CreateUINodeTabs(mc, "custom");
            rectTabs.sizeDelta = new Vector2(itemWidth + spacing.x, (itemHeight + spacing.y) * initItemCount) + spacing;

            var layoutGroup = rectTabs.GetOrAdd<VerticalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.padding.left = Mathf.FloorToInt(spacing.x);
            layoutGroup.padding.top = Mathf.FloorToInt(spacing.y);
            layoutGroup.spacing = Mathf.FloorToInt(spacing.y);
            for (var i = initItemCount; i-- > 0;) {
                CreateUINodeTabsItem(rectTabs);
            }
        }

        [MenuItem("GameObject/Kh UI Components/tabs <Tabs>/Anchor To Top", false, 0)]
        static void CreateUINodeTabsTop(MenuCommand mc) {
            var rectTabs = CreateUINodeTabs(mc, "top");
            rectTabs.anchorMin = Vector2.up;
            rectTabs.anchorMax = Vector2.one;
            rectTabs.sizeDelta = new Vector2(0, itemHeight + spacing.y * 2);
            rectTabs.pivot = new Vector2(0.5f, 1);
            CreateUINodeTabsItem(rectTabs);
            var layoutGroup = rectTabs.GetOrAdd<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.padding.left = Mathf.FloorToInt(spacing.x);
            layoutGroup.padding.top = Mathf.FloorToInt(spacing.y);
            layoutGroup.spacing = Mathf.FloorToInt(spacing.y);
            for (var i = initItemCount; i-- > 0;) {
                CreateUINodeTabsItem(rectTabs);
            }
        }

        [MenuItem("GameObject/Kh UI Components/tabs <Tabs>/Anchor To Bottom", false, 0)]
        static void CreateUINodeTabsBottom(MenuCommand mc) {
            var rectTabs = CreateUINodeTabs(mc, "bottom");
            rectTabs.anchorMin = Vector2.zero;
            rectTabs.anchorMax = Vector2.right;
            rectTabs.sizeDelta = new Vector2(0, itemHeight + spacing.y * 2);
            rectTabs.pivot = Vector2.right * 0.5f;
            CreateUINodeTabsItem(rectTabs);
            var layoutGroup = rectTabs.GetOrAdd<HorizontalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.padding.left = Mathf.FloorToInt(spacing.x);
            layoutGroup.padding.top = Mathf.FloorToInt(spacing.y);
            layoutGroup.spacing = Mathf.FloorToInt(spacing.y);
            for (var i = initItemCount; i-- > 0;) {
                CreateUINodeTabsItem(rectTabs);
            }
        }


        [MenuItem("GameObject/Kh UI Components/tabs <Tabs>/Anchor To Left", false, 0)]
        static void CreateUINodeTabsLeft(MenuCommand mc) {
            var rectTabs = CreateUINodeTabs(mc, "left");
            rectTabs.anchorMin = Vector2.zero;
            rectTabs.anchorMax = Vector2.up;
            rectTabs.sizeDelta = new Vector2(itemWidth + spacing.x * 2, 0);
            rectTabs.pivot = Vector2.up * 0.5f;
            var layoutGroup = rectTabs.GetOrAdd<VerticalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.padding.left = Mathf.FloorToInt(spacing.x);
            layoutGroup.padding.top = Mathf.FloorToInt(spacing.y);
            layoutGroup.spacing = Mathf.FloorToInt(spacing.x);
            for (var i = initItemCount; i-- > 0;) {
                CreateUINodeTabsItem(rectTabs);
            }
        }

        [MenuItem("GameObject/Kh UI Components/tabs <Tabs>/Anchor To Right", false, 0)]
        static void CreateUINodeTabsRight(MenuCommand mc) {
            var rectTabs = CreateUINodeTabs(mc, "right");
            rectTabs.anchorMin = Vector2.right;
            rectTabs.anchorMax = Vector2.one;
            rectTabs.sizeDelta = new Vector2(itemWidth + spacing.x * 2, 0);
            rectTabs.pivot = new Vector2(1, 0.5f);
            var layoutGroup = rectTabs.GetOrAdd<VerticalLayoutGroup>();
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.padding.left = Mathf.FloorToInt(spacing.x);
            layoutGroup.padding.top = Mathf.FloorToInt(spacing.y);
            layoutGroup.spacing = Mathf.FloorToInt(spacing.x);
            for (var i = initItemCount; i-- > 0;) {
                CreateUINodeTabsItem(rectTabs);
            }
        }
    }
}