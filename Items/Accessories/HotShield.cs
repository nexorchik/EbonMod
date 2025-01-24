﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace EbonianMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class HotShield : ModItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(8); //speed decrease
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = 2;
            Item.expert = true;
            Item.defense = 8;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed -= 0.08f;
        }
    }
}
