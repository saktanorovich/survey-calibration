using System.Collections;
using Core.Mvvm;

namespace Clients.CalibrationModule.ViewModels.Wizard {
    public sealed class TestColumnCollection : ViewModel, IEnumerable {
        public TestColumn AccelerometerTestColumn { get; private set; }
        public TestColumn MagnetometerTestColumn { get; private set; }
        public TestColumn InclinationTestColumn { get; private set; }
        public TestColumn RollTestColumn { get; private set; }
        public TestColumn AzimuthTestColumn { get; private set; }

        public TestColumnCollection() {
            DisplayName = "Survey Calibration Test";
            AccelerometerTestColumn = new TestColumn("Accelerometer", true, true);
            MagnetometerTestColumn = new TestColumn("Magnetometer", true, true);
            InclinationTestColumn = new TestColumn("Inclination", false, true);
            RollTestColumn = new TestColumn("Roll", false, true);
            AzimuthTestColumn = new TestColumn("Azimuth", true, false);
        }

        public IEnumerator GetEnumerator() {
            return new[] {
                AccelerometerTestColumn,
                MagnetometerTestColumn,
                InclinationTestColumn,
                RollTestColumn,
                AzimuthTestColumn
            }.GetEnumerator();
        }
    }
}
