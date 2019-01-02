namespace UniKh.mvc {
    public abstract class ViewModel<TSelf> : ViewModel where TSelf : ViewModel<TSelf>, new() {
        public TSelf PreRender() {
            MarkRendered();
            return this as TSelf;
        }

        public abstract TSelf Clone();
    }


    public class ViewModel {
        public bool dirty { get; private set; }
        public void SetDirty() { dirty = true; }

        public void MarkRendered() { dirty = false; }
    }
}