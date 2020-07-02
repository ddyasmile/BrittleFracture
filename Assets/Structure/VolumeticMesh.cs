using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuple3<T> {
    T a, b, c;

    public Tuple3(T A, T B, T C) {
        this.a = A;
        this.b = B;
        this.c = C;
    }
}

public class Tuple4<T> {
    T a, b, c, d;

    public Tuple4(T A, T B, T C, T D) {
        this.a = A;
        this.b = B;
        this.c = C;
        this.d = D;
    }
}

public class VolumeticMesh3D {
    public double volume;
    public Tuple4<int> nds;
    public List<Vector3> nodes;
}

public class VolumeticMesh2D {
    public double volume;
    public Tuple3<int> nds;
    public List<Vector2> nodes;
}