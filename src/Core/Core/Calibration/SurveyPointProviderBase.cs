using System;

namespace Core.Calibration {
    public abstract class SurveyPointProviderBase : ISurveyPointProvider {
        #region Fields

        private bool _isDisposed;

        #endregion

        #region Ctors/Dtors

        ~SurveyPointProviderBase() {
            Dispose(false);
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!_isDisposed) {
                _isDisposed = true;
                if (disposing) {
                    DisposeImpl();
                }
            }
        }

        #endregion

        #region Public Methods

        public bool HasSurveyPoint() {
            return HasSurveyPointImpl();
        }

        public SurveyPoint GetSurveyPoint() {
            return GetSurveyPointImpl();
        }

        #endregion

        #region Protected Methods

        protected abstract bool HasSurveyPointImpl();
        protected abstract SurveyPoint GetSurveyPointImpl();

        protected virtual void DisposeImpl() { }

        protected void EnsureIsNotDisposed() {
            if (_isDisposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        #endregion
    }
}
