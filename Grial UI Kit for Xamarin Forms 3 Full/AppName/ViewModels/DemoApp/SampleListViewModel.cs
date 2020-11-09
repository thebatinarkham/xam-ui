using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public abstract class SampleListViewModelBase : ObservableObject
    {
        private Sample _selectedSample;

        protected SampleListViewModelBase(INavigation navigation)
            : base(listenCultureChanges: true)
        {
            Navigation = navigation;
        }

        public abstract string Title { get; }

        public abstract string Icon { get; }

        public abstract IEnumerable<Sample> Samples { get; }

        public Sample SelectedSample
        {
            get { return _selectedSample; }
            set 
            {
                if (SetProperty(ref _selectedSample, value) && value != null)
                {
                    if (value is FlowSample flow)
                    {
                        Navigation.PushAsync(new SamplesListPage(flow.FlowType));
                    }
                    else
                    {
                        Open(value, CreateSample(value.PageType));
                    }

                    _selectedSample = null;
                    NotifyPropertyChanged(nameof(SelectedSample));
                }
            }
        }

        protected INavigation Navigation { get; private set; }

        public void SampleSelected(Sample sample)
        {
            SelectedSample = sample;
        }

        protected virtual void Open(Sample value, Page page)
        {
            if (value.IsModal)
            {
                Navigation.PushModalAsync(new NavigationPage(page));
            }
            else
            {
                Navigation.PushAsync(page);
            }
        }

        protected abstract void LoadData();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private Page CreateSample(Type pageType)
        {
            return Activator.CreateInstance(pageType) as Page;
        }
    }

    public class SamplesCategoryViewModel : SampleListViewModelBase
    {
        private readonly int _categoryId;
        private SampleCategory _sampleCategory;

        public SamplesCategoryViewModel(INavigation navigation, int categoryId)
            : base(navigation)
        {
            _categoryId = categoryId;

            LoadData();
        }

        public override string Title => SampleCategory?.Name;
        public override string Icon => SampleCategory?.Icon;
        public override IEnumerable<Sample> Samples => SampleCategory?.SamplesList;

        private SampleCategory SampleCategory
        {
            get { return _sampleCategory; }
            set
            {
                _sampleCategory = value;

                NotifyPropertyChanged(nameof(Title));
                NotifyPropertyChanged(nameof(Icon));
                NotifyPropertyChanged(nameof(Samples));
            }
        }

        protected sealed override void LoadData()
        {
            SampleCategory = SamplesCatalog.SamplesCategoryList.FirstOrDefault(c => c.Id == _categoryId);
        }
    }

    public class SamplesFlowViewModel : SampleListViewModelBase
    {
        private readonly FlowType _flowType;
        private FlowSample _flow;

        public SamplesFlowViewModel(INavigation navigation, FlowType flowType)
            : base(navigation)
        {
            _flowType = flowType;

            LoadData();
        }
        
        public override string Title => Flow?.Name;
        public override string Icon => Flow?.Icon;
        public override IEnumerable<Sample> Samples => Flow?.IndividualPages;

        private FlowSample Flow
        {
            get { return _flow; }
            set
            {
                _flow = value;

                NotifyPropertyChanged(nameof(Title));
                NotifyPropertyChanged(nameof(Icon));
                NotifyPropertyChanged(nameof(Samples));
            }
        }

        protected sealed override void LoadData()
        {
            Flow = SamplesCatalog.Flows.FirstOrDefault(c => c.FlowType == _flowType);
        }

        protected override void Open(Sample value, Page page)
        {
            var navPage = Activator.CreateInstance(_flow.PageType, page) as NavigationPage;
            Navigation.PushModalAsync(navPage);
        }
    }
}