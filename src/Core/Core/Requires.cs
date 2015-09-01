using System;

namespace Core {
    public static class Requires {
        public static void NotNull<T>(T value, string parameterName) where T : class {
            if (ReferenceEquals(value, null)) {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string value, string parameterName) {
            NotNull(value, parameterName);
            if (value.Length != 0) {
                return;
            }
            throw new ArgumentException(parameterName);
        }

        public static void NotNullOrWhiteSpace(string value, string parameterName) {
            NotNull(value, parameterName);
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(parameterName);
            }
        }

        public static void Positive(int value, string parameterName) {
            if (value < 0) {
                throw new ArgumentException(parameterName);
            }
        }

        public static void LessOrEqual<T>(T lessValue, T greaterValue, string lessParameterName) where T : IComparable<T> {
            if (lessValue.CompareTo(greaterValue) <= 0) {
            }
            else {
                throw new ArgumentException(lessParameterName);
            }
        }
    }
}
