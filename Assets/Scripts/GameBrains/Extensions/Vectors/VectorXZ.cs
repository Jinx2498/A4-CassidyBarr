using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace GameBrains.Extensions.Vectors
{
  // Representation of 2D XZ vectors and points.
  [System.Serializable]
    public struct VectorXZ : System.IEquatable<VectorXZ>, System.IFormattable
    {
        public const float kEpsilon = 1E-05f;
        public const float kEpsilonNormalSqrt = 1E-15f;

        // X component of the VectorXZ.
        public float x;

        // Z component of the VectorXZ.
        public float z;
        
        // Constructs a new VectorXZ with given x, z components.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VectorXZ(float x, float z)
        {
            this.x = x;
            this.z = z;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return z;
                    default:
                        throw new System.IndexOutOfRangeException("Invalid VectorXZ index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        z = value;
                        break;
                    default:
                        throw new System.IndexOutOfRangeException("Invalid VectorXZ index!");
                }
            }
        }

        // >Returns this VectorXZ with a magnitude of 1 (Read Only).
        public VectorXZ normalized
        {
            get
            {
                var vectorXZ = new VectorXZ(x, z);
                vectorXZ.Normalize();
                return vectorXZ;
            }
        }

        // Returns the length of this VectorXZ (Read Only).
        public float magnitude => (float) System.Math.Sqrt(x * (double) x + z * (double) z);

        // Returns the squared length of this VectorXZ (Read Only).
        public float sqrMagnitude => (float) (x * (double) x + z * (double) z);

        // Shorthand for writing VectorXZ(0, 0). 
        public static VectorXZ zero { get; } = new VectorXZ(0.0f, 0.0f);
 
        // Shorthand for writing XZ VectorXZ(1, 1).
        public static VectorXZ one { get; } = new VectorXZ(1f, 1f);

        // Shorthand for writing VectorXZ(0, 1).
        public static VectorXZ forward { get; } = new VectorXZ(0.0f, 1f);

        // Shorthand for writing VectorXZ(0, -1).
        public static VectorXZ back { get; } = new VectorXZ(0.0f, -1f);

        // Shorthand for writing VectorXZ(-1, 0).
        public static VectorXZ left { get; } = new VectorXZ(-1f, 0.0f);

        // Shorthand for writing VectorXZ(1, 0).
        public static VectorXZ right { get; } = new VectorXZ(1f, 0.0f);

        // Shorthand for writing VectorXZ(float.PositiveInfinity, float.PositiveInfinity).
        public static VectorXZ positiveInfinity { get; } =
            new VectorXZ(float.PositiveInfinity, float.PositiveInfinity);

        // Shorthand for writing VectorXZ(float.NegativeInfinity, float.NegativeInfinity).
        public static VectorXZ negativeInfinity { get; } =
            new VectorXZ(float.NegativeInfinity, float.NegativeInfinity);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(VectorXZ other)
        {
            return x == (double) other.x && z == (double) other.z;
        }

        // Returns a formatted string for this VectorXZ.
        public string ToString(string format, System.IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "F1";
            //return UnityString.Format("({0}, {1})", (object) this.x.ToString(format, formatProvider), (object) this.z.ToString(format, formatProvider));
            return string.Format("({0}, {1})", x.ToString(format, formatProvider),
                z.ToString(format, formatProvider));
        }

        // Set x and z components of an existing VectorXZ.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newX, float newZ)
        {
            x = newX;
            z = newZ;
        }

        // Linearly interpolates between VectorXZ a and b by t.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ Lerp(VectorXZ a, VectorXZ b, float t)
        {
            t = Mathf.Clamp01(t);
            return new VectorXZ(a.x + (b.x - a.x) * t, a.z + (b.z - a.z) * t);
        }

        // Linearly interpolates between VectorXZ a and b by t.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ LerpUnclamped(VectorXZ a, VectorXZ b, float t)
        {
            return new VectorXZ(a.x + (b.x - a.x) * t, a.z + (b.z - a.z) * t);
        }

        // >Moves a point current towards target.
        public static VectorXZ MoveTowards(
            VectorXZ current,
            VectorXZ target,
            float maxDistanceDelta)
        {
            var num1 = target.x - current.x;
            var num2 = target.z - current.z;
            var num3 = (float) (num1 * (double) num1 + num2 * (double) num2);
            if (num3 == 0.0 ||
                maxDistanceDelta >= 0.0 && num3 <= maxDistanceDelta * (double) maxDistanceDelta)
                return target;
            var num4 = (float) System.Math.Sqrt(num3);
            return new VectorXZ(current.x + num1 / num4 * maxDistanceDelta,
                current.z + num2 / num4 * maxDistanceDelta);
        }

        // Multiplies two VectorXZ component-wise.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ Scale(VectorXZ a, VectorXZ b)
        {
            return new VectorXZ(a.x * b.x, a.z * b.z);
        }

        // Multiplies every component of this VectorXZ by the same component of scale.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(VectorXZ scale)
        {
            x *= scale.x;
            z *= scale.z;
        }

        // Makes this VectorXZ have a magnitude of 1.
        public void Normalize()
        {
            var magnitude = this.magnitude;
            if (magnitude > 9.999999747378752E-06)
                this = this / magnitude;
            else
                this = zero;
        }

        // Returns a formatted string for this VectorXZ.
        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        // Returns a formatted string for this VectorXZ.
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public override int GetHashCode() { return x.GetHashCode() ^ (z.GetHashCode() << 2); }

        // Returns true if the given VectorXZ is exactly equal to this VectorXZ.
        public override bool Equals(object other)
        {
            return other is Vector2 other1 && Equals(other1);
        }

        // Reflects a VectorXZ off the VectorXZ defined by a normal.
        public static VectorXZ Reflect(VectorXZ inDirection, VectorXZ inNormal)
        {
            var num = -2f * Dot(inNormal, inDirection);
            return new VectorXZ(num * inNormal.x + inDirection.x, num * inNormal.z + inDirection.z);
        }
        
        // Returns the 2D VectorXZ perpendicular to this 2D VectorXZ. The result is always rotated
        // 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Z
        // axis goes forward.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ Perpendicular(VectorXZ inDirection)
        {
            return new VectorXZ(-inDirection.z, inDirection.x);
        }

        // Dot Product of two VectorXZ.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(VectorXZ lhs, VectorXZ rhs)
        {
            return (float) (lhs.x * (double) rhs.x + lhs.z * (double) rhs.z);
        }

        // Returns the unsigned angle in degrees between from and to.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(VectorXZ from, VectorXZ to)
        {
            var num = (float) System.Math.Sqrt(from.sqrMagnitude * (double) to.sqrMagnitude);
            return num < 1.0000000036274937E-15
                ? 0.0f
                : (float) System.Math.Acos(Mathf.Clamp(Dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }

        // Returns the signed angle in degrees between from and to.
        public static float SignedAngle(VectorXZ from, VectorXZ to)
        {
            return Angle(from, to) *
                   Mathf.Sign((float) (from.x * (double) to.z - from.z * (double) to.x));
        }

        // Returns the distance between a and b.
        public static float Distance(VectorXZ a, VectorXZ b)
        {
            var num1 = a.x - b.x;
            var num2 = a.z - b.z;
            return (float) System.Math.Sqrt(num1 * (double) num1 + num2 * (double) num2);
        }

        // Returns a copy of VectorXZ with its magnitude clamped to maxLength.
        public static VectorXZ ClampMagnitude(VectorXZ vectorXZ, float maxLength)
        {
            var sqrMagnitude = vectorXZ.sqrMagnitude;
            if (sqrMagnitude <= maxLength * (double) maxLength)
                return vectorXZ;
            var num1 = (float) System.Math.Sqrt(sqrMagnitude);
            var num2 = vectorXZ.x / num1;
            var num3 = vectorXZ.z / num1;
            return new VectorXZ(num2 * maxLength, num3 * maxLength);
        }

        public static float SqrMagnitude(VectorXZ a)
        {
            return (float) (a.x * (double) a.x + a.z * (double) a.z);
        }

        public float SqrMagnitude() { return (float) (x * (double) x + z * (double) z); }

        // Returns a VectorXZ that is made from the smallest components of two VectorXZ.
        public static VectorXZ Min(VectorXZ lhs, VectorXZ rhs)
        {
            return new VectorXZ(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.z, rhs.z));
        }

        // Returns a VectorXZ that is made from the largest components of two VectorXZ.
        public static VectorXZ Max(VectorXZ lhs, VectorXZ rhs)
        {
            return new VectorXZ(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.z, rhs.z));
        }

        [ExcludeFromDocs]
        public static VectorXZ SmoothDamp(
            VectorXZ current,
            VectorXZ target,
            ref VectorXZ currentVelocity,
            float smoothTime,
            float maxSpeed)
        {
            var deltaTime = Time.deltaTime;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed,
                deltaTime);
        }

        [ExcludeFromDocs]
        public static VectorXZ SmoothDamp(
            VectorXZ current,
            VectorXZ target,
            ref VectorXZ currentVelocity,
            float smoothTime)
        {
            var deltaTime = Time.deltaTime;
            var maxSpeed = float.PositiveInfinity;
            return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed,
                deltaTime);
        }

        public static VectorXZ SmoothDamp(
            VectorXZ current,
            VectorXZ target,
            ref VectorXZ currentVelocity,
            float smoothTime,
            [DefaultValue("Mathf.Infinity")] float maxSpeed,
            [DefaultValue("Time.deltaTime")] float deltaTime)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            var num1 = 2f / smoothTime;
            var num2 = num1 * deltaTime;
            var num3 = (float) (1.0 /
                                (1.0 +
                                 num2 +
                                 0.47999998927116394 * num2 * num2 +
                                 0.23499999940395355 * num2 * num2 * num2));
            var num4 = current.x - target.x;
            var num5 = current.z - target.z;
            VectorXZ vectorXZ = target;
            var num6 = maxSpeed * smoothTime;
            var num7 = num6 * num6;
            var num8 = (float) (num4 * (double) num4 + num5 * (double) num5);
            if (num8 > (double) num7)
            {
                var num9 = (float) System.Math.Sqrt(num8);
                num4 = num4 / num9 * num6;
                num5 = num5 / num9 * num6;
            }

            target.x = current.x - num4;
            target.z = current.z - num5;
            var num10 = (currentVelocity.x + num1 * num4) * deltaTime;
            var num11 = (currentVelocity.z + num1 * num5) * deltaTime;
            currentVelocity.x = (currentVelocity.x - num1 * num10) * num3;
            currentVelocity.z = (currentVelocity.z - num1 * num11) * num3;
            var x = target.x + (num4 + num10) * num3;
            var z = target.z + (num5 + num11) * num3;
            var num12 = vectorXZ.x - current.x;
            var num13 = vectorXZ.z - current.z;
            var num14 = x - vectorXZ.x;
            var num15 = z - vectorXZ.z;
            if (num12 * (double) num14 + num13 * (double) num15 > 0.0)
            {
                x = vectorXZ.x;
                z = vectorXZ.z;
                currentVelocity.x = (x - vectorXZ.x) / deltaTime;
                currentVelocity.z = (z - vectorXZ.z) / deltaTime;
            }

            return new VectorXZ(x, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator +(VectorXZ a, VectorXZ b)
        {
            return new VectorXZ(a.x + b.x, a.z + b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator -(VectorXZ a, VectorXZ b)
        {
            return new VectorXZ(a.x - b.x, a.z - b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator *(VectorXZ a, VectorXZ b)
        {
            return new VectorXZ(a.x * b.x, a.z * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator /(VectorXZ a, VectorXZ b)
        {
            return new VectorXZ(a.x / b.x, a.z / b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator -(VectorXZ a) { return new VectorXZ(-a.x, -a.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator *(VectorXZ a, float d)
        {
            return new VectorXZ(a.x * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator *(float d, VectorXZ a)
        {
            return new VectorXZ(a.x * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorXZ operator /(VectorXZ a, float d)
        {
            return new VectorXZ(a.x / d, a.z / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(VectorXZ lhs, VectorXZ rhs)
        {
            var num1 = lhs.x - rhs.x;
            var num2 = lhs.z - rhs.z;
            return num1 * (double) num1 + num2 * (double) num2 < 9.999999439624929E-11;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(VectorXZ lhs, VectorXZ rhs) { return !(lhs == rhs); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator VectorXZ(Vector3 v) { return new VectorXZ(v.x, v.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator VectorXZ(Vector2 v) { return new VectorXZ(v.x, v.y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator VectorXZ(VectorXYZ p) { return new VectorXZ(p.x, p.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3(VectorXZ l) { return new Vector3(l.x, 0.0f, l.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2(VectorXZ l) { return new Vector2(l.x, l.z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator VectorXYZ(VectorXZ l)
        {
            return new VectorXYZ(l.x, 0.0f, l.z);
        }
    }
}