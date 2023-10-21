namespace auth {
    public abstract class AuthException : Exception
    {
        protected AuthException(string message) : base(message) { }
        protected AuthException() { }
    }

    public class AuthForbiddenException : AuthException
    {
        public AuthForbiddenException() { }
        public AuthForbiddenException(string message): base(message) { }
    }
}
