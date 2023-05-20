﻿using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using EbonianMod.Projectiles.Friendly.Corruption;
using Terraria.DataStructures;
using EbonianMod.Projectiles.Exol;
using EbonianMod.Projectiles.VFXProjectiles;
using Terraria.Audio;
using EbonianMod.Projectiles.Friendly.Crimson;
using EbonianMod.Misc;
using System.IO;
using System.Collections.Generic;
using EbonianMod.NPCs.Corruption;

namespace EbonianMod.Projectiles.Friendly.Crimson
{
    public class LatcherP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.ai[0] == 0)
            {
                Projectile.timeLeft = 200;
                Projectile.ai[1] = 1;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = target.whoAmI;
                Projectile.timeLeft = 200;
                Projectile.ai[1] = 2;
            }
        }
        Verlet verlet;
        public override void OnSpawn(IEntitySource source)
        {
            verlet = new(Projectile.Center, 20, 10, 1, true, true, 10);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner && Main.mouseRight)
                Projectile.Kill();
            if (Projectile.ai[1] == 1)
            {
                player.velocity = Helper.FromAToB(player.Center, Projectile.Center) * 20;
                if (player.Center.Distance(Projectile.Center) < 50)
                    Projectile.Kill();
            }
            else if (Projectile.ai[1] == 2)
            {
                NPC npc = Main.npc[(int)Projectile.ai[0]];
                if (npc.active && npc.life > 0 && player.Center.Distance(npc.Center) > npc.width)
                {
                    Projectile.Center = npc.Center;
                    if (npc.knockBackResist == 0f)
                        player.velocity = Helper.FromAToB(player.Center, Projectile.Center) * 10;
                    else
                        npc.velocity = Helper.FromAToB(npc.Center, player.Center) * 20;
                }
                else
                    Projectile.Kill();
            }
            else
            {
                if (Projectile.timeLeft < 100)
                    Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center, 0.1f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (verlet != null)
            {
                verlet.Update(Projectile.Center, Main.player[Projectile.owner].Center);
                verlet.Draw(Main.spriteBatch, "Projectiles/Friendly/Crimson/LatcherP_Chain");
            }
            return true;
        }
    }
    public class LatcherPCecitior : ModProjectile
    {
        public override string Texture => "EbonianMod/Projectiles/Friendly/Crimson/LatcherP";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hide = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.timeLeft = 200;
                Projectile.ai[1] = 1;
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.velocity = Vector2.Zero;
            if (Projectile.localAI[0] == 0 && Projectile.ai[1] == 0)
            {
                Projectile.localAI[0] = target.whoAmI;
                Projectile.timeLeft = 100;
                Projectile.ai[1] = 2;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }
        public override void AI()
        {
            if (Projectile.ai[1] == 0)
                Projectile.rotation = Projectile.velocity.ToRotation();
            NPC player = Main.npc[(int)Projectile.ai[0]];
            if (player.ai[0] != 8)
                Projectile.Kill();
            if (Projectile.ai[1] == 1)
            {
                player.velocity = Helper.FromAToB(player.Center, Projectile.Center) * 25;
                if (player.Center.Distance(Projectile.Center) < 50)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<FatSmash>(), 0, 0, 0, 0);
                    player.velocity = Projectile.rotation.ToRotationVector2() * -10f;
                    Projectile.Kill();
                    SoundEngine.PlaySound(new SoundStyle("EbonianMod/Sounds/chomp" + Main.rand.Next(2)), Projectile.Center);
                }
            }
            else if (Projectile.ai[1] == 2)
            {
                Player playerr = Main.player[(int)Projectile.localAI[0]];
                playerr.velocity = Helper.FromAToB(playerr.Center, player.Center, false) / 10;
                Projectile.velocity = Helper.FromAToB(Projectile.Center, player.Center) * 20;

            }
            else
            {
                if (Projectile.velocity.Length() < 24)
                    Projectile.velocity *= 1.15f;
                if (Projectile.timeLeft < 100)
                {
                    Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center, 0.4f);

                    if (player.Center.Distance(Projectile.Center) < 50)
                        Projectile.Kill();
                }
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            NPC player = Main.npc[(int)Projectile.ai[0]];
            Vector2 neckOrigin = Projectile.Center;
            Vector2 center = player.Center;
            Vector2 distToProj = neckOrigin - player.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            while (distance > 20 && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= 20;
                center += distToProj;
                distToProj = neckOrigin - center;
                distance = distToProj.Length();

                //Draw chain
                Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Projectiles/Friendly/Crimson/LatcherP_Chain").Value, center - Main.screenPosition,
                    null, Lighting.GetColor((int)center.X / 16, (int)center.Y / 16), projRotation,
                    Mod.Assets.Request<Texture2D>("Projectiles/Friendly/Crimson/LatcherP_Chain").Value.Size() / 2, 1f, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
