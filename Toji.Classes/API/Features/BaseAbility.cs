using Exiled.API.Features;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class BaseAbility
    {
        private List<Player> _owners;

        public BaseAbility()
        {
            _owners = new List<Player>(Server.MaxPlayerCount);
        }

        public ReadOnlyCollection<Player> Owners => _owners.AsReadOnly();

        public abstract string Name { get; }

        public abstract string Desc { get; }

        public virtual void OnEnabled(Player player)
        {
            _owners.Add(player);

            if (this is ISubscribable sub)
            {
                sub.Subscribe();
            }
        }

        public virtual void OnDisabled(Player player)
        {
            if (this is ISubscribable sub)
            {
                sub.Unsubscribe();
            }

            _owners.Remove(player);
        }
    }
}
