using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using AppName.Core;
using Xamarin.Forms;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyTitle("AppName.Core.iOS")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("${AuthorCopyright}")]
[assembly: AssemblyTrademark("")]
[assembly: ResolutionGroupName("AppName.Core")]
[assembly: Xamarin.Forms.Dependency(typeof(CultureServiceLocator))]
[assembly: Xamarin.Forms.Dependency(typeof(LayoutDirectionServiceLocator))]
[assembly: ExportEffect(typeof(BackgroundGradientEffect), "BackgroundGradientEffect")]
[assembly: ExportRenderer(typeof(GrialNavigationBar), typeof(GrialNavigationBarRenderer))]
[assembly: ExportEffect(typeof(IgnoreIOSSafeAreaOnScrollViewEffect), "IgnoreIOSSafeAreaOnScrollViewEffect")]
[assembly: ExportEffect(typeof(ApplyIOSSafeAreaAsPaddingEffect), "ApplyIOSSafeAreaAsPaddingEffect")]
[assembly: ExportEffect(typeof(CornerRadiusEffect), "CornerRadiusEffect")]
[assembly: ExportEffect(typeof(ShadowEffect), "ShadowEffect")]
[assembly: ExportRenderer(typeof(FormsVideoPlayer), typeof(FormsVideoPlayerRenderer))]
[assembly: ExportRenderer(typeof(DataGrid.StripedListView), typeof(StripedListViewRenderer))]
[assembly: AssemblyVersion("1.0.7228.24414")]
