using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using WinUITestProject.Infrastructure.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUITestProject
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window? _window{ get; private set; }

        public static IServiceProvider Services { get; private set; } = null!;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var sc = new ServiceCollection();

            SQLitePCL.Batteries.Init();

            sc.AddDbContext<AppDbContext>(ServiceLifetime.Transient);

            Services = sc.BuildServiceProvider();

            using (var db = Services.GetRequiredService<AppDbContext>())
            {
                db.Database.Migrate();
            }

           


                _window = new MainWindow();
            _window.Activate();
        }

        public static AppDbContext CreateDbContext() => Services.GetRequiredService<AppDbContext>();
    }
}
