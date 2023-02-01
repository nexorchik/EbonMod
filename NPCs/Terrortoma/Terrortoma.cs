using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using ExolRebirth.Projectiles.Terrortoma;
using ExolRebirth.Projectiles;

namespace ExolRebirth.NPCs.Terrortoma
{
    [AutoloadBossHead]
    public class Terrortoma : ModNPC
    {
        public override void BossHeadRotation(ref float rotation)
        {
            rotation = NPC.rotation;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("The rotten corpse of many soul eaters, melded together through the Corruption's natural processes, creating an extremely vile and murderous monster."),
            });
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terrortoma");
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            Main.npcFrameCount[NPC.type] = 11;
            NPCID.Sets.TrailCacheLength[NPC.type] = 4;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "ExolRebirth/NPCs/Terrortoma/Terrortoma_Bosschecklist",
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f,
            };
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 4500;
            NPC.damage = 40;
            NPC.noTileCollide = true;
            NPC.defense = 20;
            NPC.knockBackResist = 0;
            NPC.width = 118;
            NPC.height = 120;
            NPC.rarity = 999;
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.boss = false;
            SoundStyle hit = new("ExolRebirth/Sounds/NPCHit/TerrorHit");
            SoundStyle death = new("ExolRebirth/Sounds/NPCHit/TerrorDeath");
            NPC.HitSound = hit;
            NPC.DeathSound = death;
            NPC.buffImmune[24] = true;
            NPC.netAlways = true;
            NPC.alpha = 255;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 pos, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>("ExolRebirth/NPCs/Terrortoma/Terrortoma").Value.Width * 0.5f, NPC.height * 0.5f);
            if (AIState == Dash || AIState == Spew)
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = NPC.oldPos[k] - pos + drawOrigin + new Vector2(0, NPC.gfxOffY);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("ExolRebirth/NPCs/Terrortoma/Terrortoma").Value, drawPos, NPC.frame, lightColor * 0.5f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
                }
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("ExolRebirth/NPCs/Terrortoma/Terrortoma").Value, NPC.Center - pos, NPC.frame, lightColor, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        //npc.rotation = npc.velocity.ToRotation() - MathHelper.PiOver2;
        //Vector2 toPlayer = player.Center - npc.Center;
        //npc.rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
        public override void FindFrame(int frameHeight)
        {
            if (!isLaughing && (AIState == HoverAndLetTheClingersDoTheJob || AIState == VilespitSpamBecauseYes || AIState == CursedFlamesRain || AIState == ClingerSpecial || AIState == HoverAndLetTheClingersDoTheJob2 || AIState == CursedFlamesRain2 || AIState == ClingerSpecial2))
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 0 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 2 * frameHeight;
                }
                else if (NPC.frameCounter < 20)
                {
                    NPC.frame.Y = 3 * frameHeight;
                }
                else if (NPC.frameCounter < 25)
                {
                    NPC.frame.Y = 4 * frameHeight;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = 5 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (!isLaughing && (AIState == Spew || AIState == ApeShitMode || /*AIState == Vilethorn ||*/ AIState == Dash) || AIState == Death)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 6 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 7 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
            else if (isLaughing || NPC.IsABestiaryIconDummy)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter < 5)
                {
                    NPC.frame.Y = 8 * frameHeight;
                }
                else if (NPC.frameCounter < 10)
                {
                    NPC.frame.Y = 9 * frameHeight;
                }
                else if (NPC.frameCounter < 15)
                {
                    NPC.frame.Y = 10 * frameHeight;
                }
                else
                {
                    NPC.frameCounter = 0;
                }
            }
        }
        private bool isLaughing;
        private bool HasSummonedClingers = false;
        private const int AISlot = 0;
        private const int TimerSlot = 1;

        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }

        private float angle = 0;
        public float AITimer
        {
            get => NPC.ai[TimerSlot];
            set => NPC.ai[TimerSlot] = value;
        }
        private float AITimer2 = 0;
        private bool hasDonePhase2ApeShitMode = false;

        public const int ApeShitMode = 999;
        public const int Death = -1;
        public const int Intro = 0;
        private const int Spew = 1;
        private const int Dash = 2;
        private const int ClingerSpecial = 3;
        private const int HoverAndLetTheClingersDoTheJob = 4;
        //private const int Vilethorn = 5;
        private const int VilespitSpamBecauseYes = 6;
        private const int CursedFlamesRain = 7;
        //private const int Dash2 = 8;
        private const int ClingerSpecial2 = 9;
        private const int HoverAndLetTheClingersDoTheJob2 = 10;
        //private const int Vilethorn2 = 11;
        //private const int VilespitSpamBecauseYes2 = 12;
        private const int CursedFlamesRain2 = 13;
        private const int BigBoyLaser = 14;
        float rotation;
        bool ded;
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                AIState = Death;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.dontTakeDamage = true;
                EbonianSystem.ChangeCameraPos(NPC.Center, 250);
                EbonianSystem.ScreenShakeAmount = 20;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(rotation);
            writer.Write(ded);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            rotation = reader.ReadByte();
            ded = reader.ReadBoolean();
        }
        public override void AI()
        {
            NPC.rotation = MathHelper.Lerp(NPC.rotation, rotation, 0.35f);
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (NPC.HasValidTarget)
                {
                    AIState = Intro;
                    AITimer = 0;
                }
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            if (NPC.life <= NPC.lifeMax / 2 && !hasDonePhase2ApeShitMode)
            {
                AIState = ApeShitMode;
                hasDonePhase2ApeShitMode = true;
                AITimer = 0;
            }
            if (NPC.alpha >= 255)
            {
                NPC.Center = new Vector2(player.Center.X, player.Center.Y - 230);
            }
            if (NPC.alpha <= 0 && AIState != Death)
            {
                if (!HasSummonedClingers)
                {
                    NPC fune = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerMelee>())];
                    NPC fune2 = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerSummoner>())];
                    NPC fune3 = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerRanged>())];
                    fune.ai[0] = NPC.whoAmI;
                    fune2.ai[0] = NPC.whoAmI;
                    fune3.ai[0] = NPC.whoAmI;
                    HasSummonedClingers = true;
                }
                if (!NPC.AnyNPCs(ModContent.NPCType<TerrorClingerMelee>()))
                {
                    NPC fune = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerMelee>())];
                    fune.ai[0] = NPC.whoAmI;
                }
                if (!NPC.AnyNPCs(ModContent.NPCType<TerrorClingerSummoner>()))
                {
                    NPC fune2 = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerSummoner>())];
                    fune2.ai[0] = NPC.whoAmI;
                }
                if (!NPC.AnyNPCs(ModContent.NPCType<TerrorClingerRanged>()))
                {
                    NPC fune3 = Main.npc[NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TerrorClingerRanged>())];
                    fune3.ai[0] = NPC.whoAmI;
                }
            }
            if (AIState == Death)
            {
                rotation = 0;
                NPC.damage = 0;

                AITimer++;
                if (AITimer == 250)
                    NPC.velocity = Vector2.UnitY;

                if (AITimer >= 250)
                {
                    NPC.velocity *= 1.025f;
                    rotation += MathHelper.ToRadians(3 * NPC.velocity.Y);
                    if (Helper.TRay.CastLength(NPC.Center, Vector2.UnitY, Main.screenWidth) < NPC.width / 2)
                    {
                        Projectile.NewProjectileDirect(NPC.InheritSource(NPC), Helper.TRay.Cast(NPC.Center, Vector2.UnitY, Main.screenWidth), Vector2.Zero, ModContent.ProjectileType<TExplosion>(), 0, 0).scale = 2f;
                        SoundEngine.PlaySound(SoundID.Item62);
                        NPC.immortal = false;
                        NPC.life = 0;
                        NPC.checkDead();
                    }
                }
            }
            else if (AIState == ApeShitMode)
            {
                NPC.velocity = Vector2.Zero;
                isLaughing = false;
                if (AITimer == 1)
                {
                    EbonianSystem.ScreenShakeAmount = 30f;
                }
                Vector2 toPlayer = player.Center - NPC.Center;
                rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                if (++AITimer >= 60)
                {
                    AIState = BigBoyLaser;
                    AITimer = 0;
                }
            }
            else if (AIState == Intro)
            {
                isLaughing = true;
                rotation = 0;
                if (AITimer == 1)
                {
                    //add sound later
                }
                if (NPC.alpha >= 0)
                {
                    NPC.alpha -= 5;
                }
                if (++AITimer >= 130)
                {
                    AIState = Spew;
                    AITimer = 0;
                    isLaughing = false;
                }
            }
            else if (AIState == Spew)
            {
                NPC.damage = 40;
                NPC.localAI[0] = 40;
                if (AITimer < 51)
                {
                    Vector2 pos = NPC.Center + Main.rand.NextVector2CircularEdge(100, 100);
                    Dust.NewDustPerfect(pos, ModContent.DustType<Dusts.ColoredFireDust>(), Helper.FromAToB(pos, NPC.Center) * Main.rand.NextFloat(3f, 6f), 150, Color.LawnGreen, Main.rand.NextFloat(1, 1.75f) * 0.5f).noGravity = true;
                }
                if (AITimer <= 200 && AITimer > 50)
                {
                    rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                    angle += 0.01f;
                    Vector2 pos = player.Center;
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0225f;
                    if (++AITimer2 >= 5)
                    {
                        for (int i = -2; i < 3; i++)
                        {
                            Projectile Projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 25).RotatedBy(NPC.rotation), 4.25f * Utils.RotatedBy(NPC.DirectionTo(player.Center), (double)(MathHelper.ToRadians(Main.rand.NextFloat(10f, 20f)) * (float)i), default(Vector2)), ModContent.ProjectileType<TFlameThrower>(), 20, 1f, Main.myPlayer)];
                            Projectile.tileCollide = true;
                        }
                        AITimer2 = 0;
                    }
                }
                if (AITimer == 200)
                {
                    //add sound later
                }
                if (AITimer >= 200)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 320)
                {
                    AIState = Dash;
                    angle = 0;
                    isLaughing = false;
                    AITimer = 0;
                }
            }
            else if (AIState == Dash)
            {
                NPC.damage = 40;
                NPC.localAI[0] = 40;
                if (AITimer <= 200 && AITimer > 60)
                {
                    if (AITimer % 60 == 0)
                    {
                        NPC.velocity.X *= 0.98f;
                        NPC.velocity.Y *= 0.98f;
                        Vector2 vector9 = NPC.Center;
                        {
                            float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                            NPC.velocity.X = (float)(Math.Cos(rotation2) * 21) * -1;
                            NPC.velocity.Y = (float)(Math.Sin(rotation2) * 21) * -1;
                        }
                    }
                    else if (AITimer >= 60)
                    {
                        Vector2 toPlayer = player.Center - NPC.Center;
                        rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                    }
                    else
                    {
                        rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
                    }
                }
                else
                {
                    Vector2 toPlayer = player.Center - NPC.Center;
                    rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                }
                if (AITimer == 200)
                {
                    //add sound later
                }
                if (AITimer >= 200)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 320)
                {
                    AIState = ClingerSpecial;
                    isLaughing = false;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == ClingerSpecial)
            {
                NPC.damage = 0;
                NPC.localAI[0] = 0;
                if (AITimer <= 300)
                {
                    Vector2 down = new Vector2(0, 10);
                    rotation = down.ToRotation() - MathHelper.PiOver2;
                    Vector2 pos = player.Center + new Vector2(0, -310);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0545f;
                }
                if (AITimer == 300)
                {
                    //add sound later
                }
                if (AITimer >= 300)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 420)
                {
                    angle = 0;
                    if (!hasDonePhase2ApeShitMode)
                    {
                        AIState = HoverAndLetTheClingersDoTheJob;
                    }
                    else
                    {
                        AIState = HoverAndLetTheClingersDoTheJob2;
                    }
                    isLaughing = false;
                    AITimer = 0;
                }
            }
            else if (AIState == HoverAndLetTheClingersDoTheJob)
            {
                NPC.damage = 0;
                NPC.localAI[0] = 40;
                if (AITimer <= 250)
                {
                    Vector2 toPlayer = player.Center - NPC.Center;
                    rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 340);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0445f;
                }
                if (AITimer == 250)
                {
                    //add sound later
                }
                if (AITimer >= 250)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 370)
                {
                    AIState = CursedFlamesRain;
                    isLaughing = false;
                    AITimer = 0;
                }
            }
            else if (AIState == CursedFlamesRain)
            {
                NPC.damage = 0;
                Vector2 toPlayer = new Vector2(0, -10);
                rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                NPC.localAI[0] = 40;
                if (AITimer <= 150)
                {
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 340);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0445f;
                    if (++AITimer2 % 60 == 0)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-10f, 10f), -10), ModContent.ProjectileType<TFlameThrower2>(), 40, 1f, Main.myPlayer)];
                            projectile.tileCollide = false;
                            projectile.hostile = true;
                            projectile.friendly = false;
                            projectile.timeLeft = 230;
                        }
                    }
                }
                if (AITimer == 150)
                {
                    //add sound later
                }
                if (AITimer >= 150)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 270)
                {
                    AIState = Spew;
                    isLaughing = false;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            /*else if (AIState == Dash2)
            {
                NPC.damage = 40;
                NPC.localAI[0] = 40;
                if (AITimer <= 200 && AITimer > 60)
                {
                    if (AITimer % 40 == 0)
                    {
                        for (int i = -2; i < 3; i++)
                        {
                            Projectile Projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, 25).RotatedBy(NPC.rotation), 4.25f * Utils.RotatedBy(NPC.DirectionTo(player.Center), (double)(MathHelper.ToRadians(Main.rand.NextFloat(10f, 20f)) * (float)i), default(Vector2)), ModContent.ProjectileType<TFlameThrower>(), 20, 1f, Main.myPlayer)];
                            Projectile.tileCollide = true;
                        }
                        Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, Helper.FromAToB(NPC.Center, player.Center), ModContent.ProjectileType<TelegraphLine>(), 0, 0);
                        NPC.velocity.X *= 0.98f;
                        NPC.velocity.Y *= 0.98f;
                        Vector2 vector9 = NPC.Center;
                        {
                            float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                            NPC.velocity.X = (float)(Math.Cos(rotation2) * 25) * -1;
                            NPC.velocity.Y = (float)(Math.Sin(rotation2) * 25) * -1;
                        }
                    }
                    else if (AITimer >= 90)
                    {
                        Vector2 toPlayer = player.Center - NPC.Center;
                        rotation =  toPlayer.ToRotation() - MathHelper.PiOver2;
                    }
                    else
                    {
                        rotation =  NPC.velocity.ToRotation() - MathHelper.PiOver2;
                    }
                }
                else
                {
                    Vector2 toPlayer = player.Center - NPC.Center;
                    rotation =  toPlayer.ToRotation() - MathHelper.PiOver2;
                }
                if (AITimer == 200)
                {
                    //add sound later
                }
                if (AITimer >= 200)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 320)
                {
                    AIState = ClingerSpecial;
                    isLaughing = false;
                    AITimer = 0;
                }
            }*/
            else if (AIState == HoverAndLetTheClingersDoTheJob2)
            {
                NPC.damage = 0;
                NPC.localAI[0] = 40;
                if (AITimer <= 250)
                {
                    Vector2 toPlayer = player.Center - NPC.Center;
                    rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 340);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0445f;
                }
                if (AITimer == 250)
                {
                    //add sound later
                }
                if (AITimer >= 250)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 370)
                {
                    AIState = CursedFlamesRain2;
                    isLaughing = false;
                    AITimer = 0;
                }
            }
            /*else if (AIState == VilespitSpamBecauseYes2)
            {
                NPC.damage = 0;
                angle += 0.05f;
                Vector2 toPlayer = player.Center - NPC.Center;
                rotation =  toPlayer.ToRotation() - MathHelper.PiOver2;
                NPC.localAI[0] = 40;
                if (AITimer <= 250)
                {
                    Vector2 pos = player.Center + Vector2.One.RotatedBy(angle) * 550;
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0445f;
                    if (++AITimer2 >= 15)
                    {
                        //Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, Helper.FromAToB(NPC.Center, player.Center), ModContent.ProjectileType<TelegraphLine>(), 0, 0);
                        NPC.NewNPC(NPC.InheritSource(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, 666);
                        AITimer2 = 0;
                    }
                }
                if (AITimer == 250)
                {
                    //add sound later
                }
                if (AITimer >= 250)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 370)
                {
                    angle = 0;
                    AIState = CursedFlamesRain2;
                    isLaughing = false;
                    AITimer = 0;
                }
            }*/
            else if (AIState == CursedFlamesRain2)
            {
                NPC.damage = 0;
                Vector2 toPlayer = new Vector2(0, -10);
                rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                NPC.localAI[0] = 40;
                if (AITimer <= 150)
                {
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 340);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.0445f;
                    if (++AITimer2 % 45 == 0)
                    {
                        for (int i = 0; i <= 3; i++)
                        {
                            Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Main.rand.NextFloat(-10f, 10f), -10), ModContent.ProjectileType<TFlameThrower2>(), 40, 1f, Main.myPlayer)];
                            projectile.tileCollide = false;
                            projectile.hostile = true;
                            projectile.friendly = false;
                            projectile.timeLeft = 230;
                        }
                    }
                }
                if (AITimer == 150)
                {
                    //add sound later
                }
                if (AITimer >= 150)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (++AITimer >= 270)
                {
                    AIState = ClingerSpecial;
                    isLaughing = false;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == BigBoyLaser)
            {
                AITimer++;
                if (AITimer < 130)
                {
                    Vector2 pos = NPC.Center + Main.rand.NextVector2CircularEdge(100, 100);
                    Dust.NewDustPerfect(pos, ModContent.DustType<Dusts.ColoredFireDust>(), Helper.FromAToB(pos, NPC.Center) * Main.rand.NextFloat(3f, 6f), 150, Color.LawnGreen, Main.rand.NextFloat(1, 1.75f) * 0.25f).noGravity = true;
                    Vector2 toPlayer = player.Center - NPC.Center;
                    rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
                    lastPos = player.Center;
                }
                if (AITimer == 130)
                    Projectile.NewProjectile(NPC.InheritSource(NPC), NPC.Center, Helper.FromAToB(NPC.Center, lastPos), ModContent.ProjectileType<TelegraphLine>(), 0, 0);
                if (AITimer == 160)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath44);
                    EbonianSystem.ScreenShakeAmount = 10;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Helper.FromAToB(NPC.Center, lastPos), ModContent.ProjectileType<TBeam>(), 20, 0);
                }
                if (AITimer >= 160)
                {
                    NPC.velocity *= 0.9f;
                    isLaughing = true;
                }
                if (AITimer == 230)
                {
                    AIState = ClingerSpecial;
                    isLaughing = false;
                    AITimer = 0;
                }
            }
        }
        Vector2 lastPos;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = Main.LocalPlayer;
            if (player.ZoneCorrupt && NPC.downedGolemBoss && !NPC.AnyNPCs(ModContent.NPCType<Terrortoma>()))
            {
                return .015f;
            }
            else
            {
                return 0;
            }
        }
    }
}