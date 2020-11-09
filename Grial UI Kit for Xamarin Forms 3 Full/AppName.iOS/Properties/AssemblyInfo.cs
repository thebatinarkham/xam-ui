using System.Reflection;
using Xamarin.Forms;
using AppName;
using AppName.Core;

[assembly: AssemblyTitle(AssemblyGlobal.ProductLine + " - " + "Xamarin.Forms (iOS)")]
[assembly: AssemblyConfiguration(AssemblyGlobal.Configuration)]
[assembly: AssemblyCompany(AssemblyGlobal.Company)]
[assembly: AssemblyProduct(AssemblyGlobal.ProductLine + " - " + "Xamarin.Forms (iOS)")]
[assembly: AssemblyCopyright(AssemblyGlobal.Copyright)]

[assembly: AppName.Core.GrialVersion("3.0.54.0")]
[assembly: ExportRenderer(typeof(EntryCell), typeof(AppName.Core.EntryCellRenderer))]
[assembly: ExportRenderer(typeof(ImageCell), typeof(AppName.Core.ImageCellRenderer))]
[assembly: ExportRenderer(typeof(SwitchCell), typeof(AppName.Core.SwitchCellRenderer))]
[assembly: ExportRenderer(typeof(TableView), typeof(AppName.Core.TableRenderer))]
[assembly: ExportRenderer(typeof(TextCell), typeof(AppName.Core.TextCellRenderer))]
[assembly: ExportRenderer(typeof(ViewCell), typeof(AppName.Core.ViewCellRenderer))]
[assembly: ExportRenderer(typeof(Entry), typeof(AppName.Core.EntryRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(AppName.Core.EditorRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(AppName.Core.PickerRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(AppName.Core.DatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(AppName.Core.TimePickerRenderer))]
[assembly: ExportRenderer(typeof(Page), typeof(AppName.Core.PageRenderer))]
[assembly: ExportRenderer(typeof(CarouselPage), typeof(AppName.iOS.CarouselPageVerticalScrollFixRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(AppName.iOS.SearchBarPlaceholderColorFixRenderer))]
[assembly: ExportRenderer(typeof(NavigationPage), typeof(AppName.Core.GrialNavigationPageRenderer))]

#pragma warning disable 219
internal static class WorkaroundLoadingCustomRenderersFromExternalAssemblies
{
    static WorkaroundLoadingCustomRenderersFromExternalAssemblies()
    {
        var a = new AppName.Core.EntryRenderer();
    }
}
#pragma warning restore 219
