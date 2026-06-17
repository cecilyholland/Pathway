using Microsoft.Maui.Controls;
using Pathway.ViewModels;

namespace Pathway.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly MapViewModel _viewModel;

        public MapPage(MapViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // For design-time preview
        public MapPage()
        {
            InitializeComponent();
            BindingContext = new MapViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh data when page appears
            if (_viewModel != null)
            {
                // This would trigger loading of data in a real app
            }
        }
    }
}