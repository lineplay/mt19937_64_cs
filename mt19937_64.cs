using System;
using System.Collections.Generic;
using System.Text;

namespace PFN
{
	class PFN
	{
		static readonly int nx = 312;
		static readonly int mx = 156;
		static ulong[] ax = new ulong[nx*2];
		static int idx = nx;
		
		void _Refill_lower()
		{   // compute values for the lower half of the history array
			int _Ix;
			for (_Ix = 0; _Ix < nx - mx; ++_Ix)
			{   // fill in lower region
				ulong Tmp = (ax[_Ix + nx] & 18446744071562067968UL)
					| (ax[_Ix + nx + 1] & 2147483647UL);

				ax[_Ix] = (Tmp >> 1)
					^ Check(Tmp) ^ ax[_Ix + nx + mx];
			}

			for (; _Ix < nx - 1; ++_Ix)
			{   // fill in upper region (avoids modulus operation)
				ulong Tmp = (ax[_Ix + nx] & 18446744071562067968UL)
					| (ax[_Ix + nx + 1] & 2147483647UL);

				ax[_Ix] = (Tmp >> 1)
					^ Check(Tmp) ^ ax[_Ix - nx + mx];
			}

			ulong _Tmp = (ax[_Ix + nx] & 18446744071562067968UL) | (ax[0] & 2147483647UL);
			ax[_Ix] = (_Tmp >> 1)
				^ Check(_Tmp) ^ ax[mx - 1];
			idx = 0;
		}

		void _Refill_upper()
		{   // compute values for the upper half of the history array
			int _Ix;
			for (_Ix = nx; _Ix < 2 * nx; ++_Ix)
			{   // fill in values
				ulong _Tmp = (ax[_Ix - nx] & 18446744071562067968UL)
					| (ax[_Ix - nx + 1] & 2147483647UL);
					ax[_Ix] = (_Tmp >> 1)
					^ Check(_Tmp) ^ ax[_Ix - nx + mx];
			}
		}

		ulong Check(ulong temp)
		{
			temp = temp & 1;
			if (temp != 0)
				return 0xb5026f5aa96619e9UL;
			else
				return 0;
		}

		public void init(ulong seed)
		{
			ulong _Prev = ax[0] = seed & 18446744073709551615UL;
			for (int _Ix = 1; _Ix < nx; ++_Ix)
					_Prev = ax[_Ix] =
						((ulong)_Ix + 6364136223846793005UL * (_Prev ^ (_Prev >> (64 - 2)))) & 18446744073709551615UL;
		}

		public ulong next()
		{
			if (idx == nx)
				_Refill_upper();
			else if (2 * nx <= idx)
				_Refill_lower();

			ulong _Res = ax[idx++] & 18446744073709551615UL;
			_Res ^= (_Res >> 29) & 6148914691236517205UL;
			_Res ^= (_Res << 17) & 0x71d67fffeda60000UL;
			_Res ^= (_Res << 37) & 0xfff7eee000000000UL;
			_Res ^= (_Res & 18446744073709551615UL) >> 43;
			return (_Res);
		}
	}
}
