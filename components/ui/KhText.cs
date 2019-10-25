using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniKh.comp.ui
{
    public class KhText : Text
    {
        public string[] unitLst = {"", "K", "M", "B", "T", "P", "a", "b", "c", "e", "f", "g"};

        [SerializeField]
        private float m_numberValue = 0;

        [SerializeField]
        private float m_rotateTarget = float.MinValue;

        public bool showSign = false;
        public bool usingTextValue = true;


        public void SetValue(float v) {
            if (usingTextValue) {
                m_Text = v.ToString();
            }
            else {
                m_numberValue = v;
            } 
            SetVerticesDirty();
        }
    
        public void RotateValue(float to) {
            m_rotateTarget = to;
        }
    
        public void RotateValue(float from, float to) {
            SetValue(from);
            m_rotateTarget = to;
        }
    
        private float NumberValue {
            get {
                return usingTextValue ? (string.IsNullOrWhiteSpace(m_Text) ? 0 : Convert.ToSingle(m_Text.Trim())) : m_numberValue;
            }
        }
    
        private string GetValueString() {
            var valueInUse = NumberValue;
            var sign = valueInUse < 0 ? "-" : (showSign ? "+" : "");
            var temp = Mathf.Abs(valueInUse);
            var i = 0;
            while (temp > 1000 && i < unitLst.Length) {
                temp /= 1000;
                i++;
            }
    
            return sign + temp.ToString((temp == (int) temp || i == 0)
                           ? "F0"
                           : (temp > 100
                               ? "F1"
                               : "F2")
                       //(temp > 10
                       //    ? "F2"
                       //    : "F3"))
                   ) + unitLst[i < unitLst.Length ? i : unitLst.Length - 1];
        }
    
        protected override void OnPopulateMesh(VertexHelper toFill) {
            var textOld = m_Text;
            m_Text = usingTextValue ? GetValueString() : textOld + GetValueString();
            base.OnPopulateMesh(toFill);
            m_Text = textOld;
        }
    
        public void Update() {
            if (!(Math.Abs(m_rotateTarget - float.MinValue) > float.Epsilon)) return;
    
            var valueInUse = NumberValue;
            valueInUse = Mathf.Lerp(valueInUse, m_rotateTarget, m_rotateTarget - valueInUse > 1000 ? 0.2f : 0.1f);
            if (Mathf.Abs(m_rotateTarget - valueInUse) <= float.Epsilon) {
                valueInUse = m_rotateTarget;
                m_rotateTarget = float.MinValue;
            }
    
            SetValue(valueInUse);
        }
    }
}