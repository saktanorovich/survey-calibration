using System;
using System.Windows;
using System.Windows.Controls;

namespace Clients.CalibrationModule.Controls {
    public class ControlsPoolControl : UserControl {
        public static readonly DependencyProperty TypeProperty;
        public static readonly DependencyProperty PoolProperty;

        private readonly Panel _mainPanel;
        private UIElement _innerControl;

        public Type Type {
            get { return (Type)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public ControlsPool Pool {
            get { return (ControlsPool)GetValue(PoolProperty); }
            set { SetValue(PoolProperty, value); }
        }

        protected UIElement InnerControl {
            get { return _innerControl; }
            set {
                if (!Equals(_innerControl, value)) {
                    _innerControl = value;
                    OnControlChanged();
                }
            }
        }

        static ControlsPoolControl() {
            TypeProperty = DependencyProperty.Register("Type", typeof(Type), typeof(ControlsPoolControl), new PropertyMetadata(null, OnTypeChanged));
            PoolProperty = DependencyProperty.Register("Pool", typeof(ControlsPool), typeof(ControlsPoolControl), new PropertyMetadata(null, OnPoolChanged));
        }

        public ControlsPoolControl() {
            Content = (_mainPanel = new Grid());
        }

        private void OnControlChanged() {
            VerifyAccess();
            _mainPanel.Children.Clear();
            var control = InnerControl;
            if (control != null) {
                _mainPanel.Children.Add(control);
            }
        }

        private void UpdateControl() {
            if (Type == null || Pool == null)
                InnerControl = null;
            else
                InnerControl = Pool.Get(Type);
        }

        private static void OnTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = sender as ControlsPoolControl;
            if (control != null) {
                control.UpdateControl();
            }
        }

        private static void OnPoolChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var control = sender as ControlsPoolControl;
            if (control != null) {
                control.UpdateControl();
            }
        }
    }
}
