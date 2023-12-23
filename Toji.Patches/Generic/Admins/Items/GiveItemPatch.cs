using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.Inventory;
using GameCore;
using HarmonyLib;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.NwPluginAPI.API.Extensions;
using Toji.Patches.API.Extensions;
using Utils;

namespace Toji.Patches.Generic.Admins.Items
{
    [HarmonyPatch(typeof(GiveCommand), nameof(GiveCommand.Execute))]
    public static class GiveItemPatch
    {
        private static readonly Dictionary<string, int> _usings;

        static GiveItemPatch() => _usings = new();

        private static bool Prefix(GiveCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            __result = false;

            var commandSender = sender as PlayerCommandSender;

            if (commandSender == null)
            {
                response = "brr";

                return false;
            }

            if (!commandSender.CheckPermission(PlayerPermissions.GivingItems, out response))
            {
                return false;
            }

            if (ReferenceHub.LocalHub == commandSender.ReferenceHub)
            {
                return true;
            }

            if (arguments.Count < 2)
            {
                response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + __instance.DisplayCommandUsage();
                return false;
            }

            List<ReferenceHub> targets = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out var array, false);

            if (array == null || array.Length == 0)
            {
                response = "You must specify item(s) to give.";
                return false;
            }

            ItemType[] items = GiveCommand.ParseItems(array[0]).ToArray();

            if (items.Length == 0)
            {
                response = "You didn't input any items.";
                return false;
            }

            var errors = 0;
            var handled = 0;
            var text = string.Empty;

            if (targets != null)
            {
                var player = Player.Get(commandSender);

                if (player.IsDonator(out var tag))
                {
                    if (targets.Count > 1 || targets.Count == 0 || targets.Any(target => target.authManager.UserId != player.UserId))
                    {
                        response = "Вы можете выдавать предметы только себе";
                        return false;
                    }

                    ItemType item = items.First();

                    if (RoundStart.RoundLength.TotalMinutes < 5 && item.IsDangerous() && item != ItemType.GunCOM15)
                    {
                        response = "Атятя, 5 минут ещё не прошли";
                        return false;
                    }

                    if (RoundStart.RoundLength.TotalMinutes < 4 && (item.IsKeycard() && (int)item > 3 || item.IsScp()))
                    {
                        response = "Атятя, 4 минуты ещё не прошли";
                        return false;
                    }

                    if (!_usings.ContainsKey(player.UserId))
                        _usings.Add(player.UserId, 0);

                    var max = tag.GetItemsLimit();

                    var remaining = max - _usings[player.UserId];

                    var hub = targets.First();

                    foreach (ItemType type in items)
                    {
                        if (_usings[player.UserId] >= max || remaining < 0)
                        {
                            response = "Ты уже максимальное кол-во раз использовал донат!";
                            return false;
                        }

                        try
                        {
                            var blocked = type is ItemType.ParticleDisruptor or ItemType.SCP268 or ItemType.MicroHID or ItemType.Jailbird or ItemType.GunCom45 or ItemType.SCP018;

                            if (blocked)
                                continue;

                            GiveCommand.AddItem(hub, sender, item);

                            handled++;

                            _usings[player.UserId]++;

                            remaining--;
                        }
                        catch (Exception ex)
                        {
                            text = ex.Message;

                            errors++;
                        }
                    }

                    response = $"Ты выдал себе {handled} предметов и у тебя {(remaining == 0 ? "не осталось больше использований" : $"ещё {remaining} использований")}! {errors switch
                    {
                        0 => "Ошибок не было!",
                        1 => $"Была одна ошибка! {text}",
                        _ => $"Было {errors} ошибок! Последняя: {text}"
                    }}";

                    __result = true;

                    return false;
                }
                else
                {
                    foreach (ReferenceHub referenceHub in targets)
                    {
                        try
                        {
                            foreach (ItemType item in items)
                            {
                                if (item is ItemType.ParticleDisruptor or ItemType.SCP268 or ItemType.MicroHID or ItemType.Jailbird or ItemType.GunCom45 or ItemType.SCP018)
                                    continue;

                                GiveCommand.AddItem(referenceHub, sender, item);
                            }
                        }
                        catch (Exception ex)
                        {
                            text = ex.Message;

                            errors++;

                            continue;
                        }
                        finally
                        {
                            handled++;
                        }
                    }
                }
            }

            __result = true;

            response = errors == 0 ? string.Format("Done! The request affected {0} player{1}!", handled, handled == 1 ? string.Empty : "s") : string.Format("Failed to execute the command! Failures: {0}\nLast error log:\n{1}", errors, text);
            return false;
        }

        public static void Reset()
        {
            _usings.Clear();
        }
    }
}
