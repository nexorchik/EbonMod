﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EbonianMod.Tiles
{
    internal class EbonHiveRock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.addTile(Type);
            Main.tileMerge[ModContent.TileType<EbonHiveBlock>()][Type] = true;

            DustType = DustID.GreenBlood;
            ItemDrop = ModContent.ItemType<Items.Tiles.EbonHiveI>();

            AddMapEntry(Color.LawnGreen);
        }
    }
    internal class EbonHiveRock2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.addTile(Type);

            Main.tileMerge[ModContent.TileType<EbonHiveBlock>()][Type] = true;
            DustType = DustID.GreenBlood;
            ItemDrop = ModContent.ItemType<Items.Tiles.EbonHiveI>();

            AddMapEntry(Color.LawnGreen);
        }
    }
}
