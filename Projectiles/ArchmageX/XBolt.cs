﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using EbonianMod.Dusts;

namespace EbonianMod.Projectiles.ArchmageX
{
    public class XBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            EbonianMod.projectileFinalDrawList.Add(Type);
            Main.projFrames[Type] = 5;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(60, 26);
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 0;
            Projectile.extraUpdates = 5;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 500 * Projectile.extraUpdates;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) => hitbox.Width = 26;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<XExplosionTiny>(), 0, 0);
            if (Projectile.ai[1] == 0)
            {
                Projectile.ai[1] = 0.01f;
            }
            return false;
        }
        public override bool? CanDamage() => Projectile.ai[0] >= 25;
        public override void AI()
        {
            if (++Projectile.frameCounter % 25 == 0)
                if (++Projectile.frame > 4)
                    Projectile.frame = 0;
            Projectile.ai[0] += 0.2f;
            if (Projectile.ai[0] < 25)
            {
                if (Projectile.velocity.Length() > 0.1f)
                    Projectile.velocity *= 0.92f;
            }
            else
            {
                if (Projectile.velocity.Length() < 3)
                    Projectile.velocity *= 1.025f;
            }
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1] += 0.01f;
                if (Projectile.ai[1] >= 1)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (Projectile.velocity.Length() > 1.5f && Projectile.ai[0] > 25)
                {
                    Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * Projectile.width / 2, ModContent.DustType<XGoopDust>(), Vector2.Zero, Scale: 0.7f * MathHelper.Lerp(0, 1, Projectile.velocity.Length() / 3));

                    if (Main.rand.NextBool(3))
                    {
                        Vector2 pos = Projectile.Center + new Vector2((Projectile.width / 4) * Main.rand.NextFloat(-1, 1f), 10).RotatedBy(Projectile.rotation + MathHelper.PiOver2);
                        Dust D = Dust.NewDustPerfect(pos, ModContent.DustType<LineDustFollowPoint>(), Helper.FromAToB(Projectile.Center, pos) * 0.1f, 0, Color.DarkOrchid, Main.rand.NextFloat(0.05f, 0.175f));
                        D.customData = Projectile.oldPos[Projectile.oldPos.Length - 1] + Projectile.Size / 2;
                    }
                }
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            Projectile.ai[2] = MathHelper.Lerp(1, 0, Projectile.ai[1]);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D glow = Helper.GetTexture(Texture + "_Glow");
            Main.spriteBatch.Reload(BlendState.Additive);
            var fadeMult = 1f / Projectile.oldPos.Length;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float mult = (1f - fadeMult * i);
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, new Rectangle(0, Projectile.frame * 28, 60, 28), Color.White * (0.25f * mult * Projectile.ai[2]), Projectile.rotation, Projectile.Size / 2, Projectile.scale * mult, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.White * (0.5f * Projectile.ai[2]), Projectile.rotation, glow.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 28, 60, 28), Color.White * (0.5f * Projectile.ai[2]), Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Reload(BlendState.AlphaBlend);
            return false;
        }
    }
}
