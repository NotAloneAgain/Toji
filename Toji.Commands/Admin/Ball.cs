﻿using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;
using UnityEngine;

namespace Marine.Commands.Commands
{
    public class Ball : CooldownCommand
    {
        public override string Command { get; set; } = "ball";

        public override string Description { get; set; } = "Команда для спавна мячика под человеком.";

        public override List<CommandType> Types { get; set; } = [ CommandType.RemoteAdmin, CommandType.ServerConsole ];

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 1, "[ИГРОК]" },
        };

        public override CommandPermission Permission { get; set; } = new(true, [

            "owner"
        ], new (0));

        public override int Cooldown { get; set; } = 3;

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            if (base.Handle(arguments, player, out response) == CommandResultType.Fail)
            {
                return CommandResultType.Fail;
            }

            var list = (List<Player>)arguments[0];

            if (!list.Any())
            {
                list.Add(player);
            }

            foreach (Player ply in list)
            {
                var pickup = Pickup.CreateAndSpawn(ItemType.SCP018, ply.Position, ply.Rotation, ply);

                var projectile = pickup as Scp018Projectile;

                Vector3 a2 = player.CameraTransform.forward * (1 - Mathf.Abs(Vector3.Dot(player.CameraTransform.forward, Vector3.up)));

                Rigidbody rigidbody = projectile.GameObject.GetComponent<Rigidbody>();

                if (rigidbody == null)
                {
                    continue;
                }

                rigidbody.velocity = InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.GetLimitedVelocity(pickup.Position) + a2 * projectile.VelocityPerBounce;
            }

            return CommandResultType.Success;
        }

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = [];

            if (count == 1)
            {
                if (!TryParsePlayers(input[0], out List<Player> players))
                {
                    return false;
                }

                output.Add(players);

                return true;
            }

            return false;
        }
    }
}
