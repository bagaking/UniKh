using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UniKh.core {

    public class IdsTree {
        public IdsTree children;
        public IdsTree lSibling;
        public IdsTree rSibling;
        public uint fromPos;
        public uint toPos;
        public Type value;

        public IdsTree(uint from, uint to, Type v) {
            fromPos = from;
            toPos = to;
            value = v;
        }
        
        public Type Find(uint i) {
            if (i < fromPos) {
                if (lSibling == null || i >= lSibling.toPos) return null;
                return lSibling.Find(i);
            }
            if (i >= toPos) {
                if (rSibling == null || i < rSibling.fromPos) return null;
                return rSibling.Find(i);
            }
            var v = children?.Find(i);
            return v ?? value;
        }

        public IdsTree Insert(IdsTree other) {
            if (other.fromPos >= toPos) { // Right Sibling
                rSibling = rSibling != null ? rSibling.Insert(other) : other;
                return this;
            }

            if (other.toPos < fromPos) { // Left Sibling
                lSibling = lSibling != null ? lSibling.Insert(other) : other;
                return lSibling;
            }

            if (other.fromPos >= fromPos && other.toPos <= toPos) {
                children = children != null ? children.Insert(other) : other;
                return this;
            }

            if (other.fromPos < fromPos && other.toPos > toPos) {
                other.children = other.children != null ? other.children.Insert(this) : this;
                return other;
            }

            throw new Exception("Id Segments do not intersect.");
        }

        public override string ToString() => $"[{fromPos},{toPos}):{value.Name}";
        
        public void ForEach(Action<IdsTree, int> func, int depth = 0) {
            var c = this;
            while (c.lSibling != null) c = c.lSibling;
            do {
                func(c, depth);
                c.children?.ForEach(func, depth + 1);
                c = c.rSibling;
            } while (c != null);
        }
        
        public string ToDir() {
            var sb = new StringBuilder();
            ForEach(
                (c, depth) => {
                    sb.Append('-', depth).Append(c).AppendLine();
                });
            return sb.ToString();
        }
    }
}