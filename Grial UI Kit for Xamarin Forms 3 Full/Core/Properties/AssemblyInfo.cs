using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using AppName.Core;
using Xamarin.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: Xamarin.Forms.Dependency(typeof(MirrorService))]
[assembly: AssemblyDescription("Core support library.")]
[assembly: AssemblyCopyright("Binaria - 2018")]
[assembly: InternalsVisibleTo("AppName.Core.Droid")]
[assembly: InternalsVisibleTo("AppName.Core.iOS")]
[assembly: InternalsVisibleTo("AppName.Core.UWP")]
[assembly: XmlnsDefinition("http://binariatechnologies.com/grial", "AppName.Core")]
