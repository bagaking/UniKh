using UniKh.core;

namespace UniKh.comp.ui {

    public  class KhPanel : BetterBehavior { }
    

    public abstract class KhPanel<TMotion> : KhPanel where TMotion : MotionObject {

        public TMotion motion;
        
        public abstract TMotion CreateDefaultMotionComponents();
    }
}