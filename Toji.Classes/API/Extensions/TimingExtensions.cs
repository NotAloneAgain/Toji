using MEC;
using System;
using System.Linq;

namespace Toji.Classes.API.Extensions
{
    public static class TimingExtensions
    {
        public static object CallDelayedWithResult(this Delegate action, float delay = 0.0003f, params object[] args)
        {
            object result = null!;

            Timing.CallDelayed(delay, delegate()
            {
                if (args != null && args.Any())
                {
                    result = action.DynamicInvoke(args);
                }
                else
                {
                    result = action.DynamicInvoke();
                }
            });

            return result;
        }

        public static TReturn CallDelayedWithResult<TReturn>(this Delegate action, float delay = 0.0003f, params object[] args)
        {
            var result = CallDelayedWithResult(action, delay, args);

            return result == null ? default : (TReturn)result;
        }

        public static void CallDelayed(this Delegate action, float delay = 0.0003f, params object[] args) => CallDelayedWithResult(action, delay, args);

        public static void CallDelayed(this Action action, float delay = 0.0003f) => CallDelayed(action, delay);

        public static void CallDelayed<TArg>(this Action<TArg> action, TArg arg, float delay = 0.0003f) => CallDelayed(action, delay, arg);

        public static void CallDelayed<TReturn>(this Func<TReturn> func, float delay = 0.0003f) => CallDelayed(func, delay);

        public static void CallDelayed<TReturn, T1>(this Func<T1, TReturn> func, T1 arg, float delay = 0.0003f) => CallDelayed(func.Invoke, delay, arg);
    }
}
