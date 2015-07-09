using System;

namespace InversionOfControlContainer
{
    public class TypeNotRegisteredException : Exception
    {
        private string _Message;

        public TypeNotRegisteredException(Type t)
        {
            _Message = string.Format("'{0}' has not been registered.", t.Name);
        }

        public override string Message
        {
            get
            {
                return _Message;
            }
        }
    }
}