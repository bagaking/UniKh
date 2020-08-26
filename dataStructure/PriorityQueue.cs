using System.Collections;
using System.Collections.Generic;
using UniKh.utils;

namespace UniKh.dataStructure {
    public class PriorityQueue<T>: IEnumerable<PriorityQueue<T>.Node> {
    public class Node {
        internal int Priority;
        internal T Val;

        public override string ToString() {
            return SGen.New[Val].End;
        }
    }

    public delegate bool PredicatePriorityLess(int priorityA, int priorityB);

    public PriorityQueue(int capacity = 0) {
        _items = 0 != capacity ? new List<Node>(capacity + 1) : new List<Node>();
        _items.Add(null);
    }

    public PriorityQueue(PredicatePriorityLess fnLess, int capacity = 0) : this(capacity) {
        _fnLess = fnLess;
    }

    private readonly List<Node> _items; // list based on 1

    private readonly PredicatePriorityLess _fnLess;

    private bool Less(int indA, int indB) {
        return _fnLess?.Invoke(_items[indA].Priority, _items[indB].Priority) ??
               _items[indA].Priority < _items[indB].Priority;
    }

    private bool Less(Node nodeA, Node nodeB) {
        return _fnLess?.Invoke(nodeA.Priority, nodeB.Priority) ??
               nodeA.Priority < nodeB.Priority;
    }


    public T Peek() {
        return _items[1].Val;
    }
    
    public int PeekPriority() {
        return _items[1].Priority;
    }
 
    private void Swap(int indA, int indB) {
        var temp = _items[indA];
        _items[indA] = _items[indB];
        _items[indB] = temp;
    }

    private void SiftUp() {
        var ind = Count;
        while (ind > 1) {
            var parent = ind >> 1;
            if (Less(parent, ind)) {
                return;
            }

            Swap(parent, ind);
            ind = parent;
        }
    }

    private void SiftDown() {
        var ind = 1;
        int left;

        var temp = _items[ind];
        while ((left = ind << 1) <= Count) { // 退出条件: left 已经在范围外
            var right = left + 1;
            var target = right < Count && Less(right, left) ? right : left;
            if (Less(temp, _items[target])) {
                break;
            }

            _items[ind] = _items[target];
            ind = target;
        }

        _items[ind] = temp;
    }

    /// <summary>
    /// 获取堆内项的数量
    /// </summary>
    public int Count { get; private set; } = 0;

    public void Clear() {
        _items.RemoveRange(1, Count);
        Count = 0;
    }
    
    /// <summary>
    /// Insert into the heap
    /// </summary> 
    public PriorityQueue<T> Insert(T val, int priority) {
        Count += 1;
        if (Count == _items.Count) {
            _items.Add(new Node {
                Val = val, Priority = priority
            });
        }
        else {
            _items[Count].Val = val;
            _items[Count].Priority = priority;
        }

        SiftUp();
        return this;
    }

    /// <summary>
    /// Replace
    /// </summary>
    public PriorityQueue<T> Replace(T val, int priority) {
        _items[1].Priority = priority;
        _items[1].Val = val;
        SiftDown();
        return this;
    }

    public T Extract() {
        var ret = _items[1].Val;
        var lastItem = _items[Count--];
        _items[1].Priority = lastItem.Priority;
        _items[1].Val = lastItem.Val;
        lastItem.Val = default(T); // 清理无效值防止内存泄露
        SiftDown();
        // SiftDownToLeaf();
        return ret;
    }

    public override string ToString() {
        var left = 1;
        var builder = SGen.New;
        while (left < Count) { // 退出条件: left 已经在范围外
            // builder = builder.Append(' ', left * 4);
            for (var i = left; i < left << 1; i++) {
                if (i > Count) break;
                // builder.Append(ind).Append('.').
                builder.Append(_items[i].Val).Append(' ');
            }

            left <<= 1;
            builder.AppendLine();
        }

        return builder.End;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public IEnumerator<Node> GetEnumerator() {
        var it = _items.GetEnumerator();
        it.MoveNext();
        return it;
    }
}
}