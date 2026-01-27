using System;
using System.Runtime.CompilerServices;

namespace Tobo.Util
{
    // https://stackoverflow.com/questions/11665279/why-enums-hasflag-method-need-boxing
    public static class EnumFlagHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static bool HasFlagUnsafe<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
            switch (sizeof(TEnum))
            {
                case 1:
                    return (*(byte*)(&lhs) & *(byte*)(&rhs)) > 0;
                case 2:
                    return (*(ushort*)(&lhs) & *(ushort*)(&rhs)) > 0;
                case 4:
                    return (*(uint*)(&lhs) & *(uint*)(&rhs)) > 0;
                case 8:
                    return (*(ulong*)(&lhs) & *(ulong*)(&rhs)) > 0;
                default:
                    throw new Exception("Size does not match a known Enum backing type.");
            }
        }

        public unsafe static bool HasFlagByte<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AssertSize<TEnum>(1);
#endif
            return (*(byte*)(&lhs) & *(byte*)(&rhs)) > 0;
        }

        public unsafe static bool HasFlagShort<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AssertSize<TEnum>(2);
#endif
            return (*(ushort*)(&lhs) & *(ushort*)(&rhs)) > 0;
        }

        public unsafe static bool HasFlagInt<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AssertSize<TEnum>(4);
#endif
            return (*(uint*)(&lhs) & *(uint*)(&rhs)) > 0;
        }

        public unsafe static bool HasFlagLong<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AssertSize<TEnum>(8);
#endif
            return (*(ulong*)(&lhs) & *(ulong*)(&rhs)) > 0;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        static unsafe void AssertSize<T>(int expected) where T : unmanaged
        {
            if (expected != sizeof(T))
                throw new InvalidCastException($"Size of enum {typeof(T).Name} is not {expected}!");
        }
#endif
    }
}
