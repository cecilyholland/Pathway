using Microsoft.Maui.Controls;
using Pathway.ViewModels;

namespace Pathway.Views
{
    public partial class PlantDetailPage : ContentPage
    {
        private readonly PlantDetailViewModel _viewModel;

        public PlantDetailPage(PlantDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        // For design-time preview
        public PlantDetailPage()
        {
            InitializeComponent();
            BindingContext = new PlantDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // This would be initialized with a plant ID in a real app, often from navigation
            if (_viewModel != null && !string.IsNullOrEmpty(PlantId))
            {
                _viewModel.InitializeAsync(PlantId);
            }
        }

        // Property to hold the plant ID when navigated to
        public string PlantId { get; set; }
    }
}