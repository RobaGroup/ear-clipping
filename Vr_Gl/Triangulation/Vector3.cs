using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Triangulation
{
    public class Vector3 : ICloneable, IEquatable<Vector3>
    {
        internal DynamicProperties DynamicProperties = new DynamicProperties();

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public Vector3 Inverse()
        {
            return this * -1;
        }

        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }

        public double X { get; set; }

        public Vector3 Absolute()
        {
             return new Vector3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
        }

        public double Y { get; set; }
        public double Z { get; set; }

        public object Clone()
        {
            return new Vector3(X, Y, Z);
        }

        public void ImplizitNegated()
        {
            X = -X; Y = -Y; Z = -Z;
        }

        public Vector3 Negated()
        {
            return new Vector3(-X, -Y, -Z);
        }

        public Vector3 Plus(Vector3 a)
        {
            return new Vector3(this.X + a.X, this.Y + a.Y, this.Z + a.Z);
        }

        public Vector3 Minus(Vector3 a)
        {
            return new Vector3(this.X - a.X, this.Y - a.Y, this.Z - a.Z);
        }

        public Vector3 Times(double a)
        {
            return new Vector3(this.X * a, this.Y * a, this.Z * a);
        }

        public Vector3 DividedBy(double a)
        {
            return new Vector3(this.X / a, this.Y / a, this.Z / a);
        }

        public double Dot(Vector3 a)
        {
            return this.X * a.X + this.Y * a.Y + this.Z * a.Z;
        }

        public Vector3 Lerp(Vector3 a, double t)
        {
            return this.Plus(a.Minus(this).Times(t));
        }

        public double Length()
        {
            return System.Math.Sqrt(Dot(this));
        }

        public double LengthSquared()
        {
            return Dot(this);
        }

        public Vector3 ShortenByLargestComponent()
        {
            if (this.LengthSquared() == 0)
                return new Vector3(0, 0, 0);
            var absNormal = Absolute();
            double largestValue = 0;
            if (absNormal.X >= absNormal.Y && absNormal.X >= absNormal.Z)
                largestValue = absNormal.X;
            else if (absNormal.Y >= absNormal.X && absNormal.Y >= absNormal.Z)
                largestValue = absNormal.Y;
            else
            {
                largestValue = absNormal.Z;
            }
            Debug.Assert(largestValue != 0);
            return this / largestValue;
        }

        public Vector3 Cross(Vector3 a)
        {
            return new Vector3(
            this.Y * a.Z - this.Z * a.Y,
            this.Z * a.X - this.X * a.Z,
            this.X * a.Y - this.Y * a.X
            );
        }

        internal bool IsZero()
        {
            return X == 0 && Y == 0 && Z == 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector3;

            if (other == null)
            {
                return false;
            }

            return Double.Equals(X, other.X) && Double.Equals(Y, other.Y) && Double.Equals(Z, other.Z);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return a.Plus(b);
        }

        public static Vector3 operator +(Vector3 a, float b)
        {
            return new Vector3(a.X + b, a.Y + b, a.Z + b);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return a.Minus(b);
        }

        public static Vector3 operator -(Vector3 a, float b)
        {
            return new Vector3(a.X - b, a.Y - b, a.Z - b);
        }

        public static Vector3 operator *(Vector3 a, double d)
        {
            return new Vector3(a.X * d, a.Y * d, a.Z * d);
        }

        public static Vector3 operator /(Vector3 a, double d)
        {
            return a.DividedBy(d);
        }

        public override string ToString()
        {
            return "Vector:" + " " + X + " " + Y + " " + Z + " ";
        }

        public static Vector3 PlaneNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            Vector3 a = v1 - v0;
            Vector3 b = v2 - v0;
            return a.Cross(b);
        }

        public bool SameDirection(Vector3 he)
        {
            var res = this.Cross(he);
            return Double.Equals(res.X, 0) && Double.Equals(res.Y, 0) && Double.Equals(res.Z, 0);
        }

        public Vector3 Normalized()
        {
            return new Vector3(this.X, this.Y, this.Z) / this.Length();
        }

        public double[] Data()
        {
            return new double[] { X, Y, Z };
        }

        internal bool IsMostlyZero()
        {
            return X <= 0.01 && Y <= 0.01 && Z <= 0.01;
        }

        public bool Equals(Vector3 other)
        {
            if (other == null)
                return false;
            return Double.Equals(this.X, other.X) && Double.Equals(this.Y, other.Y) && Double.Equals(this.Z, other.Z);
        }

        public double Angle(Vector3 other)
        {
            return Math.Acos((this.Dot(other)) / (this.Length() * other.Length()));
        }
        

        public bool IsParallel(Vector3 other)
        {
            return Math.Abs(Angle(other)) <= 0.001;
        }
    }
}