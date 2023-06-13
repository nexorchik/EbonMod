﻿using EbonianMod.Tiles.Paintings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace EbonianMod.Items.Tiles
{
    public class EbonPaintingI : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = 0;
            Item.useTurn = true;
            Item.rare = 0;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Purple;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<EbonPainting>();
        }
    }
}
