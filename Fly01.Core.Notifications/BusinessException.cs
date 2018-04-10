using System;

namespace Fly01.Core.Notifications
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}