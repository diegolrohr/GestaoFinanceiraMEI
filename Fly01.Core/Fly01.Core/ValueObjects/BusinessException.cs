using System;

namespace Fly01.Core.ValueObjects
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
