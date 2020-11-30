# Codesanook.ReactJS

ReactJS server side rendering module for Orchard CMS

# How to integrate React server side rendering

-   Install React.Core by command `install-package React.Core -version 5.0.0`
-   Include built bundle script in ShellEvent. For example:

```
    namespace Codesanook.Module {
        public class ShellEvent : IOrchardShellEvents {
            public void Activated() {
                ReactSiteConfiguration.Configuration
                    // Disable load Babel because we already transformed TypeScript with Webpack
                    .SetLoadBabel(false)
                    .AddScriptWithoutTransform(
                        "~/Modules/Codesanook.Module/Scripts/codesanook-module.js"
                    );
            }

            public void Terminating() {
            }
        }
    }
```

-   Add `<add namespace="Codesanook.ReactJS" />` to `<system.web.webPages.razor>` in Web.config root level.
-   Add `Script.Require("React").AtHead()` and `Script.Require("ReactDOM").AtHead()`.
-   Include a bundle script in a module's Razor page with `Script.Include("bundle.js")`.
-   Add `HtmlHelper.React("Module.Component", {Props})`.
-   Add `@Html.ReactInitJavaScript()` at bottom of Document.cshtml (theme) of an active theme to initialize React script.

## Throuble shooting

### All required libraries that need to go inside main website bin folder

-   ClearScriptV8-32.dll
-   v8-base-ia32.dll
-   v8-ia32.dll
-   v8-libcpp-ia32.dll

### You Azure app service need to run as 32 bit version
