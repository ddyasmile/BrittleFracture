using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3D
{
    List<float> values;

    Matrix3D(float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9)
    {
        this.values = new List<float>{
            v1, v2, v3, v4, v5, v6, v7, v8, v9
        };
    }

    public static Matrix3D initMatrixWithValues(float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8, float v9)
    {
        return new Matrix3D(v1, v2, v3, v4, v5, v6, v7, v8, v9);
    }

    public static Matrix3D initMatrixWithRowVectors(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return new Matrix3D(v1.x, v1.y, v1.z, v2.x, v2.y, v2.z, v3.x, v3.y, v3.z);
    }

    public static Matrix3D initMatrixWithColumnVectors(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return new Matrix3D(v1.x, v2.x, v3.x, v1.y, v2.y, v3.y, v1.z, v2.z, v3.z);
    }

    public Matrix3D getTransponseMatrix()
    {
        // 0 3 6
        // 1 4 7
        // 2 5 8
        return new Matrix3D(values[0], values[3], values[6], values[1], values[4], values[7], values[2], values[5], values[8]);
    }

    public Matrix3D multiply(float constValue)
    {
        return new Matrix3D(values[0] * constValue, values[1] * constValue, values[2] * constValue,
                            values[3] * constValue, values[4] * constValue, values[5] * constValue,
                            values[6] * constValue, values[7] * constValue, values[8] * constValue);
    }

    public Vector3 multiply(Vector3 vector)
    {
        return new Vector3(Vector3.Dot(vector, new Vector3(values[0], values[1], values[2])),
                            Vector3.Dot(vector, new Vector3(values[3], values[4], values[5])),
                            Vector3.Dot(vector, new Vector3(values[6], values[7], values[8])));
    }

    public float getDeterminant()
    {
        var a1 = values[0];
        var a2 = values[1];
        var a3 = values[2];
        var b1 = values[3];
        var b2 = values[4];
        var b3 = values[5];
        var c1 = values[6];
        var c2 = values[7];
        var c3 = values[8];

        return a1 * (b2 * c3 - c2 * b3) - a2 * (b1 * c3 - c1 * b3) + a3 * (b1 * c2 - c1 * b2);
    }

    public Matrix3D getInverseMatrix()
    {
        var constValue = 1F / getDeterminant();

        var a1 = values[0];
        var a2 = values[1];
        var a3 = values[2];
        var b1 = values[3];
        var b2 = values[4];
        var b3 = values[5];
        var c1 = values[6];
        var c2 = values[7];
        var c3 = values[8];

        var resp = new Matrix3D(b2 * c3 - c2 * b3, c1 * b3 - b1 * c3, b1 * c2 - c1 * b2,
                                c2 * a3 - a2 * c3, a1 * c3 - c1 * a3, a2 * c1 - a1 * c2,
                                a2 * b3 - b2 * a3, b1 * a3 - a1 * b3, a1 * b2 - a2 * b1);

        return resp.multiply(constValue);
    }
}