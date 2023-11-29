﻿using Exiled.API.Features;
using Toji.Classes.API.Features;
using UnityEngine;

namespace Toji.Classes.Characteristics
{
    public class RespawnCharacteristics : Characteristic<bool>
    {
        public RespawnCharacteristics(bool value) : base(value) { }

        public override string Name => "Сохранение подкласса";

        public override string GetDesc(Player player = null) => Value ? "Ваш подкласс остается с вами даже после смерти и возрождения" : "Вы не будете сохранять подкласс после смерти (возможно, нерф).";
    }
}
