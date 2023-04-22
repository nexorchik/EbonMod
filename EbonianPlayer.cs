﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using System;
using static Terraria.ModLoader.PlayerDrawLayer;
using EbonianMod.Items.Accessories;
using Terraria.Graphics.Effects;

namespace EbonianMod
{
    public class EbonianPlayer : ModPlayer
    {
        public int bossTextProgress, bossMaxProgress, dialogueMax, dialogueProg;
        public string bossName;
        public string bossTitle;
        public string dialogue;
        public int bossStyle;
        public Color bossColor, dialogueColor;
        public static EbonianPlayer Instance;
        public bool rolleg, brainAcc, heartAcc, ToxicGland, doomMinion;
        public override void ResetEffects()
        {
            rolleg = false;
            doomMinion = false;
            brainAcc = false;
            heartAcc = false;
            ToxicGland = false;
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<TinyBrain>()))
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.type == ModContent.NPCType<TinyBrain>())
                    {
                        npc.life = 0;
                        npc.checkDead();
                    }
                }
            }
            return true;
        }
        public override void PostUpdateMiscEffects()
        {
            EbonianMod.sys.UpdateParticles();
            //Player.ManageSpecialBiomeVisuals("EbonianMod:CorruptTint", Player.ZoneCorrupt && !Player.ZoneUnderworldHeight);
            //Player.ManageSpecialBiomeVisuals("EbonianMod:CrimsonTint", Player.ZoneCrimson && !Player.ZoneUnderworldHeight);
            #region "hell stuff"
            Player.ManageSpecialBiomeVisuals("EbonianMod:HellTint", Player.ZoneUnderworldHeight);
            if (Player.ZoneUnderworldHeight && Main.BackgroundEnabled)
            {
                if (Main.rand.NextBool(13))
                {
                    EbonianMod.sys.CreateParticle((part) =>
                    {
                        if (part.ai[0] > 200)
                        {
                            part.dead = true;
                        }
                        part.rotation = part.velocity.ToRotation();
                        part.ai[0]++;
                        part.scale = (float)Math.Sin(part.ai[0] * Math.PI / 200) * part.ai[1];
                        part.alpha = (float)Math.Sin(part.ai[0] * Math.PI / 200);
                    },
                    new[]
                    {
                        Helper.GetExtraTexture("cinder")
                    }, (part, spriteBatch, position) =>
                    {
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                        spriteBatch.Draw(part.textures[0], part.position - Main.screenPosition, null, part.color, part.rotation, part.textures[0].Size() / 2, part.scale, SpriteEffects.None, 0);
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
                    }
                    , new(Main.windSpeedCurrent + Main.rand.NextFloat(-2, 2f), Main.rand.NextFloat(-5, -10)), part =>
                    {
                        part.color = Color.White;
                        part.scale = Main.rand.NextFloat(0.05f, 0.15f);
                        part.ai[1] = Main.rand.NextFloat(0.1f, 0.2f);
                        part.rotation = Main.rand.NextFloat(-1, 1);
                        part.position = new Vector2(Main.screenPosition.X + Main.rand.NextFloat(Main.screenWidth), Main.screenPosition.Y + Main.screenHeight + 100);
                    });
                }
            } // saved for the hell update
            #endregion 
            //dont delete hell stuff...
        }
        public override void OnEnterWorld(Player player)
        {
            Instance = Player.GetModPlayer<EbonianPlayer>();
        }
        public int flashTime;
        public int flashMaxTime;
        public Vector2 flashPosition;
        public void FlashScreen(Vector2 pos, int time)
        {
            flashMaxTime = time;
            flashTime = time;
            flashPosition = pos;
        }
        public override void PostUpdate()
        {
            if (flashTime > 0)
            {
                flashTime--;
                if (!Filters.Scene["EbonianMod:ScreenFlash"].IsActive())
                    Filters.Scene.Activate("EbonianMod:ScreenFlash", flashPosition);
                Filters.Scene["EbonianMod:ScreenFlash"].GetShader().UseProgress((float)Math.Sin((float)flashTime / flashMaxTime * Math.PI) * 2f);
                Filters.Scene["EbonianMod:ScreenFlash"].GetShader().UseTargetPosition(flashPosition); // already added it to luminary but gotta test alr a
            }
            else
            {
                if (Filters.Scene["EbonianMod:ScreenFlash"].IsActive())
                    Filters.Scene["EbonianMod:ScreenFlash"].Deactivate();
            }
            if (bossTextProgress > 0)
                bossTextProgress--;
            if (bossTextProgress == 0)
            {
                bossName = null;
                bossTitle = null;
                bossStyle = -1;
                bossMaxProgress = 0;
                bossColor = Color.White;
            }
            if (dialogueProg > 0)
                dialogueProg--;
            if (dialogueProg == 0)
            {
                dialogue = null;
                dialogueMax = 0;
                dialogueColor = Color.White;
            }
        }
    }
}
