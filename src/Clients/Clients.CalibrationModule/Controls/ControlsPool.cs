using System;
using System.Collections.Generic;
using System.Windows;
using Core;

namespace Clients.CalibrationModule.Controls {
    public class ControlsPool {
        private readonly IDictionary<Type, UIElement> _controls = new Dictionary<Type, UIElement>();

        public UIElement Get(Type type) {
            Requires.NotNull(type, "type");
            UIElement uiElement;
            if (!_controls.TryGetValue(type, out uiElement)) {
                uiElement = Activator.CreateInstance(type) as UIElement;
                _controls[type] = uiElement;
            }
            return uiElement;
        }

        public void Clear() {
            _controls.Clear();
        }
    }
}
