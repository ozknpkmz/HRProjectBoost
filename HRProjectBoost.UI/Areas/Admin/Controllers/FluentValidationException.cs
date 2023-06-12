using System.Runtime.Serialization;

namespace HRProjectBoost.UI.Areas.Admin.Controllers
{
    [Serializable]
    internal class FluentValidationException : Exception
    {
        private object errors;

        public FluentValidationException()
        {
        }

        public FluentValidationException(object errors)
        {
            this.errors = errors;
        }

        public FluentValidationException(string? message) : base(message)
        {
        }

        public FluentValidationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FluentValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}