﻿using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using EbonianMod.Projectiles.Dev;
using EbonianMod.Projectiles;

namespace EbonianMod.Buffs
{
    public class Sheepened : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 1; i < BuffID.Count; i++)
            {
                if (!Main.buffNoSave[i] && !Main.buffNoTimeDisplay[i] && !Main.debuff[i])
                    player.ClearBuff(i);
            }
            player.GetModPlayer<EbonianPlayer>().sheep = true;
            if (player.ownedProjectileCounts[ProjectileType<player_sheep>()] < 1)
                Projectile.NewProjectile(null, player.Center, Vector2.Zero, ProjectileType<player_sheep>(), 0, 0, player.whoAmI);
        }
    }
}
