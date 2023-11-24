using MEC;
using System;

namespace Toji.Classes.API.Extensions
{
    public static class TimingExtensions
    {
        public static object CallDelayedWithResult(this Delegate action, float delay = 0.0005f, params object[] args)
        {
            object result = null!;

            Timing.CallDelayed(delay, delegate()
            {
                result = action.DynamicInvoke(args);
            });

            return result;
        }

        public static TReturn CallDelayedWithResult<TReturn>(this Delegate action, float delay = 0.0005f, params object[] args) => (TReturn)CallDelayedWithResult(action, delay, args);

        public static void CallDelayed(this Delegate action, float delay = 0.0005f, params object[] args) => CallDelayedWithResult(action, delay, args);

        public static void CallDelayed(this Action action, float delay = 0.0005f) => CallDelayed(action, delay);

        public static void CallDelayed<TArg>(this Action<TArg> action, TArg arg, float delay = 0.0005f) => CallDelayed(action, delay, arg);

        public static void CallDelayed<TReturn>(this Func<TReturn> func, float delay = 0.0005f) => CallDelayed(func, delay);

        public static void CallDelayed<TReturn, T1>(this Func<T1, TReturn> func, T1 arg, float delay = 0.0005f) => CallDelayed(func.Invoke, delay, arg);
    }
}
