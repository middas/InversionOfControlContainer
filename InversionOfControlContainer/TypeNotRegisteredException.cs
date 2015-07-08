using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControlContainer
{
    public class TypeNotRegisteredException : Exception
    {
        private string _Message;
        public override string Message
        {
            get
            {
                return _Message;
            }
        }

        public TypeNotRegisteredException(Type t)
        {
            _Message = string.Format("'{0}' has not been registered.", t.Name);
        }
    }
}
