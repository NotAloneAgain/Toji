using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Cleanups.API.Enums;

namespace Toji.Cleanups.API.Features
{
    public abstract class BaseCleanup : IDisposable
    {
        private static Dictionary<CleanupType, List<BaseCleanup>> _typeToCleanup;
        private static List<BaseCleanup> _cleanups;

        static BaseCleanup()
        {
            _typeToCleanup = new(8);
            _cleanups = new(8);
        }

        public BaseCleanup()
        {
            if (_typeToCleanup.TryGetValue(Type, out var value))
            {
                value.Add(this);
            }
            else
            {
                _typeToCleanup.Add(Type, new List<BaseCleanup>(4) { this });
            }

            _cleanups.Add(this);
        }

        public static IReadOnlyDictionary<CleanupType, List<BaseCleanup>> TypeToCleanups => _typeToCleanup;

        public static IReadOnlyCollection<BaseCleanup> ReadOnlyCollection => _cleanups.AsReadOnly();

        public static IEnumerable<BaseCleanup> Get(CleanupType type) => TypeToCleanups.TryGetValue(type, out var value) ? value : null;

        public static IEnumerable<TCleanup> Get<TCleanup>(CleanupType type) where TCleanup : BaseCleanup => Get(type).Select(x => x as TCleanup);

        public static BaseCleanup Get(CleanupType type, GameStage stage) => Get(type).FirstOrDefault(x => x.Stage == stage);

        public static TCleanup Get<TCleanup>(CleanupType type, GameStage stage) where TCleanup : BaseCleanup => Get(type, stage) as TCleanup;

        public void Dispose()
        {
            var cleanups = _typeToCleanup[Type];

            if (cleanups.Count > 1)
            {
                cleanups.Remove(this);
            }
            else
            {
                _typeToCleanup.Remove(Type);
            }

            _cleanups.Remove(this);
        }

        public abstract CleanupType Type { get; }

        public abstract GameStage Stage { get; }
    }
}
