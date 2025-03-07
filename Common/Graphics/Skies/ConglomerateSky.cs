﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ModLoader;
using ReLogic.Content;

namespace EbonianMod.Common.Graphics.Skies
{
    internal class ConglomerateSky : CustomSky
    {
        private bool isActive;
        private float intensity;

        public override void Update(GameTime gameTime)
        {
            if (isActive && intensity < 1f)
            {
                intensity += 0.01f;
            }
            else if (!isActive && intensity > 0)
            {
                intensity -= 0.01f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            Rectangle rect = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
            sb.SaveCurrent();
            if (!Main.gameMenu)
            {
                if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
                {
                    sb.Draw(Helper.GetExtraTexture("black"), rect, Color.White * intensity);
                    Main.spriteBatch.SaveCurrent();
                    Main.spriteBatch.End();
                    sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
                    EbonianMod.waterEffect.CurrentTechnique.Passes[0].Apply();
                    gd.Textures[1] = Request<Texture2D>("EbonianMod/Extras/swirlyNoise", AssetRequestMode.ImmediateLoad).Value;
                    gd.Textures[2] = Request<Texture2D>("EbonianMod/Extras/waterNoise", AssetRequestMode.ImmediateLoad).Value;
                    gd.Textures[3] = Request<Texture2D>("EbonianMod/Extras/vein", AssetRequestMode.ImmediateLoad).Value;
                    EbonianMod.waterEffect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.015f);
                    EbonianMod.waterEffect.Parameters["totalAlpha"].SetValue(intensity + EbonianSystem.conglomerateSkyFlash);

                    EbonianMod.waterEffect.Parameters["offset"].SetValue(.05f + Clamp(EbonianSystem.conglomerateSkyFlash, 0, 10) * 0.01f);

                    EbonianMod.waterEffect.Parameters["mainScale"].SetValue(.7f);
                    EbonianMod.waterEffect.Parameters["secondaryScale"].SetValue(2f);
                    EbonianMod.waterEffect.Parameters["tertiaryScale"].SetValue(3.5f);

                    EbonianMod.waterEffect.Parameters["mainDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * -0.0255f) * 0.4f, .45f));
                    EbonianMod.waterEffect.Parameters["secondaryDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.025f) * 0.4f, -.45f));
                    EbonianMod.waterEffect.Parameters["tertiaryDirection"].SetValue(new Vector2(-1.9f, 0));
                    sb.Draw(Helper.GetExtraTexture("conglomerateSky"), rect, Color.White * intensity);
                    sb.ApplySaved();
                }
                Main.spriteBatch.SaveCurrent();
                Main.spriteBatch.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
                EbonianMod.waterEffect.CurrentTechnique.Passes[0].Apply();
                gd.Textures[1] = Request<Texture2D>("EbonianMod/Extras/seamlessNoise", AssetRequestMode.ImmediateLoad).Value;
                gd.Textures[2] = Request<Texture2D>("EbonianMod/Extras/waterNoise", AssetRequestMode.ImmediateLoad).Value;
                gd.Textures[3] = Request<Texture2D>("EbonianMod/Extras/swirlyNoise", AssetRequestMode.ImmediateLoad).Value;
                EbonianMod.waterEffect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * 0.03f);
                EbonianMod.waterEffect.Parameters["totalAlpha"].SetValue(intensity * (0.1f + EbonianSystem.conglomerateSkyFlash * 0.1f));

                EbonianMod.waterEffect.Parameters["offset"].SetValue(.05f);
                EbonianMod.waterEffect.Parameters["colOverride"].SetValue(GetInstance<EbonianSystem>().conglomerateSkyColorOverride.ToVector4());

                EbonianMod.waterEffect.Parameters["mainScale"].SetValue(3);
                EbonianMod.waterEffect.Parameters["secondaryScale"].SetValue(3f);
                EbonianMod.waterEffect.Parameters["tertiaryScale"].SetValue(3);

                EbonianMod.waterEffect.Parameters["mainDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * -0.015f) * 0.1f, .45f));
                EbonianMod.waterEffect.Parameters["secondaryDirection"].SetValue(new Vector2(MathF.Sin(Main.GlobalTimeWrappedHourly * 0.015f) * 0.1f, -.55f));
                EbonianMod.waterEffect.Parameters["tertiaryDirection"].SetValue(new Vector2(2.9f, 0));

                sb.Draw(Helper.GetExtraTexture("conglomerateSky"), rect, Color.White * intensity);
                sb.ApplySaved();
            }
            sb.ApplySaved();
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }
        public override Color OnTileColor(Color inColor)
        {
            return inColor * 0.5f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive || intensity > 0;
        }
    }
}
