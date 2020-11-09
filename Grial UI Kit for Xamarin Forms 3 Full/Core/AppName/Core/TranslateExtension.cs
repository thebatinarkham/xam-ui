using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    [ContentProperty("Key")]
    public class TranslateExtension : IMarkupExtension<Binding>, IMarkupExtension
    {
        private class BindingSource : INotifyPropertyChanged, NotificationObjectsTracker.INotifier
        {
            private ResourceManager _manager;

            private string _key;

            public string Text
            {
                get
                {
                    CultureInfo currentCulture = _CultureService.CurrentCulture;
                    string text = _manager.GetString(_key, currentCulture);
                    if (text == null)
                    {
                        text = _key;
                    }
                    return text;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public BindingSource(ResourceManager manager, string key)
            {
                _manager = manager;
                _key = key;
            }

            public void Notify()
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        private const string DefaultResourcesKey = "DefaultStringResources";

        private static readonly Dictionary<string, ResourceManager> _ResourceManagers = new Dictionary<string, ResourceManager>();

        private static readonly NotificationObjectsTracker _notificationTracker = new NotificationObjectsTracker();

        private static ResourceManager _defaultResourceManager;

        private static ICultureService _CultureService
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string Source
        {
            get;
            set;
        }

        public IValueConverter Converter
        {
            get;
            set;
        }

        public object ConverterParameter
        {
            get;
            set;
        }

        public string StringFormat
        {
            get;
            set;
        }

        public TranslateExtensionMode Mode
        {
            get;
            set;
        }

        public TranslateExtension()
        {
            Mode = TranslateExtensionMode.UpdateWhenCurrentCultureChanges;
            if (_CultureService != null)
            {
                return;
            }
            ICultureServiceLocator cultureServiceLocator = DependencyService.Get<ICultureServiceLocator>();
            if (cultureServiceLocator == null)
            {
                throw new InvalidOperationException(string.Format(SR.MissingDependencyService, "ICultureServiceLocator"));
            }
            else
            {
                _CultureService = cultureServiceLocator.Service;
                _CultureService.CurrentCultureChanged += OnCurrentCultureChanged;
            }
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return CreateBinding(serviceProvider);
        }

        Binding IMarkupExtension<Binding>.ProvideValue(IServiceProvider serviceProvider)
        {
            return CreateBinding(serviceProvider);
        }

        private Binding CreateBinding(IServiceProvider serviceProvider)
        {
            if (Key == null)
            {
                return null;
            }
            string resourceKey = Key;
            ResourceManager resourceManager = null;
            if (Source != null)
            {
                resourceManager = GetResourceManager(Source.GetType());
                if (resourceManager == null)
                {
                    return null;
                }
            }
            else
            {
                if (!TryParseKey(resourceKey, out string typeReference, out resourceKey))
                {
                    return null;
                }
                if (typeReference != null)
                {
                    Type resourceType = GetResourceType(typeReference, serviceProvider);
                    if (resourceType == null)
                    {
                        return null;
                    }
                    resourceManager = GetResourceManager(resourceType);
                }
                else
                {
                    resourceManager = GetDefaultResourceManager();
                    if (resourceManager == null)
                    {
                        return null;
                    }
                }
            }
            BindingSource bindingSource = new BindingSource(resourceManager, resourceKey);
            Binding result = new Binding
            {
                Source = bindingSource,
                Path = "Text",
                Converter = Converter,
                ConverterParameter = ConverterParameter,
                StringFormat = StringFormat
            };
            if (Mode == TranslateExtensionMode.UpdateWhenCurrentCultureChanges)
            {
                _notificationTracker.Add(bindingSource);
            }
            return result;
        }

        private static void OnCurrentCultureChanged(object sender, EventArgs e)
        {
            _notificationTracker.NotifyAlive();
        }

        private static ResourceManager GetDefaultResourceManager()
        {
            if (_defaultResourceManager == null)
            {
                if (!Application.Current.Resources.TryGetValue(DefaultResourcesKey, out object value) || value == null)
                {
                    return ValidationFailed<ResourceManager>(null, "[Translate Xaml Extension] No default Resources found and no explicit Source indicated. You can define a default in the App.xaml resources with key 'DefaultStringResources'");
                }
                ResourceManager resourceManager = GetResourceManager(value.GetType());
                if (resourceManager == null)
                {
                    return null;
                }
                _defaultResourceManager = resourceManager;
            }
            return _defaultResourceManager;
        }

        private static bool TryParseKey(string key, out string typeReference, out string resourceKey)
        {
            typeReference = null;
            resourceKey = null;
            int num = key.IndexOf('.');
            if (num >= 0)
            {
                if (num == 0 || num + 1 == key.Length)
                {
                    return ValidationFailed(result: false, "[Translate Xaml Extension] Invalid format on text '" + key + "', format should be: <namepsace>:<resource-type-name>.<resource-key>");
                }
                typeReference = key.Substring(0, num);
                resourceKey = key.Substring(num + 1);
            }
            else
            {
                typeReference = null;
                resourceKey = key;
            }
            return true;
        }

        private static Type GetResourceType(string xamlTypeReference, IServiceProvider serviceProvider)
        {
            IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
            try
            {
                return xamlTypeResolver.Resolve(xamlTypeReference);
            }
            catch
            {
                return ValidationFailed<Type>(null, "[Translate Xaml Extension] Invalid Xaml type reference found: '" + xamlTypeReference + "'");
            }
        }

        private static ResourceManager GetResourceManager(Type resourceType, Assembly assembly = null)
        {
            string fullName = resourceType.FullName;
            if (!_ResourceManagers.TryGetValue(fullName, out ResourceManager value))
            {
                value = new ResourceManager(fullName, assembly ?? resourceType.GetTypeInfo().Assembly);
                try
                {
                    value.GetString("dummy");
                }
                catch
                {
                    return ValidationFailed<ResourceManager>(null, "Invalid resource type '" + fullName + "', it doesn't seem to be generated from a .resx file'");
                }
                _ResourceManagers.Add(fullName, value);
            }
            return value;
        }

        private static T ValidationFailed<T>(T result, string error)
        {
            return result;
        }
    }
}
