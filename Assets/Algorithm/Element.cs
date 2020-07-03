using System.Collections;
using System.Collections.Generic;

namespace ElementNS
{
    public struct Tuple3D<T> : IEnumerable
    {
        public T a, b, c;

        public Tuple3D(T A, T B, T C)
        {
            this.a = A;
            this.b = B;
            this.c = C;
        }

        public List<T> flatten()
        {
            return new List<T> { a, b, c };
        }

        public IEnumerator GetEnumerator()
        {
            return new Tuple3DEnum(a, b, c);
        }

        public class Tuple3DEnum : IEnumerator
        {
            private List<T> _list;
            private int _index = -1;

            public Tuple3DEnum(T a, T b, T c)
            {
                _list = new List<T>(){
                    a, b, c
                };
            }

            public object Current => _list[_index];

            public bool MoveNext()
            {
                _index++;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }

    public struct Tuple4D<T> : IEnumerable
    {
        public T a, b, c, d;

        public Tuple4D(T A, T B, T C, T D)
        {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
        }

        public List<T> flatten()
        {
            return new List<T> { a, b, c, d };
        }

        public IEnumerator GetEnumerator()
        {
            return new Tuple4DEnum(a, b, c, d);
        }

        public class Tuple4DEnum : IEnumerator
        {
            private List<T> _list;
            private int _index = -1;

            public Tuple4DEnum(T a, T b, T c, T d)
            {
                _list = new List<T>(){
                    a, b, c, d
                };
            }

            public object Current => _list[_index];

            public bool MoveNext()
            {
                _index++;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }

    public struct Tuple6D<T> : IEnumerable
    {
        public T a, b, c, d, e, f;

        public Tuple6D(T A, T B, T C, T D, T E, T F)
        {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
            this.e = E;
            this.f = F;
        }


        public List<T> flatten()
        {
            return new List<T> { a, b, c, d, e, f };
        }

        public IEnumerator GetEnumerator()
        {
            return new Tuple6DEnum(a, b, c, d, e, f);
        }

        public class Tuple6DEnum : IEnumerator
        {
            private List<T> _list;
            private int _index = -1;

            public Tuple6DEnum(T a, T b, T c, T d, T e, T f)
            {
                _list = new List<T>(){
                    a, b, c, d, e, f
                };
            }

            public object Current => _list[_index];

            public bool MoveNext()
            {
                _index++;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
}