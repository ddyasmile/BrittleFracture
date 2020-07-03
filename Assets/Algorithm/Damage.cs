using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Edge2D = EdgeNS.Edge;
using Edge3D = EdgeNS.Edge;

public class Damage2D
{
    public Edge2D edge;

    // should be set between 0 & 1
    public double cutPosition;

    public Damage2D(Edge2D Edge, double CutPosition)
    {
        this.edge = Edge;
        this.cutPosition = CutPosition;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var another = (Damage2D)obj;

        return this.edge == another.edge;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Damage3D
{
    public Edge3D edge;

    // should be set between 0 & 1
    public double cutPosition;

    public Damage3D(Edge3D Edge, double CutPosition)
    {
        this.edge = Edge;
        this.cutPosition = CutPosition;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var another = (Damage3D)obj;

        return this.edge == another.edge;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}