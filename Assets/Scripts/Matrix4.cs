using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    public struct Matrix4
    {
        private float m11, m12, m13, m14;
        private float m21, m22, m23, m24;
        private float m31, m32, m33, m34;
        private float m41, m42, m43, m44;

        public Matrix4(float _11, float _12, float _13, float _14,
                       float _21, float _22, float _23, float _24,
                       float _31, float _32, float _33, float _34,
                       float _41, float _42, float _43, float _44)
        {
            m11 = _11; m12 = _12; m13 = _13; m14 = _14;
            m21 = _21; m22 = _22; m23 = _23; m24 = _24;
            m31 = _31; m32 = _32; m33 = _33; m34 = _34;
            m41 = _41; m42 = _42; m43 = _43; m44 = _44;
        }

        public static Matrix4 Identity()
        {
            return new Matrix4(1, 0, 0, 0,
                               0, 1, 0, 0,
                               0, 0, 1, 0,
                               0, 0, 0, 1);
        }

        public Matrix4 Dot(Matrix4 mat)
        {
            return new Matrix4(
                m11 * mat.m11 + m12 * mat.m21 + m13 * mat.m31 + m14 * mat.m41,
                m11 * mat.m12 + m12 * mat.m22 + m13 * mat.m32 + m14 * mat.m42,
                m11 * mat.m13 + m12 * mat.m23 + m13 * mat.m33 + m14 * mat.m43,
                m11 * mat.m14 + m12 * mat.m24 + m13 * mat.m34 + m14 * mat.m44,
                m21 * mat.m11 + m22 * mat.m21 + m23 * mat.m31 + m24 * mat.m41,
                m21 * mat.m12 + m22 * mat.m22 + m23 * mat.m32 + m24 * mat.m42,
                m21 * mat.m13 + m22 * mat.m23 + m23 * mat.m33 + m24 * mat.m43,
                m21 * mat.m14 + m22 * mat.m24 + m23 * mat.m34 + m24 * mat.m44,
                m31 * mat.m11 + m32 * mat.m21 + m33 * mat.m31 + m34 * mat.m41,
                m31 * mat.m12 + m32 * mat.m22 + m33 * mat.m32 + m34 * mat.m42,
                m31 * mat.m13 + m32 * mat.m23 + m33 * mat.m33 + m34 * mat.m43,
                m31 * mat.m14 + m32 * mat.m24 + m33 * mat.m34 + m34 * mat.m44,
                m41 * mat.m11 + m42 * mat.m21 + m43 * mat.m31 + m44 * mat.m41,
                m41 * mat.m12 + m42 * mat.m22 + m43 * mat.m32 + m44 * mat.m42,
                m41 * mat.m13 + m42 * mat.m23 + m43 * mat.m33 + m44 * mat.m43,
                m41 * mat.m14 + m42 * mat.m24 + m43 * mat.m34 + m44 * mat.m44
            );
        }



        public Vector3 GetTranslation()
        {
            return new Vector3(m14, m24, m34);
        }


        public Matrix4 Translation2D(float x, float y)
        {
            Matrix4 trans = Identity();
            trans.m14 = x;
            trans.m24 = y;
            trans.m34 = 0; // z value
            return trans;
        }







    }
}
