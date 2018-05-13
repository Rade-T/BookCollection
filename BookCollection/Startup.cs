using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookCollection.Startup))]
namespace BookCollection
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
