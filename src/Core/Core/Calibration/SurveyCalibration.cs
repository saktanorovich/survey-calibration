using System;

namespace Core.Calibration {
    public class SurveyCalibration {
        #region Properties

        public SurveyPoint[] Source { get; internal set; }
        public SurveyPoint[] Output { get; internal set; }
        public SurveyDescriptor SourceDescriptor { get; internal set; }
        public SurveyDescriptor OutputDescriptor { get; internal set; }
        public SurveyPointTransformer Transformer { get; internal set; }
        public TimeSpan ElapsedTime { get; internal set; }

        #endregion

        #region Ctors

        internal SurveyCalibration() {
        }

        #endregion
    }
}
