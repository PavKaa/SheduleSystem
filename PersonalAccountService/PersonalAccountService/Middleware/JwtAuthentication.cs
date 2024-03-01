namespace PersonalAccountService.Middleware
{
	public class JwtAuthentication
	{
		private readonly RequestDelegate next;

        public JwtAuthentication(RequestDelegate next)
        {
            this.next = next; 
        }

        public async Task Invoke(HttpContext context) 
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

            if (authHeader != null) 
            {
                var token = authHeader.Split(" ").Last();
            }

            await next.Invoke(context);
        }
    }
}
