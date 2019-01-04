using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniKh.dataStructure {
    [Serializable]
    public class BoundLBRT {
        public BoundLBRT(float left_, float bottom_, float width_, float height_) {
            left = left_;
            bottom = bottom_;
            width = width_;
            height = height_;
        }

        public BoundLBRT(Vector2 pos, Vector2 size) :
            this(pos.x, pos.y, size.x, size.y) {
        }

        public float left;
        public float bottom;
        public float width;
        public float height;

        public float right => left + width;
        public float top => bottom + height;

        public Vector2 leftBottom => new Vector2(left, bottom);

        public Vector2 rightBottom => new Vector2(right, bottom);

        public Vector2 leftTop => new Vector2(left, top);

        public Vector2 rightTop => new Vector2(right, top);

        public Vector2 size => new Vector2(width, height);

        public Vector2 center => new Vector2(left + width / 2, bottom + height / 2);

        public BoundLBRT ExpandX(float size) {
            left -= size;
            bottom -= size;
            width += size * 2;
            height += size * 2;
            return this;
        }

        public BoundLBRT Clone() {
            return new BoundLBRT(
                left,
                bottom,
                width,
                height);
        }

        public BoundLBRT Expand(float size) {
            return Clone().ExpandX(size);
        }

        public BoundLBRT Expand2AccommodateNodes(Vector2 pos) {
            return Expand2AccommodateCoordinateX(pos.x).Expand2AccommodateCoordinateY(pos.y);
        }

        public BoundLBRT Expand2AccommodateCoordinateX(float x) {
            if (x < left) {
                width += left - x;
                left = x;
            } else if (x > right) {
                width = x - left;
            }

            return this;
        }

        public BoundLBRT Expand2AccommodateCoordinateY(float y) {
            if (y < bottom) {
                height += bottom - y;
                bottom = y;
            } else if (y > top) {
                height = y - bottom;
            }

            return this;
        }

        public bool XIn(float x_) { return x_ >= left && x_ <= right; }

        public bool Yin(float y_) { return y_ >= bottom && y_ <= top; }

        public bool In(Vector2 pos) { return XIn(pos.x) && Yin(pos.y); }

        public bool Intersect(BoundLBRT target) {
            return !(target.left > right
                     || target.right < left
                     || target.bottom > top
                     || target.top < bottom
                );
        }

        public Vector2 RandomPos() { return new Vector2(Random.Range(left, right), Random.Range(bottom, top)); }

        public Vector2Int PosToCoord(Vector2 pos, int split) {
            var offset = pos - leftBottom;
            return new Vector2Int(
                Mathf.FloorToInt(offset.x * split / size.x),
                Mathf.FloorToInt(offset.y * split / size.y));
        }

        public override string ToString() { return $"[{leftBottom}->{rightTop}]"; }
    }
}