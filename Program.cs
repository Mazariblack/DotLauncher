using Photino.NET;
using System.Drawing;
using System.Text;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using System.Runtime.CompilerServices;

namespace HelloPhotinoApp
{
    //NOTE: To hide the console window, go to the project properties and change the Output Type to Windows Application. THIS IS JUST EXAMPLE OF THE CODE!!!! MOST OF IT TAKEN FROM THE DOCS
    // Or edit the .csproj file and change the <OutputType> tag from "WinExe" to "Exe".
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Window title declared here for visibility
            string windowTitle = "Dot Launcher";

            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(1024, 800))
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                .SetResizable(false)
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler((object sender, string message) =>
                {
                    var window = (PhotinoWindow)sender;

                    // The message argument is coming in from sendMessage.
                    // "window.external.sendMessage(message: string)"
                    if (message.StartsWith("launch")) { startMinecraft(); }

                })
                .Load("wwwroot/index.html"); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.WaitForClose(); // Starts the application event loop
        }

        static async void startMinecraft()
        {
            var path = new MinecraftPath(); //Finds the minecraft path
            var launcher = new MinecraftLauncher(path);
            launcher.FileProgressChanged += (sender, args) =>
            {
                Console.WriteLine($"Name: {args.Name}");  //Logging the process
                Console.WriteLine($"Type: {args.EventType}");
                Console.WriteLine($"Total: {args.TotalTasks}");
                Console.WriteLine($"Progressed: {args.ProgressedTasks}");
            };
            launcher.ByteProgressChanged += (sender, args) =>
            {
                Console.WriteLine($"{args.ProgressedBytes} bytes / {args.TotalBytes} bytes");
            };

            await launcher.InstallAsync("1.20.4"); //Install version
            var process = await launcher.BuildProcessAsync("1.20.4", new MLaunchOption
            {
                Session = MSession.CreateOfflineSession("Gamer123"), //(<= Nickname) Makes startup settings
                MaximumRamMb = 4096, //(<= Maximum Ram for minecraft, 4GB)
                MinimumRamMb = 1026  //(<= Minimum Ram for minecraft, 1GB)
            });
            process.Start(); //Starts the minecraft
        }
    }
}
