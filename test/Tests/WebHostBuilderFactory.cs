using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Reflection;

namespace Tests
{
    public static class WebHostBuilderFactory
    {
        private const string _sourceDirectoryName = "src";
        private const string _solutionName = "NetCoreMediatr.sln";

        public static IWebHostBuilder Create<TStartUp>()
        {
            return GetBuilder<TStartUp>();
        }

        public static IWebHostBuilder Create<TStartUp>(Action<IServiceCollection> configureServices)
        {
            var builder = GetBuilder<TStartUp>()
                .ConfigureServices(configureServices);

            return builder;
        }

        private static IWebHostBuilder GetBuilder<TStartUp>()
        {
            var startupAssembly = typeof(TStartUp).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(_sourceDirectoryName, startupAssembly);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartUp));

            return builder;
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, _solutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}
