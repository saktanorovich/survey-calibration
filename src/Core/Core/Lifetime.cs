using System;

namespace Core {
    public class Lifetime : IDisposable {
        private readonly Action _cleanup;
        private bool _isDisposed;

        public Lifetime(Action cleanup) {
            Requires.NotNull(cleanup, "cleanup");
            _cleanup = cleanup;
        }

        public void Dispose() {
            if (_isDisposed) {
                return;
            }
            _isDisposed = true;
            _cleanup();
        }
    }
}
