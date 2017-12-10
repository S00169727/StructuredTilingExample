using AnimatedSprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Tiling;
using Helpers;
using Microsoft.Xna.Framework.Graphics;
using TileBasedPlayer20172018;

namespace Tiler
{
    class Sentry : RotatingSprite
    {
        public Projectile sentryProjectile;
        public bool isAlive = true;
        public static int aliveSentries;
        Vector2 target;
        public float chaseRadius = 200;
        public bool following = false;
        float previousAngleOfRotation = 0;

        public Sentry(Game game, Vector2 userPosition,
           List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
               : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
        }
        public bool InChaseZone(TilePlayer p)
        {
            float distance = Math.Abs(Vector2.Distance(this.PixelPosition, p.PixelPosition));
            if (distance <= chaseRadius)
                return true;
            return false;
        }

        public void Follow(TilePlayer p)
        {
            bool inchaseZone = InChaseZone(p);
            if (inchaseZone)
            {
                this.angleOfRotation = TurnToFace(PixelPosition, p.PixelPosition, angleOfRotation, .3f);
                this.following = true;
                target = p.PixelPosition;
            }
            else
            {
                this.following = false;
            }

        }

        public void LoadProjectile(Projectile r)
        {
            sentryProjectile = r;
            sentryProjectile.DrawOrder = 2;
        }

        public void Die()
        {
            isAlive = false;
            aliveSentries--;
        }

        public override void Draw(GameTime gameTime)
        {
            if (isAlive)
            {
                if (sentryProjectile != null && sentryProjectile.ProjectileState != Projectile.PROJECTILE_STATE.STILL)
                    sentryProjectile.Draw(gameTime);
                base.Draw(gameTime);
            }
        }
        public override void Update(GameTime gametime)
        {
            if (isAlive)
            {
                if (sentryProjectile != null && sentryProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
                {
                    sentryProjectile.PixelPosition = this.PixelPosition;
                    //sentryProjectile.hit = false;
                    // fire the rocket and it looks for the target
                   // if (following && previousAngleOfRotation == angleOfRotation)
                      //  sentryProjectile.fire(target);
                }

                previousAngleOfRotation = angleOfRotation;
                base.Update(gametime);
            }



        }
    }
}
