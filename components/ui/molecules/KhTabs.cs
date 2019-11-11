using System;
using System.Collections.Generic;
using UniKh.core;
using UniKh.extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniKh.comp.ui {
    public class KhTabs : BetterBehavior {
        public event Action<int, int> OnSelectionChanged;        
        public List<GameObject> panels = new List<GameObject>();

        public int selectedTabIndex = -1;        
        public int defaultSelection = -1;
        
        
        private List<KhTabsItem> btns = new List<KhTabsItem>();
        private int prevSelection = -1;

        protected override void OnInit() {
            base.OnInit();
            for (var i = 0; i < transform.childCount; i++) {
                var tNode = transform.GetChild(i);
                var tab = tNode.gameObject.GetComponent<KhTabsItem>();
                var tabInd = i;
                tab.onClick.AddListener(() => Select(tabInd));
                btns.Push(tab);
            } 
            
            btns.ForEach(btn => btn.UsingActiveState(false));
            panels.ForEach(p => p.gameObject.SetActive(false)); 
            
            Select(defaultSelection); 
        } 
 
        public void Select(int index) {
            if (index < 0) return;
            if (selectedTabIndex == index) {
                SetIndexGroupActive(selectedTabIndex, true);
                return;
            }

            selectedTabIndex = index;
            if (prevSelection >= 0) {
                SetIndexGroupActive(prevSelection, false);
            }
            
            SetIndexGroupActive(selectedTabIndex, true);
            var prev = prevSelection;
            prevSelection = selectedTabIndex; 
            
            OnSelectionChanged?.Invoke(prev, selectedTabIndex); 
        }

        public void SetIndexGroupActive(int index, bool active) {
            var btn = btns[index];
            if (btn) {
                btn.UsingActiveState(active);

                if (panels.Count <= index) return;

                var p = panels[index];
                p.SetActive(active);
            }
        }
    }
}