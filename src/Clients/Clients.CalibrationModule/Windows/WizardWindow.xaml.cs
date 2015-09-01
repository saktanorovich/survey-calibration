using System;
using Clients.CalibrationModule.Controls;

namespace Clients.CalibrationModule.Windows.Wizard {
    public partial class WizardWindow {
        public WizardWindow() {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e) {
            var pageViewPool = FindResource("ControlsPool") as ControlsPool;
            if (pageViewPool != null) {
                pageViewPool.Clear();
            }
            base.OnClosed(e);
        }
    }
}
