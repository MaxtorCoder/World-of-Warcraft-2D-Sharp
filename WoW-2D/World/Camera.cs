using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW_2D.World
{
    /// <summary>
    /// The camera.
    /// </summary>
    public class Camera
    {
        private Viewport viewport;
        private DefaultViewportAdapter defaultViewport;

        public Vector2 Origin = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public RectangleF ClampBounds;

        public Camera(GraphicsDevice graphicsDevice)
        {
            viewport = graphicsDevice.Viewport;
            defaultViewport = new DefaultViewportAdapter(graphicsDevice);

            Origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
        }

        public void Update(Vector2 position)
        {
            Position = CheckClamps(position);
        }

        private Vector2 CheckClamps(Vector2 position)
        {
            var newPosition = position - new Vector2(viewport.Width / 2, viewport.Height / 2);
            var inputInversed = Matrix.Invert(
                Matrix.CreateTranslation(new Vector3(-newPosition * Vector2.One, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(0f) *
                Matrix.CreateScale(1.5f, 1.5f, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f))
                ) * defaultViewport.GetScaleMatrix();
            var inversedVector = inputInversed.Translation;
            var oldPos = Position;

            if (inversedVector.X <= ClampBounds.Left) newPosition.X = oldPos.X;
            if (inversedVector.Y <= ClampBounds.Top) newPosition.Y = oldPos.Y;
            if ((inversedVector.X + BoundingRectangle.Width) >= ClampBounds.Right) newPosition.X = oldPos.X;
            if ((inversedVector.Y + BoundingRectangle.Height) >= ClampBounds.Bottom) newPosition.Y = oldPos.Y;

            return newPosition;
        }

        #region MonoGame.Extended Methods.
        public Matrix GetViewMatrix(Vector2 parallaxFactor)
        {
            return GetVirtualViewMatrix(parallaxFactor) * defaultViewport.GetScaleMatrix();
        }

        private Matrix GetVirtualViewMatrix(Vector2 parallaxFactor)
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position * parallaxFactor, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(0f) *
                Matrix.CreateScale(1.5f, 1.5f, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        private Matrix GetVirtualViewMatrix()
        {
            return GetVirtualViewMatrix(Vector2.One);
        }

        public Matrix GetViewMatrix()
        {
            return GetViewMatrix(Vector2.One);
        }

        private Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, defaultViewport.VirtualWidth, defaultViewport.VirtualHeight, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);
            return projection;
        }

        public BoundingFrustum GetBoundingFrustum()
        {
            var viewMatrix = GetVirtualViewMatrix();
            var projectionMatrix = GetProjectionMatrix(viewMatrix);
            return new BoundingFrustum(projectionMatrix);
        }

        public RectangleF BoundingRectangle
        {
            get
            {
                var frustum = GetBoundingFrustum();
                var corners = frustum.GetCorners();
                var topLeft = corners[0];
                var bottomRight = corners[2];
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                return new RectangleF(topLeft.X, topLeft.Y, width, height);
            }
        }
        #endregion
    }
}
