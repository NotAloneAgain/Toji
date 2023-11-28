using Exiled.API.Features;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Toji.Classes.API.Features
{
    public abstract class BaseCharacteristic
    {
        private List<Player> _owners;

        public BaseCharacteristic()
        {
            _owners = new List<Player>(Server.MaxPlayerCount);
        }

        public ReadOnlyCollection<Player> Owners => _owners.AsReadOnly();

        public abstract string Name { get; }

        public string GetDesc(Player player = null) => player == null ? GetDefaultDescription() : GetAdvancedDescription(player);

        public virtual void OnEnabled(Player player) => _owners.Add(player);

        public virtual void OnDisabled(Player player) => _owners.Remove(player);

        protected abstract string GetDefaultDescription();

        protected abstract string GetAdvancedDescription(Player player);
    }
}
