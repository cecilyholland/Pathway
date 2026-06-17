using Pathway.Views;

namespace Pathway
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("plantdetail", typeof(PlantDetailPage));
        }
    }
}
