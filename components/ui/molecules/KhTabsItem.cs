using UnityEngine;

namespace UniKh.comp.ui
{
    public class KhTabsItem : KhBtn
    {
        public GameObject pActive;
        public GameObject pUnActive;

        public void SetActive(bool active)
        {
            pActive.SetActive(active);
            pUnActive.SetActive(!active);
        }
    }
}