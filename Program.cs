using System.Reflection;
using TMXTile;
using xTile;
using xTile.Format;

namespace ToasterMapCLI
{
    class ToasterMapCLI
    {
        public static void Main(string[] args)
        {
            // For some reason, there's an xTile version mismatch between what TMXTile references and what the game provides.
            // When .NET fails to load a dependency, we try to load it by ignoring the assembly version.
            //
            // Note that the code which references xTile must be in a separate method, to avoid referencing it before we hook
            // into the assembly resolution.
            AppDomain.CurrentDomain.AssemblyResolve += ToasterMapCLI.CurrentDomain_AssemblyResolve;
            Run(args);
        }

        public static void Run(string[] args)
        {
            string tbinpath = args[0];
            string? tmxpath = null;
            if (args.Length > 1)
                tmxpath = args[1];
            else
                tmxpath = tbinpath.Replace(".tbin", ".tmx");
            //string tbinpath = @"C:\Coding\C#\ToasterMapCLI\Python\Farm.tbin";
            //string tmxpath = @"C:\Coding\C#\ToasterMapCLI\Python\Farm.tmx";

            Console.WriteLine("tbinpath: " + tbinpath);
            Console.WriteLine("tmxpath: " + tmxpath);

            FormatManager formatManager = FormatManager.Instance;
            Map map = formatManager.LoadMap(tbinpath);
            map.assetPath = tmxpath;

            TMXFormat format = new(16, 16, 4, 4);
            TMXMap tmx = format.Store(map);

            var parser = new TMXParser();
            parser.Export(tmx, tmxpath);

            Console.WriteLine($"Successfully wrote {tmxpath}");
        }

        /// <summary>Method called when assembly resolution fails, which may return a manually resolved assembly.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private static Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            // Console.WriteLine($"Resolving assembly: {e.Name}");
            // get assembly name
            AssemblyName name = new AssemblyName(e.Name);
            if (name.Name is null || !name.Name.Equals("xTile", StringComparison.OrdinalIgnoreCase))
                return null;

            // get path containing the executable
            string? searchPath = Path.GetDirectoryName(Environment.ProcessPath);
            if (searchPath is null)
                return null;

            // try to resolve DLL
            try
            {
                foreach (FileInfo dll in new DirectoryInfo(searchPath).EnumerateFiles("*.dll"))
                {
                    // get assembly name
                    string? dllAssemblyName;
                    try
                    {
                        dllAssemblyName = AssemblyName.GetAssemblyName(dll.FullName).Name;
                        if (dllAssemblyName is null)
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    // check for match
                    if (name.Name.Equals(dllAssemblyName, StringComparison.OrdinalIgnoreCase))
                        return Assembly.LoadFrom(dll.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving assembly: {ex}");
                return null;
            }

            return null;
        }
    }
}