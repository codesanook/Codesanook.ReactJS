using System;
using System.Globalization;
using System.Reflection;
using System.Web;
using Autofac;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using React;

namespace Codesanook.ReactJS {
    public class ComponentModule : Autofac.Module {
        protected override void Load(ContainerBuilder builder) {
            //public ReactEnvironment(
            //    IJavaScriptEngineFactory engineFactory,
            //    IReactSiteConfiguration config,
            //    ICache cache,
            //    IFileSystem fileSystem,
            //    IFileCacheHash fileCacheHash,
            //    IReactIdGenerator reactIdGenerator
            //);

            // public JavaScriptEngineFactory(
            //IJsEngineSwitcher jsEngineSwitcher,
            //     IReactSiteConfiguration config,
            //     ICache cache,
            //     IFileSystem fileSystem
            //);

            JsEngineSwitcher.Current.DefaultEngineName = V8JsEngine.EngineName;
            JsEngineSwitcher.Current.EngineFactories.AddV8();

            builder.RegisterType<JavaScriptEngineFactory>().As<IJavaScriptEngineFactory>().SingleInstance();
            builder.RegisterInstance(ReactSiteConfiguration.Configuration).As<IReactSiteConfiguration>().SingleInstance();
            builder.RegisterType<ReactIdGenerator>().As<IReactIdGenerator>().SingleInstance();
            builder.RegisterInstance(JsEngineSwitcher.Current).As<IJsEngineSwitcher>().SingleInstance();

            builder.Register(c => new AspNetCache(HttpRuntime.Cache)).As<ICache>().InstancePerDependency();
            builder.RegisterType<AspNetFileSystem>().As<IFileSystem>().InstancePerDependency();
            builder.RegisterType<FileCacheHash>().As<IFileCacheHash>().InstancePerDependency();

            builder.RegisterType<ReactEnvironment>().As<IReactEnvironment>().InstancePerLifetimeScope();
            RedirectAssembly("JavaScriptEngineSwitcher.Core", new Version("3.1.0.0"), "c608b2a8cc9e4472");
        }

        private static void RedirectAssembly(string shortName, Version targetVersion, string publicKeyToken)
        {
            ResolveEventHandler handler = null;
            handler = (sender, args) => {

                // Use latest strong name & version when trying to load SDK assemblies
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != shortName)
                    return null;

                requestedAssembly.Version = targetVersion;
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;
                //For security reasons public key token should match;
                requestedAssembly.SetPublicKeyToken(new AssemblyName("x, PublicKeyToken=" + publicKeyToken).GetPublicKeyToken());
                AppDomain.CurrentDomain.AssemblyResolve -= handler;

                return Assembly.Load(requestedAssembly);
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }
    }
}