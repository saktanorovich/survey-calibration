using System;

namespace Core.Calibration {
    public interface ISurveyPointProvider : IDisposable
    {
        bool HasSurveyPoint();
        SurveyPoint GetSurveyPoint();
    }
}
