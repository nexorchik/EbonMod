using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.ID;
using EbonianMod.Dusts;
using EbonianMod.NPCs.Exol;
using EbonianMod.Skies;
using System.Collections.Generic;
using EbonianMod.Projectiles.Terrortoma;
using EbonianMod.Projectiles.VFXProjectiles;
using ReLogic.Graphics;
using EbonianMod.Misc;
using EbonianMod.Projectiles.Exol;
////using EbonianMod.Worldgen.Subworlds;
////using SubworldLibrary;
using Terraria.GameContent.Drawing;

namespace EbonianMod
{
    public class EbonianMod : Mod
    {
        public static EbonianMod Instance;
        public static Effect Tentacle, TentacleBlack, TentacleRT, ScreenDistort, TextGradient, TextGradient2, TextGradientY, BeamShader, Lens, Test1, Test2, LavaRT, Galaxy, CrystalShine, HorizBlur, TrailShader, RTAlpha, Crack;
        public RenderTarget2D render;
        public static DynamicSpriteFont lcd;
        public static BGParticleSys sys;
        public override void Load()
        {

            sys = new();
            Instance = this;
            Test1 = ModContent.Request<Effect>("EbonianMod/Effects/Test1", (AssetRequestMode)1).Value;
            HorizBlur = ModContent.Request<Effect>("EbonianMod/Effects/horizBlur", (AssetRequestMode)1).Value;
            Crack = ModContent.Request<Effect>("EbonianMod/Effects/crackTest", (AssetRequestMode)1).Value;
            RTAlpha = ModContent.Request<Effect>("EbonianMod/Effects/RTAlpha", (AssetRequestMode)1).Value;
            CrystalShine = ModContent.Request<Effect>("EbonianMod/Effects/CrystalShine", (AssetRequestMode)1).Value;
            TextGradient = ModContent.Request<Effect>("EbonianMod/Effects/TextGradient", (AssetRequestMode)1).Value;
            TextGradient2 = ModContent.Request<Effect>("EbonianMod/Effects/TextGradient2", (AssetRequestMode)1).Value;
            TextGradientY = ModContent.Request<Effect>("EbonianMod/Effects/TextGradientY", (AssetRequestMode)1).Value;
            Test2 = ModContent.Request<Effect>("EbonianMod/Effects/Test2", (AssetRequestMode)1).Value;
            Galaxy = ModContent.Request<Effect>("EbonianMod/Effects/Galaxy", (AssetRequestMode)1).Value;
            LavaRT = ModContent.Request<Effect>("EbonianMod/Effects/LavaRT", (AssetRequestMode)1).Value;
            BeamShader = ModContent.Request<Effect>("EbonianMod/Effects/Beam", (AssetRequestMode)1).Value;
            Lens = ModContent.Request<Effect>("EbonianMod/Effects/Lens", (AssetRequestMode)1).Value;
            Tentacle = ModContent.Request<Effect>("EbonianMod/Effects/Tentacle", (AssetRequestMode)1).Value;
            TentacleRT = ModContent.Request<Effect>("EbonianMod/Effects/TentacleRT", (AssetRequestMode)1).Value;
            ScreenDistort = ModContent.Request<Effect>("EbonianMod/Effects/DistortMove", (AssetRequestMode)1).Value;
            TentacleBlack = ModContent.Request<Effect>("EbonianMod/Effects/TentacleBlack", (AssetRequestMode)1).Value;
            TrailShader = ModContent.Request<Effect>("EbonianMod/Effects/TrailShader", (AssetRequestMode)1).Value;
            Filters.Scene["EbonianMod:CorruptTint"] = new Filter(new BasicScreenTint("FilterMiniTower").UseColor(.68f, .56f, .73f).UseOpacity(0.35f), EffectPriority.Medium);
            SkyManager.Instance["EbonianMod:CorruptTint"] = new BasicTint();
            Filters.Scene["EbonianMod:CrimsonTint"] = new Filter(new BasicScreenTint("FilterMiniTower").UseColor(.75f, 0f, 0f).UseOpacity(0.45f), EffectPriority.Medium);
            SkyManager.Instance["EbonianMod:CrimsonTint"] = new BasicTint();
            Filters.Scene["EbonianMod:HellTint"] = new Filter(new BasicScreenTint("FilterMiniTower").UseColor(2.55f, .97f, .31f).UseOpacity(0.2f), EffectPriority.Medium);
            SkyManager.Instance["EbonianMod:HellTint"] = new BasicTint();
            Filters.Scene["EbonianMod:HellTint2"] = new Filter(new BasicScreenTint("FilterMiniTower").UseColor(2.55f, .97f, .31f).UseOpacity(0.425f), EffectPriority.Medium);
            SkyManager.Instance["EbonianMod:HellTint2"] = new BasicTint();
            Filters.Scene["EbonianMod:ScreenFlash"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("EbonianMod/Effects/ScreenFlash", (AssetRequestMode)1).Value), "Flash"), EffectPriority.VeryHigh);
            Terraria.Graphics.Effects.On_FilterManager.EndCapture += FilterManager_EndCapture;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            Terraria.On_Main.DrawBG += DrawBehindTilesAndWalls;
            Terraria.On_Main.DrawNPC += DrawNPC;
            //On.Terraria.Main.DoUpdate += testSlowDown;
            //On.Terraria.Audio.SoundEngine.Update += testSlowDownAudio;
            CreateRender();
        }
        /*public static int timeSkips;
        public static int _timeSkips;
        private void testSlowDown(On.Terraria.Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
        {
            if (Main.gameMenu || Main.gamePaused || Main.LocalPlayer.dead)
                timeSkips = 0;
            if (timeSkips > 0 && !Main.gameMenu)
            {
                _timeSkips++;
                if (_timeSkips > timeSkips)
                    _timeSkips = 0;
                if (_timeSkips < timeSkips)
                    return;
            }
            orig(self, ref gameTime);
        }


        private void testSlowDownAudio(On.Terraria.Audio.SoundEngine.orig_Update orig)
        {
            if (timeSkips > 0 && !Main.gameMenu)
            {
                if (_timeSkips < timeSkips)
                    return;
            }
            orig();
        }
        */
        void DrawNPC(Terraria.On_Main.orig_DrawNPC orig, global::Terraria.Main self, int iNPCIndex, bool behindTiles)
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            SmokeDustAkaFireDustButNoGlow.DrawAll(Main.spriteBatch);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            orig(self, iNPCIndex, behindTiles);
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && (projectile.type == ModContent.ProjectileType<TExplosion>()/* || projectile.type == ModContent.ProjectileType<ScreenFlash>()*/))
                {
                    Color color = Color.White;
                    projectile.ModProjectile.PreDraw(ref color);
                }
            }
            if (FlashAlpha > 0)
            {
                Main.spriteBatch.Draw(Helper.GetExtraTexture("Line"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * FlashAlpha * 2);
            }
        }

        public void DrawBehindTilesAndWalls(Terraria.On_Main.orig_DrawBG orig, global::Terraria.Main self)
        {
            orig(self);
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && (projectile.type == ModContent.ProjectileType<EBoulder>() || projectile.type == ModContent.ProjectileType<EBoulder2>()))
                {
                    Color color = Color.White;
                    projectile.ModProjectile.PreDraw(ref color);
                }
            }
            sys.DrawParticles();

        }
        public override void Unload()
        {
            sys = null;
            Test1 = null;
            RTAlpha = null;
            CrystalShine = null;
            TextGradient = null;
            TextGradient2 = null;
            TextGradientY = null;
            Test2 = null;
            Galaxy = null;
            LavaRT = null;
            BeamShader = null;
            Lens = null;
            Tentacle = null;
            TentacleRT = null;
            ScreenDistort = null;
            TentacleBlack = null;
            TrailShader = null;
            Terraria.Graphics.Effects.On_FilterManager.EndCapture -= FilterManager_EndCapture;
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            Terraria.On_Main.DrawBG -= DrawBehindTilesAndWalls;
        }
        private void Main_OnResolutionChanged(Vector2 obj)
        {
            CreateRender();
        }
        public void CreateRender()
        {
            if (Main.netMode != NetmodeID.Server)
                Main.QueueMainThreadAction(() =>
                {
                    render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                });
        }

        public static int ExolID = ModContent.NPCType<Exol>();
        public static float FlashAlpha;
        private void FilterManager_EndCapture(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            #region "rt2d"
            #endregion
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            FireDust.DrawAll(sb);
            ColoredFireDust.DrawAll(sb);
            GenericAdditiveDust.DrawAll(sb);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            if (FlashAlpha > 0)
            {
                Main.spriteBatch.Draw(Helper.GetExtraTexture("Line"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * FlashAlpha * 2);
            }
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && (projectile.type == ModContent.ProjectileType<TExplosion>()))// || projectile.type == ModContent.ProjectileType<ScreenFlash>()))
                {
                    Color color = Color.White;
                    projectile.ModProjectile.PreDraw(ref color);
                }
            }
            /*foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == ModContent.NPCType<Exol>())
                {
                    Color color = Color.White;
                    npc.ModNPC.PreDraw(sb, Main.screenPosition, color);
                }
            }*/
            sb.End();
            #region "ripple"
            if (Main.netMode != NetmodeID.Server)
            {
                gd.SetRenderTarget(render);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                sb.End();
                gd.SetRenderTarget(Main.screenTargetSwap);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.active && projectile.type == ModContent.ProjectileType<Projectiles.Ripple>())
                    {
                        Texture2D a = TextureAssets.Projectile[projectile.type].Value;
                        Main.spriteBatch.Draw(a, projectile.Center - Main.screenPosition, null, Color.White, 0, a.Size() / 2, projectile.ai[0], SpriteEffects.None, 0f);
                    }
                }
                sb.End();
                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                Test2.CurrentTechnique.Passes[0].Apply();
                Test2.Parameters["tex0"].SetValue(Main.screenTargetSwap);
                Test2.Parameters["i"].SetValue(0.02f);
                sb.Draw(render, Vector2.Zero, Color.White);
                sb.End();

            }
            #endregion
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
    }
    public class EbonMenu : ModMenu
    {
        public override string DisplayName => "Ebonian Mod";
        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<EbonMenuBG>();
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("EbonianMod/Extras/Sprites/Logo");
        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            //spriteBatch.Draw(Helper.GetExtraTexture("menutest"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            drawColor = Color.White;
            return true;
        }
        public class EbonMenuBG : ModSurfaceBackgroundStyle
        {
            public override void ModifyFarFades(float[] fades, float transitionSpeed)
            {
            }
        }
    }
}
