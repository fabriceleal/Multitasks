
namespace MultiTasks.RT
{
    public class MtObject 
    {
        private object _value;

        public MtObject(object value) 
        { 
            _value = value; 
        }
        
        public object Value
        {
            get
            {
                return _value;
            }
        }

        public override string ToString()
        {
            if (_value == null)
                return "<MtObject with no value!>";

            return _value.ToString();
        }

        #region Static

        private static MtObject _true = new MtObject(true);
       
        private static MtObject _false = new MtObject(false);

        public static MtObject True
        {
            get
            {
                return _true;
            }
        }

        public static MtObject False
        {
            get
            {
                return _false;
            }
        }
        
        #endregion

    }
}
