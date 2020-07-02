using System.Collections;
using System.Collections.Generic;

namespace Element {
    public class Tuple3D<T> {
        T a, b, c;

        public Tuple3D(T A, T B, T C) {
            this.a = A;
            this.b = B;
            this.c = C;
        }
    }

    public class Tuple4D<T> {
        T a, b, c, d;

        public Tuple4D(T A, T B, T C, T D) {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
        }
    }

    public class Tuple6D<T> {
        T a, b, c, d, e, f;

        public Tuple6D(T A, T B, T C, T D, T E, T F) {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
            this.e = E;
            this.f = F;
        }
    }
}