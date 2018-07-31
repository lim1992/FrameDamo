using System;
using UnityEngine;

namespace FixMath
{
	public struct FVector3 : IEquatable<FVector3>
	{
		public static readonly Fix64 kEpsilon = (Fix64)1E-05F;
		public static readonly Fix64 kEpsilonNormalSqrt = (Fix64)1E-15F;

		public static readonly FVector3 Zero = new FVector3();
		public static readonly FVector3 One = new FVector3(Fix64.One, Fix64.One, Fix64.One);
		public static readonly FVector3 Forward = new FVector3(Fix64.Zero, Fix64.Zero, Fix64.One);
		public static readonly FVector3 Back = new FVector3(Fix64.Zero, Fix64.Zero, -Fix64.One);
		public static readonly FVector3 Right = new FVector3(Fix64.One, Fix64.Zero, Fix64.Zero);
		public static readonly FVector3 Down = new FVector3(Fix64.Zero, -Fix64.One, Fix64.Zero);
		public static readonly FVector3 Left = new FVector3(-Fix64.One, Fix64.Zero, Fix64.Zero);
		public static readonly FVector3 Up = new FVector3(Fix64.Zero, Fix64.One, Fix64.Zero);
		public static readonly FVector3 PositiveInfinity = new FVector3(Fix64.MaxValue, Fix64.MaxValue, Fix64.MaxValue);
		public static readonly FVector3 NegativeInfinity = new FVector3(Fix64.MinValue, Fix64.MinValue, Fix64.MinValue);

		//
		// 摘要:
		//     X component of the vector.
		public Fix64 x;
		//
		// 摘要:
		//     Y component of the vector.
		public Fix64 y;
		//
		// 摘要:
		//     Z component of the vector.
		public Fix64 z;


		//
		// 摘要:
		//     Creates a new vector with given x, y components and sets z to zero.
		//
		// 参数:
		//   x:
		//
		//   y:
		public FVector3(Fix64 _x, Fix64 _y)
		{
// 			x = new Fix64();
// 			y = new Fix64();
// 			z = new Fix64();

			x = _x;
			y = _y;
			z = Fix64.Zero;
		}
		//
		// 摘要:
		//     Creates a new vector with given x, y, z components.
		//
		// 参数:
		//   x:
		//
		//   y:
		//
		//   z:
		public FVector3(Fix64 _x, Fix64 _y, Fix64 _z)
		{
			x = _x;
			y = _y;
			z = _z;
		}
		//
		// 摘要:
		//     Creates a new vector with given x, y, z components.
		//
		// 参数:
		//   x:
		//
		//   y:
		//
		//   z:
		public FVector3(float _x, float _y, float _z)
		{
			x = (Fix64)_x;
			y = (Fix64)_y;
			z = (Fix64)_z;
		}

		// 		public float this[int index] { get; set; }
		//
		// 摘要:
		//     Returns the squared length of this vector (Read Only).
		public Fix64 sqrMagnitude { get { return (x * x + y * y + z * z); } }
		//
		// 摘要:
		//     Returns this vector with a magnitude of 1 (Read Only).
		public FVector3 normalized
		{
			get
			{
				// test for zero length vector
				// if found return zero vector
				if (magnitude < kEpsilon)
					return Zero;

				Fix64 length_inv = Fix64.One / magnitude;

				// compute normalized version of vector
				FVector3 n = new FVector3();
				n.x = x * length_inv;
				n.y = y * length_inv;
				n.z = z * length_inv;
				return n;
			}
		}
		//
		// 摘要:
		//     Returns the length of this vector (Read Only).
		public Fix64 magnitude { get { return (Fix64.Sqrt(x * x + y * y + z * z)); } }

		//
		// 摘要:
		//     Returns the angle in degrees between from and to.
		//
		// 参数:
		//   from:
		//     The vector from which the angular difference is measured.
		//
		//   to:
		//     The vector to which the angular difference is measured.
		//
		// 返回结果:
		//     The angle in degrees between the two vectors.
		public static Fix64 Angle(FVector3 from, FVector3 to)
		{
			return Fix64.Acos(Dot(from, to)/(from.magnitude * to.magnitude));
		}
// 		//
// 		// 摘要:
// 		//     Returns a copy of vector with its magnitude clamped to maxLength.
// 		//
// 		// 参数:
// 		//   vector:
// 		//
// 		//   maxLength:
		public static FVector3 ClampMagnitude(FVector3 vector, Fix64 maxLength)
		{
			if (vector.magnitude > maxLength)
			{
				return vector * (maxLength / vector.magnitude);
			}
			else
			{
				return new FVector3(vector.x, vector.y, vector.z);
			}
		}
		//
		// 摘要:
		//     Cross Product of two vectors.
		//
		// 参数:
		//   lhs:
		//
		//   rhs:
		public static FVector3 Cross(FVector3 lhs, FVector3 rhs)
		{
			FVector3 re = new FVector3();
			re.x = ((lhs.y * rhs.z) - (lhs.z * rhs.y));
			re.y = -((lhs.x * rhs.z) - (lhs.z * rhs.x));
			re.z = ((lhs.x * rhs.y) - (lhs.y * rhs.x));
			return re;
		}
		//
		// 摘要:
		//     Returns the distance between a and b.
		//
		// 参数:
		//   a:
		//
		//   b:
		public static Fix64 Distance(FVector3 a, FVector3 b)
		{
			return Fix64.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
		}
		//
		// 摘要:
		//     Dot Product of two vectors.
		//
		// 参数:
		//   lhs:
		//
		//   rhs:
		public static Fix64 Dot(FVector3 lhs, FVector3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}
		//
		// 摘要:
		//     Linearly interpolates between two vectors.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FVector3 Lerp(FVector3 a, FVector3 b, Fix64 t)
		{
			FVector3 vlerp= new FVector3();
			vlerp = a + (b - a) * t;
			return vlerp;
		}
		//
		// 摘要:
		//     Linearly interpolates between two vectors.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FVector3 LerpUnclamped(FVector3 a, FVector3 b, Fix64 t)
		{
			FVector3 vlerp = new FVector3();
			vlerp = a + (b - a) * t;
			return vlerp;
		}

		public static Fix64 Magnitude(FVector3 vector)
		{
			return vector.magnitude;
		}
		//
		// 摘要:
		//     Returns a vector that is made from the largest components of two vectors.
		//
		// 参数:
		//   lhs:
		//
		//   rhs:
		public static FVector3 Max(FVector3 lhs, FVector3 rhs)
		{
			FVector3 vmax = new FVector3();
			vmax.x = lhs.x > rhs.x ? lhs.x : rhs.x;
			vmax.y = lhs.y > rhs.y ? lhs.y : rhs.y;
			vmax.z = lhs.z > rhs.z ? lhs.z : rhs.z;
			return vmax;
		}
		//
		// 摘要:
		//     Returns a vector that is made from the smallest components of two vectors.
		//
		// 参数:
		//   lhs:
		//
		//   rhs:
		public static FVector3 Min(FVector3 lhs, FVector3 rhs)
		{
			FVector3 vmin = new FVector3();
			vmin.x = lhs.x < rhs.x ? lhs.x : rhs.x;
			vmin.y = lhs.y < rhs.y ? lhs.y : rhs.y;
			vmin.z = lhs.z < rhs.z ? lhs.z : rhs.z;
			return vmin;
		}
		//
		// 摘要:
		//     Moves a point current in a straight line towards a target point.
		//
		// 参数:
		//   current:
		//
		//   target:
		//
		//   maxDistanceDelta:
		public static FVector3 MoveTowards(FVector3 current, FVector3 target, Fix64 maxDistanceDelta)
		{
			FVector3 vlerp = new FVector3();
			vlerp = current + (target - current).normalized * maxDistanceDelta;
			return vlerp;
		}
		//
		// 摘要:
		//     Makes this vector have a magnitude of 1.
		//
		// 参数:
		//   value:
		public static FVector3 Normalize(FVector3 value)
		{
			value.Normalize();
			return value;
		}
		public static void OrthoNormalize(ref FVector3 normal, ref FVector3 tangent, ref FVector3 binormal)
		{
			normal.Normalize();
			tangent = tangent - (FVector3.Dot(tangent, normal) / normal.magnitude * normal.magnitude) * normal;
			tangent.Normalize();
			binormal = binormal - (FVector3.Dot(binormal, normal) / normal.magnitude * normal.magnitude) * normal - (FVector3.Dot(binormal, tangent) / tangent.magnitude * tangent.magnitude) * tangent;
			binormal.Normalize();
		}
		public static void OrthoNormalize(ref FVector3 normal, ref FVector3 tangent)
		{
			normal.Normalize();
			tangent = tangent - (FVector3.Dot(tangent, normal) / normal.magnitude * normal.magnitude) * normal;
			tangent.Normalize();
		}
		//
		// 摘要:
		//     Projects a vector onto another vector.
		//
		// 参数:
		//   vector:
		//
		//   onNormal:
		public static FVector3 Project(FVector3 vector, FVector3 onNormal)
		{
			FVector3 vproject = new FVector3();
			onNormal.Normalize();
			vproject = FVector3.Dot(vector, onNormal) * onNormal;
			return vproject;
		}
		//
		// 摘要:
		//     Projects a vector onto a plane defined by a normal orthogonal to the plane.
		//
		// 参数:
		//   vector:
		//
		//   planeNormal:
		public static FVector3 ProjectOnPlane(FVector3 vector, FVector3 planeNormal)
		{
			FVector3 vproject = new FVector3();
			vproject = vector - FVector3.Project(vector, planeNormal);
			return vproject;
		}
		//
		// 摘要:
		//     Reflects a vector off the plane defined by a normal.
		//
		// 参数:
		//   inDirection:
		//
		//   inNormal:
		public static FVector3 Reflect(FVector3 inDirection, FVector3 inNormal)
		{
			FVector3 vproject = new FVector3();
			vproject = inDirection - FVector3.Project(inDirection, inNormal) * Fix64.Two;
			return vproject;
		}
		#region 未实现 
		//
		// 摘要:
		//     Rotates a vector current towards target.
		//
		// 参数:
		//   current:
		//     The vector being managed.
		//
		//   target:
		//     The vector.
		//
		//   maxRadiansDelta:
		//     The distance between the two vectors in radians.
		//
		//   maxMagnitudeDelta:
		//     The length of the radian.
		//
		// 返回结果:
		//     The location that RotateTowards generates.
		public static FVector3 RotateTowards(FVector3 current, FVector3 target, Fix64 maxRadiansDelta, Fix64 maxMagnitudeDelta)
		{
			FQuaternion q = new FQuaternion();
			FQuaternion.AngleAxis(maxRadiansDelta, Cross(current, target));
			return q * current;
		}
		//
		// 摘要:
		//     Multiplies two vectors component-wise.
		//
		// 参数:
		//   a:
		//
		//   b:
		public static FVector3 Scale(FVector3 a, FVector3 b)
		{
			return new FVector3(a.x * b.x, a.y * b.y * a.z * b.z);
		}
		//
		// 摘要:
		//     Returns the signed angle in degrees between from and to.
		//
		// 参数:
		//   from:
		//     The vector from which the angular difference is measured.
		//
		//   to:
		//     The vector to which the angular difference is measured.
		//
		//   axis:
		//     A vector around which the other vectors are rotated.
		public static Fix64 SignedAngle(FVector3 from, FVector3 to, FVector3 axis)
		{
			if (Angle(Cross(from, to), axis) > Fix64.PiOver2)
			{
				return -Fix64.Acos(Dot(from, to) / (from.magnitude * to.magnitude));

			}
			else
			{
				return Fix64.Acos(Dot(from, to) / (from.magnitude * to.magnitude));
			}
		}
		//
		// 摘要:
		//     Spherically interpolates between two vectors.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FVector3 Slerp(FVector3 a, FVector3 b, Fix64 t)
		{
			FQuaternion qa = new FQuaternion(a);
			FQuaternion qb = new FQuaternion(b);
			FQuaternion qend = FQuaternion.Lerp(qa, qb, t);
			return new FVector3(qend.x, qend.y, qend.z);
		}
		//
		// 摘要:
		//     Spherically interpolates between two vectors.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FVector3 SlerpUnclamped(FVector3 a, FVector3 b, Fix64 t)
		{
			FQuaternion qa = new FQuaternion(a);
			FQuaternion qb = new FQuaternion(a);
			FQuaternion qend = FQuaternion.Lerp(qa, qb, t);
			return new FVector3(qend.x, qend.y, qend.z);

		}
// 		public static FVector3 SmoothDamp(FVector3 current, FVector3 target, ref FVector3 currentVelocity, Fix64 smoothTime, Fix64 maxSpeed, Fix64 deltaTime);
// 		public static FVector3 SmoothDamp(FVector3 current, FVector3 target, ref FVector3 currentVelocity, Fix64 smoothTime, Fix64 maxSpeed);
// 		public static FVector3 SmoothDamp(FVector3 current, FVector3 target, ref FVector3 currentVelocity, Fix64 smoothTime);
//  	public static float SqrMagnitude(FVector3 vector)
		#endregion
		public bool Equals(FVector3 other)
		{
			return x == other.x && y == other.y && z == other.z;
		}
		//
		// 摘要:
		//     Returns true if the given vector is exactly equal to this vector.
		//
		// 参数:
		//   other:
		public override bool Equals(object other)
		{
			return other is FVector3 && Equals((FVector3)other);
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
		}
		public void Normalize()
		{
			if (magnitude < kEpsilon)
			{

			}
			else
			{
				Fix64 length_inv = Fix64.One / magnitude;
				// compute normalized version of vector
				x = x * length_inv;
				y = y * length_inv;
				z = z * length_inv;
			}

		}
		//
		// 摘要:
		//     Multiplies every component of this vector by the same component of scale.
		//
		// 参数:
		//   scale:
		public void Scale(FVector3 scale)
		{
			x *= scale.x;
			y *= scale.y;
			z *= scale.z;
		}
		//
		// 摘要:
		//     Set x, y and z components of an existing Vector3.
		//
		// 参数:
		//   newX:
		//
		//   newY:
		//
		//   newZ:
		public void Set(Fix64 newX, Fix64 newY, Fix64 newZ)
		{
			x = newX;
			y = newY;
			z = newZ;

		}
		//
		// 摘要:
		//     Returns a nicely formatted string for this vector.
		//
		// 参数:
		//   format:
		public string ToString(string format)
		{
			return string.Format(format, x, y, z);
		}
		//
		// 摘要:
		//     Returns a nicely formatted string for this vector.
		//
		// 参数:
		//   format:
		public override string ToString()
		{
			return ToString("x: {0} y: {1} z: {2}");
		}

		public Vector3 asVector3
		{
			get { return new Vector3((float)x, (float)y, (float)z); }
		}

		public static FVector3 operator +(FVector3 a, FVector3 b)
		{
			FVector3 vsum = new FVector3(a.x + b.x, a.y + b.y, a.z + b.z);
			return vsum;
		}
		public static FVector3 operator -(FVector3 a)
		{
			FVector3 vdiff = new FVector3(-a.x, -a.y, -a.z);
			return vdiff;
		}
		public static FVector3 operator -(FVector3 a, FVector3 b)
		{
			FVector3 vdiff = new FVector3(a.x - b.x, a.y - b.y, a.z - b.z);
			return vdiff;
		}
		public static FVector3 operator *(FVector3 a, Fix64 d)
		{
			FVector3 vproduct = new FVector3(a.x * d, a.y * d, a.z * d);
			return vproduct;
		}
		public static FVector3 operator *(Fix64 d, FVector3 a)
		{
			FVector3 vproduct = new FVector3(a.x * d, a.y * d, a.z * d);
			return vproduct;
		}
		public static FVector3 operator /(FVector3 a, Fix64 d)
		{
			FVector3 vquotient = new FVector3(a.x / d, a.y / d, a.z / d);
			return vquotient;
		}
		public static bool operator ==(FVector3 lhs, FVector3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}
		public static bool operator !=(FVector3 lhs, FVector3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}
	}
}
