using System;
using System.Collections;
using System.Collections.Generic;
using UniKh.utils;

namespace UniKh.dataStructure {
    public class Heap<T>: IEnumerable, IEnumerable<T> {
     
    private readonly Func<T, T, bool> _fnLess;
    private readonly List<T> _items; // list based on 1
     
    public Heap(int capacity = 0) {
        _items = 0 != capacity ? new List<T>(capacity + 1) : new List<T>();
        _items.Add(default(T));
    }

    public Heap(Func<T, T, bool> fnLess, int capacity = 0) : this(capacity) {
        _fnLess = fnLess;
    }
    
    public Heap(Comparison<T> fnLess, int capacity = 0) : this(capacity) {
        _fnLess = (a, b) => fnLess(a, b) < 0;
    }
    
   
    private bool Less(int indA, int indB) {
        if (_fnLess != null) {
            return _fnLess(_items[indA], _items[indB]);
        }
        return Comparer<T>.Default.Compare(_items[indA], _items[indB]) < 0;
    }
    
    private bool Less(T a, T b) {
        if (_fnLess != null) {
            return _fnLess(a, b);
        }
        return Comparer<T>.Default.Compare(a, b) < 0;
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

    private void SiftDown(int ind = 1) { 
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
     

    
    // private void SiftDownSwap() {
    //     var ind = 1;
    //     int left;
    //
    //     while ((left = ind << 1) <= Count) { // 退出条件: left 已经在范围外
    //         var right = left + 1;
    //         var target = right < Count && Less(right, left) ? right : left;
    //         if (Less(ind, target)){
    //             break;
    //         }
    //
    //         Swap(ind, target);
    //         ind = target;
    //     }
    // }
    //
    private void SiftDownBottomUp() {
        var iTarget = 1;
        int left;
        while ((left = iTarget << 1) < Count) {
            var right = left + 1;
            iTarget = Less(left, right) ? left : right;
        }

        if (iTarget << 1 == Count) iTarget = Count; // special leaf

        var curVal = _items[1];
        for (; iTarget > 1; iTarget >>= 1) {
            if (Less(curVal, _items[iTarget])) continue;
            var temp = _items[iTarget];
            _items[iTarget] = curVal;
            curVal = temp;
        }
        _items[1] = curVal; 
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
    /// 完全重排堆
    /// </summary>
    public void Heapify( int count){
        var start = (count - 1) / 2; // get parent of element count - 1
        while(start > 0){
            SiftDown(start--);
        }
    } 
    
    public T Peek() {
        return _items[1];
    }
    
    /// <summary>
    /// Insert into the heap
    /// </summary> 
    public Heap<T> Insert(T val) {
        Count += 1;
        if (Count == _items.Count) {
            _items.Add(val);
        }
        else {
            _items[Count] = val; 
        }

        SiftUp();
        return this;
    }

    /// <summary>
    /// Replace
    /// </summary>
    public Heap<T> Replace(T val) { 
        _items[1]= val;
        SiftDown();
        return this;
    }

    public T Extract() {
        var ret = _items[1];
        var lastItem = _items[Count--];
        _items[1] = lastItem;
        // SiftDownSwap();
        SiftDownBottomUp();
        // SiftDown();
        _items[Count + 1] = default(T);
        return ret;
    }


    public override string ToString() {
        var left = 1;
        var builder = SGen.New;
        while (left < Count) {
            for (var i = left; i < left << 1; i++) {
                if (i > Count) break;
                // builder.Append(ind).Append('.').
                builder.Append(_items[i]).Append(' ');
            }

            left <<= 1;
            builder.AppendLine();
        }

        return builder.End;
    }

    public IEnumerator GetEnumerator() {
        var it = _items.GetEnumerator();
        it.MoveNext();
        return it;
    }
    
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
        var it = _items.GetEnumerator();
        it.MoveNext();
        return it;
    }
} 
}