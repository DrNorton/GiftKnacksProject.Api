using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace GiftKnacksProject.Api
{
    public class MyStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
            app.UseWelcomePage("/Welcome"); // See Microsoft.Owin.Diagnostics 
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello world using OWIN TestServer");
            });
        }
    }
}