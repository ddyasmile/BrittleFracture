using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge2D = EdgeNS.Edge;
using Edge3D = EdgeNS.Edge;

public class Damage2D {
    public Edge2D edge;

    // should be set between 0 & 1
    public double cutPosition;
}

public class Damage3D {
    public Edge3D edge;

    // should be set between 0 & 1
    public double cutPosition;
}