﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MagicWorld.DynamicLevelContent.Player;

namespace MagicWorld.Services
{
    interface IPlayerService : IServiceProvider
    {
        Vector2 Position { get; set; }
        bool IsCasting { get; }
        bool IsAlive { get; }

        bool isAiming { get; }

        bool isNearCastingCancel { get; }

        Spell CurrentSpell { get; }

        Mana Mana { get; }
        //int CollectedIngredients { get; }

        Spells.SpellType selectedSpell_A { get; }
        Spells.SpellType selectedSpell_B { get; }
        double SpellAimAngle { get; }
    }
}
