using System;
using System.Collections.Generic;
using System.Text;

namespace ToyBox
{
    // TODO: Can this be an extension of Math?
	public static class MathHelper
	{
		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
		{
			return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
		}
		
		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			// Using formula from http://www.mvps.org/directx/articles/catmull/
			// Internally using doubles not to lose precission
			double amountSquared = amount * amount;
			double amountCubed = amountSquared * amount;
			return (float)(0.5 * (2.0 * value2 +
			                      (value3 - value1) * amount +
			                      (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
			                      (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
		}
		
        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else 
                return value;
        }
        
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else 
                return value;
        }
        
        public static float Distance(float value1, float value2)
		{
			return Math.Abs(value1 - value2);
		}
		
		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			// All transformed to double not to lose precission
			// Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
			double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
			double sCubed = s * s * s;
			double sSquared = s * s;
			
			if (amount == 0f)
				result = value1;
			else if (amount == 1f)
				result = value2;
			else
				result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
					(3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
					t1 * s +
					v1;
			return (float)result;
		}
		
		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}
		
		public static float SmoothStep(float value1, float value2, float amount)
		{
			// It is expected that 0 < amount < 1
			// If amount < 0, return value1
			// If amount > 1, return value2
			float result = MathHelper.Clamp(amount, 0f, 1f);
			result = MathHelper.Hermite(value1, 0f, value2, 0f, result);

			return result;
		}
		
		public static float ToDegrees(float radians)
		{
			// This method uses double precission internally,
			// though it returns single float
			// Factor = 180 / pi
			return (float)(radians * 57.295779513082320876798154814105);
		}
		
		public static float ToRadians(float degrees)
		{
			// This method uses double precission internally,
			// though it returns single float
			// Factor = pi / 180
			return (float)(degrees * 0.017453292519943295769236907684886);
		}
		
		public static float WrapAngle(float angle)
		{
			angle = (float)Math.IEEERemainder((double)angle, 6.2831854820251465);
			if (angle <= -3.14159274f)
			{
				angle += 6.28318548f;
			}
			else
			{
				if (angle > 3.14159274f)
				{
					angle -= 6.28318548f;
				}
			}
			return angle;
		}
		
		public static bool IsPowerOfTwo(int value)
		{
			return (value > 0) && ((value & (value - 1)) == 0);
		}
	}
}
