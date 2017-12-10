﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    class CrossHair : AnimateSheetSprite
    {
        private Game myGame;
        private float CrossHairVelocity = 5.0f;

        public CrossHair(Game g, Texture2D texture, Vector2 userPosition, int framecount) : base(g,userPosition,framecount)
            {
                myGame = g;

            }

        public override void Update(GameTime gametime)
        {
            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                this.TilePosition += new Vector2(1, 0) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                this.TilePosition += new Vector2(-1, 0) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                this.TilePosition += new Vector2(0, -1) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                this.TilePosition += new Vector2(0, 1) * CrossHairVelocity;

            // Make sure the Cross Hair stays in the bounds see previous lab for details
            TilePosition = Vector2.Clamp(TilePosition, Vector2.Zero,
                                            new Vector2(gameScreen.Width - FrameWidth,
                                                        gameScreen.Height - FrameHeight));
            
            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }
    }
}
