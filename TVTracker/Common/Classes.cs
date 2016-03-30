using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVTracker.Common
{
    public class CustomException : Exception
    {
        public string ClassName { get; set; }
        public string MethodName { get; set;  }
        public string Details { get; set; }

        public CustomException(string className, string methodName, string message)
        {
            ClassName = className;
            MethodName = methodName;
            Details = message;
        }
    }

    public class CustomEventArgs : EventArgs
    {
        private CustomException _customException;
        public CustomException CustomException
        {
            get { return _customException; }
            set { _customException = value; }
        }

        public CustomEventArgs(CustomException customException)
        {
            this._customException = customException;
        }
    }
}
