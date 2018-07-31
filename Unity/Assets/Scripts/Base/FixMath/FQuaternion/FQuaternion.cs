using System;
using UnityEngine.Internal;

namespace FixMath
{
	//
	// 摘要:
	//     Quaternions are used to represent rotations.
	public struct FQuaternion : IEquatable<FQuaternion>
	{
		public static readonly Fix64 kEpsilon = (Fix64)1E-06F;
		public static readonly Fix64 kThreshold = Fix64.Half - kEpsilon;

		
		public static readonly FQuaternion Identity = new FQuaternion(Fix64.Zero, Fix64.Zero, Fix64.Zero, Fix64.Zero);

		//
		// 摘要:
		//     X component of the Quaternion. Don't modify this directly unless you know quaternions
		//     inside out.
		public Fix64 x;
		//
		// 摘要:
		//     Y component of the Quaternion. Don't modify this directly unless you know quaternions
		//     inside out.
		public Fix64 y;
		//
		// 摘要:
		//     Z component of the Quaternion. Don't modify this directly unless you know quaternions
		//     inside out.
		public Fix64 z;
		//
		// 摘要:
		//     W component of the Quaternion. Don't modify this directly unless you know quaternions
		//     inside out.
		public Fix64 w;

		//
		// 摘要:
		//     Constructs new Quaternion with given x,y,z,w components.
		//
		// 参数:
		//   x:
		//
		//   y:
		//
		//   z:
		//
		//   w:
		public FQuaternion(Fix64 _x, Fix64 _y, Fix64 _z, Fix64 _w)
		{
			x = _x;
			y = _y;
			z = _z;
			w = _w;
		}

		public FQuaternion(FVector3 v)
		{
			x = v.x;
			y = v.y;
			z = v.z;
			w = Fix64.Zero;
		}

		// 		public float this[int index] { get; set; }

		//
		// 摘要:
		//     The identity rotation (Read Only).
		public static FQuaternion identity { get { return Identity; } }
		//
		// 摘要:
		//     Returns the euler angle representation of the rotation.
		public FVector3 eulerAngles {
			get
			{
				Fix64 TEST = w * y - x * z;

				if (TEST < -kThreshold || TEST > kThreshold) // 奇异姿态,俯仰角为±90°
				{
					Fix64 sign = Fix64.Signfix64(TEST);

					return new FVector3(Fix64.Zero, sign * Fix64.PiOver2, -Fix64.Two * sign * Fix64.Atan2(x, w));

				}
				else
				{
					Fix64 r11 = Fix64.Two * (x * y + w * z);
					Fix64 r12 = w * w + x * x - y * y - z * z;
					Fix64 r21 = -Fix64.Two * (x * z - w * y);
					Fix64 r31 = Fix64.Two * (y * z + w * x);
					Fix64 r32 = w * w - x * x - y * y + z * z;

					return new FVector3(Fix64.Atan2(r31, r32), Fix64.Asin(r21), Fix64.Atan2(r11, r12));

				}
			}
			set
			{
				this = Euler(value);
			}
		}
		//
		// 摘要:
		//     Returns this quaternion with a magnitude of 1 (Read Only).

		public Fix64 norm2 { get { return w * w + x * x + y * y + z * z; } }

		public FQuaternion normalized {
			get {

				// this functions normalizes the sent quaternion in place

				// compute length
				Fix64 qlength_inv = Fix64.One / (Fix64.Sqrt(norm2));

				// now normalize
				return new FQuaternion(x * qlength_inv, y * qlength_inv, z * qlength_inv, w * qlength_inv);
			}
		}
		#region 这是什么东西?
		// 		//
		// 		// 摘要:
		// 		//     Returns the angle in degrees between two rotations a and b.
		// 		//
		// 		// 参数:
		// 		//   a:
		// 		//
		// 		//   b:
		// 		public static float Angle(Quaternion a, Quaternion b);
		#endregion
		//
		// 摘要:
		//     Creates a rotation which rotates angle degrees around axis.
		//
		// 参数:
		//   angle:
		//
		//   axis:
		public static FQuaternion AngleAxis(Fix64 angle, FVector3 axis)
		{
			FQuaternion q = new FQuaternion();
			q.FVector3AndTheta(axis, angle);
			return q;
		}
		//
		// 摘要:
		//     The dot product between two rotations.
		//
		// 参数:
		//   a:
		//
		//   b:
		public static Fix64 Dot(FQuaternion a, FQuaternion b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		public void FVector3AndTheta(FVector3 v, Fix64 theta)
		{
			// initializes a quaternion based on a 3d direction vector and angle
			// note the direction vector must be a unit vector
			// and the angle is in rads

			Fix64 theta_div_2 = theta * Fix64.Half; // compute theta/2

			// compute the quaterion, note this is from chapter 4
			// pre-compute to save time
			Fix64 sinf_theta = Fix64.Sin(theta_div_2);

			x = sinf_theta * v.x;
			y = sinf_theta * v.y;
			z = sinf_theta * v.z;
			w = Fix64.Cos(theta_div_2);
		}

		//
		// 摘要:
		//     Returns a rotation that rotates z degrees around the z axis, x degrees around
		//     the x axis, and y degrees around the y axis.
		//
		// 参数:
		//   euler:
		public static FQuaternion Euler(FVector3 euler)
		{
			// this function intializes a quaternion based on the zyx
			// multiplication order of the angles that are parallel to the
			// zyx axis respectively, note there are 11 other possibilities
			// this is just one, later we may make a general version of the
			// the function

			// precompute values
			Fix64 cos_z_2 = Fix64.Half * Fix64.Cos(euler.z);
			Fix64 cos_y_2 = Fix64.Half * Fix64.Cos(euler.y);
			Fix64 cos_x_2 = Fix64.Half * Fix64.Cos(euler.x);

			Fix64 sin_z_2 = Fix64.Half * Fix64.Sin(euler.z);
			Fix64 sin_y_2 = Fix64.Half * Fix64.Sin(euler.y);
			Fix64 sin_x_2 = Fix64.Half * Fix64.Sin(euler.x);

			// and now compute quaternion
			Fix64 w = cos_z_2 * cos_y_2 * cos_x_2 + sin_z_2 * sin_y_2 * sin_x_2;
			Fix64 x = cos_z_2 * cos_y_2 * sin_x_2 - sin_z_2 * sin_y_2 * cos_x_2;
			Fix64 y = cos_z_2 * sin_y_2 * cos_x_2 + sin_z_2 * cos_y_2 * sin_x_2;
			Fix64 z = sin_z_2 * cos_y_2 * cos_x_2 - cos_z_2 * sin_y_2 * sin_x_2;

			return new FQuaternion(x, y, z, w);
		}
		//
		// 摘要:
		//     Returns a rotation that rotates z degrees around the z axis, x degrees around
		//     the x axis, and y degrees around the y axis.
		//
		// 参数:
		//   x:
		//
		//   y:
		//
		//   z:
		public static FQuaternion Euler(Fix64 _x, Fix64 _y, Fix64 _z)
		{
			// this function intializes a quaternion based on the zyx
			// multiplication order of the angles that are parallel to the
			// zyx axis respectively, note there are 11 other possibilities
			// this is just one, later we may make a general version of the
			// the function

			// precompute values
			Fix64 cos_z_2 = Fix64.Half * Fix64.Cos(_z);
			Fix64 cos_y_2 = Fix64.Half * Fix64.Cos(_y);
			Fix64 cos_x_2 = Fix64.Half * Fix64.Cos(_x);

			Fix64 sin_z_2 = Fix64.Half * Fix64.Sin(_z);
			Fix64 sin_y_2 = Fix64.Half * Fix64.Sin(_y);
			Fix64 sin_x_2 = Fix64.Half * Fix64.Sin(_x);

			// and now compute quaternion
			Fix64 w = cos_z_2 * cos_y_2 * cos_x_2 + sin_z_2 * sin_y_2 * sin_x_2;
			Fix64 x = cos_z_2 * cos_y_2 * sin_x_2 - sin_z_2 * sin_y_2 * cos_x_2;
			Fix64 y = cos_z_2 * sin_y_2 * cos_x_2 + sin_z_2 * cos_y_2 * sin_x_2;
			Fix64 z = sin_z_2 * cos_y_2 * cos_x_2 - cos_z_2 * sin_y_2 * sin_x_2;

			return new FQuaternion(x, y, z, w);
		}
		//
		// 摘要:
		//     Creates a rotation which rotates from fromDirection to toDirection.
		//
		// 参数:
		//   fromDirection:
		//
		//   toDirection:
		public static FQuaternion FromToRotation(FVector3 fromDirection, FVector3 toDirection)
		{
			FVector3 vcorss = FVector3.Cross(fromDirection, toDirection);
			FQuaternion q = new FQuaternion();
			q.FVector3AndTheta(vcorss, FVector3.Angle(fromDirection, toDirection));
			return q;
		}
		//
		// 摘要:
		//     Returns the Inverse of rotation.
		//
		// 参数:
		//   rotation:
		public static FQuaternion Inverse(FQuaternion rotation)
		{
			// this function computes the inverse of a general quaternion
			// and returns result
			// in general, q-1 = *q/|q|2
			// compute norm squared
			Fix64 norm2_inv = Fix64.One / rotation.norm2;

			// and plug in
			return new FQuaternion(-rotation.x * norm2_inv, -rotation.y * norm2_inv, -rotation.z * norm2_inv, rotation.w * norm2_inv);
		}

		public static FQuaternion Conjugate(FQuaternion rotation)
		{
			return new FQuaternion(-rotation.x, -rotation.y, -rotation.z, rotation.w);
		}

		public static FQuaternion UnitInverse(FQuaternion rotation)
		{
			// and plug in
			return new FQuaternion(-rotation.x, -rotation.y, -rotation.z, rotation.w);
		}
		//
		// 摘要:
		//     Interpolates between a and b by t and normalizes the result afterwards. The parameter
		//     t is clamped to the range [0, 1].
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FQuaternion Lerp(FQuaternion a, FQuaternion b, Fix64 t)
		{
			a.Normalize();
			b.Normalize();

			Fix64 cosa = Dot(a, b);

// 			// If the dot product is negative, the quaternions have opposite handed-ness and slerp won't take
// 			// the shorter path. Fix by reversing one quaternion.
// 			if (cosa < Fix64.Zero)
// 			{
// 				b.x = -b.x;
// 				b.y = -b.y;
// 				b.z = -b.z;
// 				b.w = -b.w;
// 				cosa = -cosa;
// 			}

			Fix64 k0, k1;

			// If the inputs are too close for comfort, linearly interpolate
			if (Fix64.Abs(cosa) > (Fix64)0.9995f)
			{
				k0 = Fix64.One - t;
				k1 = t;
			}
			else
			{
				Fix64 sina = Fix64.Sqrt(Fix64.One - cosa * cosa);
				Fix64 theta = Fix64.Atan2(sina, cosa);
				k0 = Fix64.Sin((Fix64.One - t) * theta) / sina;
				k1 = Fix64.Sin(t * theta) / sina;
			}
			return new FQuaternion(a.x * k0 + b.x * k1, a.y * k0 + b.y * k1, a.z * k0 + b.z * k1, a.w * k0 + b.w * k1);
		}
		//
		// 摘要:
		//     Interpolates between a and b by t and normalizes the result afterwards. The parameter
		//     t is not clamped.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FQuaternion LerpUnclamped(FQuaternion a, FQuaternion b, Fix64 t)
		{
			a.Normalize();
			b.Normalize();

			Fix64 cosa = Dot(a, b);

			// If the dot product is negative, the quaternions have opposite handed-ness and slerp won't take
			// the shorter path. Fix by reversing one quaternion.
			if (cosa < Fix64.Zero)
			{
				b.x = -b.x;
				b.y = -b.y;
				b.z = -b.z;
				b.w = -b.w;
				cosa = -cosa;
			}

			Fix64 k0, k1;

			// If the inputs are too close for comfort, linearly interpolate
			if (cosa > (Fix64)0.9995f)
			{
				k0 = Fix64.One - t;
				k1 = t;
			}
			else
			{
				Fix64 sina = Fix64.Sqrt(Fix64.One - cosa * cosa);
				Fix64 theta = Fix64.Atan2(sina, cosa);
				k0 = Fix64.Sin((Fix64.One - t) * theta) / sina;
				k1 = Fix64.Sin(t * theta) / sina;
			}
			return new FQuaternion(a.x * k0 + b.x * k1, a.y * k0 + b.y * k1, a.z * k0 + b.z * k1, a.w * k0 + b.w * k1);
		}
// 		//
// 		// 摘要:
// 		//     Creates a rotation with the specified forward and upwards directions.
// 		//
// 		// 参数:
// 		//   forward:
// 		//     The direction to look in.
// 		//
// 		//   upwards:
// 		//     The vector that defines in which direction up is.
// 		[ExcludeFromDocs]
// 		public static Quaternion LookRotation(Vector3 forward);
// 		//
// 		// 摘要:
// 		//     Creates a rotation with the specified forward and upwards directions.
// 		//
// 		// 参数:
// 		//   forward:
// 		//     The direction to look in.
// 		//
// 		//   upwards:
// 		//     The vector that defines in which direction up is.
// 		public static Quaternion LookRotation(Vector3 forward, [DefaultValue("Vector3.up")] Vector3 upwards);
		//
		// 摘要:
		//     Converts this quaternion to one with the same orientation but with a magnitude
		//     of 1.
		//
		// 参数:
		//   q:
		public static FQuaternion Normalize(FQuaternion q)
		{
			q.Normalize();
			return q;
		}
// 		//
// 		// 摘要:
// 		//     Rotates a rotation from towards to.
// 		//
// 		// 参数:
// 		//   from:
// 		//
// 		//   to:
// 		//
// 		//   maxDegreesDelta:
// 		public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta);
		//
		// 摘要:
		//     Spherically interpolates between a and b by t. The parameter t is clamped to
		//     the range [0, 1].
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FQuaternion Slerp(FQuaternion a, FQuaternion b, Fix64 t)
		{
			a.Normalize();
			b.Normalize();

			Fix64 cosa = Dot(a, b);

			// If the dot product is negative, the quaternions have opposite handed-ness and slerp won't take
			// the shorter path. Fix by reversing one quaternion.
			if (cosa < Fix64.Zero)
			{
				b.x = -b.x;
				b.y = -b.y;
				b.z = -b.z;
				b.w = -b.w;
				cosa = -cosa;
			}

			Fix64 k0, k1;

			// If the inputs are too close for comfort, linearly interpolate
			if (cosa > (Fix64)0.9995f)
			{
				k0 = Fix64.One - t;
				k1 = t;
			}
			else
			{
				Fix64 sina = Fix64.Sqrt(Fix64.One - cosa * cosa);
				Fix64 theta = Fix64.Atan2(sina, cosa);
				k0 = Fix64.Sin((Fix64.One - t) * theta) / sina;
				k1 = Fix64.Sin(t * theta) / sina;
			}
			return new FQuaternion(a.x * k0 + b.x * k1, a.y * k0 + b.y * k1, a.z * k0 + b.z * k1, a.w * k0 + b.w * k1);
		}
		//
		// 摘要:
		//     Spherically interpolates between a and b by t. The parameter t is not clamped.
		//
		// 参数:
		//   a:
		//
		//   b:
		//
		//   t:
		public static FQuaternion SlerpUnclamped(FQuaternion a, FQuaternion b, Fix64 t)
		{
			a.Normalize();
			b.Normalize();

			Fix64 cosa = Dot(a, b);

			// If the dot product is negative, the quaternions have opposite handed-ness and slerp won't take
			// the shorter path. Fix by reversing one quaternion.
			if (cosa < Fix64.Zero)
			{
				b.x = -b.x;
				b.y = -b.y;
				b.z = -b.z;
				b.w = -b.w;
				cosa = -cosa;
			}

			Fix64 k0, k1;

			// If the inputs are too close for comfort, linearly interpolate
			if (cosa > (Fix64)0.9995f)
			{
				k0 = Fix64.One - t;
				k1 = t;
			}
			else
			{
				Fix64 sina = Fix64.Sqrt(Fix64.One - cosa * cosa);
				Fix64 theta = Fix64.Atan2(sina, cosa);
				k0 = Fix64.Sin((Fix64.One - t) * theta) / sina;
				k1 = Fix64.Sin(t * theta) / sina;
			}
			return new FQuaternion(a.x * k0 + b.x * k1, a.y * k0 + b.y * k1, a.z * k0 + b.z * k1, a.w * k0 + b.w * k1);
		}
		public bool Equals(FQuaternion other)
		{
			return x == other.x && y == other.y && z == other.z && w == other.w;
		}
		public override bool Equals(object other)
		{
			return other is FQuaternion && Equals((FQuaternion)other);
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() + y.GetHashCode() + z.GetHashCode() + w.GetHashCode();
		}
		public void Normalize()
		{
			Fix64 qlength_inv = Fix64.One / (Fix64.Sqrt(norm2));

			// now normalize
			x *= qlength_inv;
			y *= qlength_inv;
			z *= qlength_inv;
			w *= qlength_inv;
		}
		//
		// 摘要:
		//     Set x, y, z and w components of an existing Quaternion.
		//
		// 参数:
		//   newX:
		//
		//   newY:
		//
		//   newZ:
		//
		//   newW:
		public void Set(Fix64 newX, Fix64 newY, Fix64 newZ, Fix64 newW)
		{
			x = newX;
			y = newY;
			z = newZ;
			w = newW;
		}
// 		//
// 		// 摘要:
// 		//     Creates a rotation which rotates from fromDirection to toDirection.
// 		//
// 		// 参数:
// 		//   fromDirection:
// 		//
// 		//   toDirection:
// 		public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection);
// 		//
// 		// 摘要:
// 		//     Creates a rotation with the specified forward and upwards directions.
// 		//
// 		// 参数:
// 		//   view:
// 		//     The direction to look in.
// 		//
// 		//   up:
// 		//     The vector that defines in which direction up is.
// 		public void SetLookRotation(Vector3 view, [DefaultValue("Vector3.up")] Vector3 up);
// 		//
// 		// 摘要:
// 		//     Creates a rotation with the specified forward and upwards directions.
// 		//
// 		// 参数:
// 		//   view:
// 		//     The direction to look in.
// 		//
// 		//   up:
// 		//     The vector that defines in which direction up is.
// 		[ExcludeFromDocs]
// 		public void SetLookRotation(Vector3 view);
		public void ToAngleAxis(out Fix64 angle, out FVector3 axis)
		{
			// this function converts a unit quaternion into a unit direction
			// vector and rotation angle about that vector

			// extract theta
			angle = Fix64.Acos(w);

			// pre-compute to save time
			Fix64 sinf_theta_inv = Fix64.One / Fix64.Sin(angle);

			// now the vector
			axis.x = x * sinf_theta_inv;
			axis.y = y * sinf_theta_inv;
			axis.z = z * sinf_theta_inv;

			// multiply by 2
			angle *= Fix64.Two;
		}
		//
		// 摘要:
		//     Returns a nicely formatted string of the Quaternion.
		//
		// 参数:
		//   format:
		public string ToString(string format)
		{
			return string.Format(format, x, y, z, w);
		}
		//
		// 摘要:
		//     Returns a nicely formatted string of the Quaternion.
		//
		// 参数:
		//   format:
		public override string ToString()
		{
			return ToString("x: {0} y: {1} z: {2} w: {3}");
		}

		public static FVector3 operator *(FQuaternion rotation, FVector3 point)
		{
			FQuaternion qpoint = new FQuaternion(point);
			FQuaternion qend = rotation * qpoint * Inverse(rotation);
			return new FVector3(qend.x, qend.y, qend.z);
		}
		public static FQuaternion operator *(FQuaternion lhs, FQuaternion rhs)
		{
			// this function multiplies two quaternions

			// this is the brute force method
			//qprod->w = q1->w*q2->w - q1->x*q2->x - q1->y*q2->y - q1->z*q2->z;
			//qprod->x = q1->w*q2->x + q1->x*q2->w + q1->y*q2->z - q1->z*q2->y;
			//qprod->y = q1->w*q2->y - q1->x*q2->z + q1->y*q2->w - q1->z*q2->x;
			//qprod->z = q1->w*q2->z + q1->x*q2->y - q1->y*q2->x + q1->z*q2->w;

			// this method was arrived at basically by trying to factor the above
			// expression to reduce the # of multiplies

			Fix64 prd_0 = (lhs.z - lhs.y) * (rhs.y - rhs.z);
			Fix64 prd_1 = (lhs.w + lhs.x) * (rhs.w + rhs.x);
			Fix64 prd_2 = (lhs.w - lhs.x) * (rhs.y + rhs.z);
			Fix64 prd_3 = (lhs.y + lhs.z) * (rhs.w - rhs.x);
			Fix64 prd_4 = (lhs.z - lhs.x) * (rhs.x - rhs.y);
			Fix64 prd_5 = (lhs.z + lhs.x) * (rhs.x + rhs.y);
			Fix64 prd_6 = (lhs.w + lhs.y) * (rhs.w - rhs.z);
			Fix64 prd_7 = (lhs.w - lhs.y) * (rhs.w + rhs.z);

			Fix64 prd_8 = prd_5 + prd_6 + prd_7;
			Fix64 prd_9 = (prd_4 + prd_8) * Fix64.Half;

			// and finally build up the result with the temporary products

			return new FQuaternion(prd_1 + prd_9 - prd_8, prd_2 + prd_9 - prd_7, prd_3 + prd_9 - prd_6, prd_0 + prd_9 - prd_5);
		}
		public static bool operator ==(FQuaternion lhs, FQuaternion rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
		}
		public static bool operator !=(FQuaternion lhs, FQuaternion rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z || lhs.w != rhs.w;
		}
	}
}
