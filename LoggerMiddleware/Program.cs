using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Threading.Tasks;

namespace LoggerMiddleware
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:5000"))
            {
                Console.WriteLine("Server running at http://localhost:5000/");
                Console.ReadLine();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.Use<ResponseLoggingMiddleware>();
            app.Run(context =>
            {
                return context.Response.WriteAsync("Hello, OWIN Self-Host!");
            });
        }
    }

    public class ResponseLoggingMiddleware : OwinMiddleware
    {
        public ResponseLoggingMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            Console.WriteLine($"Response detected: {context.Response.StatusCode} for {context.Request.Uri}");
        }
    }

}
