using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Scp914;
using UnityEngine;

namespace Toji.Redux914.Handlers
{
    internal sealed class Scp914Handlers
    {
        public void OnUpgradingPickup(UpgradingPickupEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            if (Random.Range(0, 100) >= 90)
            {
                ev.OutputPosition = Room.Random().Position + Vector3.up * 2;

                return;
            }

            if (Random.Range(0, 100) >= 96)
            {
                ev.IsAllowed = false;
                ev.Pickup.Destroy();

                return;
            }
        }

        public void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            if (Random.Range(0, 100) >= 92)
            {
                ev.IsAllowed = false;

                Pickup.CreateAndSpawn(ev.Item.Type, Room.Random().Position + Vector3.up * 2, default);

                ev.Item.Destroy();

                return;
            }

            if (Random.Range(0, 100) >= 98)
            {
                ev.IsAllowed = false;
                ev.Item.Destroy();

                return;
            }
        }

        public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                return;
            }

            if (Random.Range(0, 100) >= 94)
            {
                ev.IsAllowed = false;
                ev.Player.Position = GetSafePosition(Room.Random(ZoneType.LightContainment)) + Vector3.up * 2;

                return;
            }

            switch (ev.KnobSetting)
            {
                case Scp914.Scp914KnobSetting.Rough:
                    {
                        if (Random.Range(0, 100) >= 90)
                        {
                            ev.Player.Teleport(RoomType.Pocket);
                            ev.Player.EnableEffect(EffectType.Corroding);

                            return;
                        }

                        if (Random.Range(0, 100) >= 90)
                        {
                            ev.Player.EnableEffect(EffectType.AntiScp207);

                            return;
                        }

                        if (Random.Range(0, 100) >= 50)
                        {
                            ev.Player.EnableEffect(EffectType.CardiacArrest, 1, 20);

                            return;
                        }

                        break;
                    }
                case Scp914.Scp914KnobSetting.Coarse:
                    {
                        if (Random.Range(0, 100) >= 50)
                        {
                            ev.Player.EnableEffect(EffectType.Blinded, 1, 30);
                        }

                        if (Random.Range(0, 100) >= 50)
                        {
                            ev.Player.Stamina = 0;
                        }

                        break;
                    }
                case Scp914.Scp914KnobSetting.OneToOne:
                    {
                        break;
                    }
                case Scp914.Scp914KnobSetting.Fine:
                    {
                        if (Random.Range(0, 100) >= 94 && !ev.Player.IsEffectActive<Scp207>())
                        {
                            ev.Player.EnableEffect(EffectType.Scp207, 1, 30);
                            ev.Player.MaxHealth = Mathf.Max(ev.Player.MaxHealth * 0.88f, 40);
                            ev.Player.Health = ev.Player.MaxHealth;

                            return;
                        }

                        if (Random.Range(0, 100) >= 90)
                        {
                            ev.Player.EnableEffect(EffectType.Invigorated, 1, 15);
                            ev.Player.Health = ev.Player.MaxHealth;

                            return;
                        }

                        break;
                    }
                case Scp914.Scp914KnobSetting.VeryFine:
                    {
                        if (Random.Range(0, 100) >= 95 && !ev.Player.IsEffectActive<Scp207>())
                        {
                            ev.Player.EnableEffect(EffectType.Scp207, 1, 100);
                            ev.Player.MaxHealth = Mathf.Max(ev.Player.MaxHealth * 0.9f, 50);
                            ev.Player.Health = ev.Player.MaxHealth;

                            return;
                        }

                        if (Random.Range(0, 100) >= 88)
                        {
                            ev.Player.Health = ev.Player.MaxHealth * 0.06f;

                            return;
                        }

                        break;
                    }
            }
        }

        private Vector3 GetSafePosition(Room room)
        {
            if (room.Type == RoomType.Lcz173 && Random.Range(0, 100) >= 30)
            {
                return room.Doors.GetRandomValue().Position;
            }

            return room.Position;
        }
    }
}
