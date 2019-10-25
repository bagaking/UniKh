using System;
using System.Collections.Generic;
using UniKh.extensions;
using UnityEngine;

namespace UniKh.comp.ui
{
    public class KhTabs : MonoBehaviour
    {
        public List<KhTabsItem> btns = new List<KhTabsItem>();
        public List<GameObject> panels = new List<GameObject>();
        public Action<int> OnSelectTab;
        public int lastSelect = -1;
        public int select = -1;
        public int defaultSelect = 0;
        // Start is called before the first frame update
        void Start()
        {

            var btnsTransforms = new List<Transform>();
            for (var i = 0; i < transform.childCount; i++) {
                var tNode = transform.GetChild(i);
                var tab = tNode.gameObject.GetComponent<KhTabsItem>();
                if (tab)
                {
                    btnsTransforms.Add(tNode);
                }
            }
            for (var i = 0; i < btnsTransforms.Count; i++)
            {
                btns.Push(btnsTransforms[i].gameObject.GetComponent<KhTabsItem>());
                var btn = btns[i];
                var index = i;
                btn.onClick.AddListener(() => OnTabClick(index));
            }
            OnTabClick(defaultSelect);
        }

        // Update is called once per frame
        void Update() {

        }

        public void OnTabClick(int index) {

            if (index < 0) return;
            if (select == index) {
//                SetActive(select, false);
//                lastSelect = -1;
//                select = -1;
                return;
            }
            lastSelect = select;
            select = index;

            if (lastSelect != -1) {
                SetActive(lastSelect, false);
            }
            SetActive(select, true);
            Debug.Log(index);
            OnSelectTab?.Invoke(index);

        }
        public void SetActive(int index, bool active) {
            var btn = btns[index];
//            var p = panels[index];

            if (btn)
            {
                btn.SetActive(active);
            }
        }
    }
}