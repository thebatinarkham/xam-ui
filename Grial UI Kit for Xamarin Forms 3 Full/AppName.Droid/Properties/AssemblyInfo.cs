using System.Reflection;
using Xamarin.Forms;
using AppName;

[assembly: AssemblyTitle(AssemblyGlobal.ProductLine + " - " + "Xamarin.Forms (Android)")]
[assembly: AssemblyConfiguration(AssemblyGlobal.Configuration)]
[assembly: AssemblyCompany(AssemblyGlobal.Company)]
[assembly: AssemblyProduct(AssemblyGlobal.ProductLine + " - " + "Xamarin.Forms (Android)")]
[assembly: AssemblyCopyright(AssemblyGlobal.Copyright)]

[assembly: AppName.Core.GrialVersion("3.0.54.0")]
[assembly: ExportRenderer(typeof(Entry), typeof(AppName.Core.EntryRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(AppName.Core.EditorRenderer))]
[assembly: ExportRenderer(typeof(Switch), typeof(AppName.Core.SwitchRenderer))]
[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(AppName.Core.ActivityIndicatorRenderer))]
[assembly: ExportRenderer(typeof(SwitchCell), typeof(AppName.Core.SwitchCellRenderer))]
[assembly: ExportRenderer(typeof(TextCell), typeof(AppName.Core.TextCellRenderer))]
[assembly: ExportRenderer(typeof(ImageCell), typeof(AppName.Core.ImageCellRenderer))]
[assembly: ExportRenderer(typeof(ViewCell), typeof(AppName.Core.ViewCellRenderer))]
[assembly: ExportRenderer(typeof(EntryCell), typeof(AppName.Core.EntryCellRenderer))]
[assembly: ExportRenderer(typeof(SearchBar), typeof(AppName.Core.SearchBarRenderer))]
[assembly: ExportRenderer(typeof(DatePicker), typeof(AppName.Core.DatePickerRenderer))]
[assembly: ExportRenderer(typeof(TimePicker), typeof(AppName.Core.TimePickerRenderer))]
[assembly: ExportRenderer(typeof(ExtendedCarouselViewControl), typeof(AppName.Droid.ExtendedCarouselViewRenderer))]

[assembly: ExportRenderer(typeof(NavigationPage), typeof(AppName.Core.GrialNavigationPageRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(AppName.Core.PickerRenderer))]

// Fix for ScrollView layout after device orientation change
[assembly: ExportRenderer(typeof(ScrollView), typeof(AppName.Droid.ScrollViewRendererOrientationFix))]

// XF 3.1+ Includes properties to control the colors of sliders that work on Android API level >= 21.
// Uncomment below renderer if you are targeting Android API <= 20 to get the Grial theme accent color applied to the Slider.
// Note that this renderer owerrides the beahvior of the Slider color properties of XF 3.1+. If you need to set a specific color
// you can use grial:SliderProperties.TintColor attached property.
//[assembly: ExportRenderer(typeof(Slider), typeof(AppName.Core.SliderRenderer))]