////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Elements
{
    /// <summary>
    /// Visject Surface output box element.
    /// </summary>
    /// <seealso cref="FlaxEditor.Surface.Elements.Box" />
    public class OutputBox : Box
    {
        /// <inheritdoc />
        public OutputBox(SurfaceNode parentNode, NodeElementArchetype archetype)
            : base(parentNode, archetype, archetype.Position + new Vector2(parentNode.Archetype.Size.X, 0))
        {
        }

        /// <summary>
        /// Draw all connections comming from this box.
        /// </summary>
        public void DrawConnections()
        {
            // Draw all the connections
            var centerPos = Size * 0.5f;
            for (int i = 0; i < Connections.Count; i++)
            {
                // Find target box location in current box space
                Box targetBox = Connections[i];
                Vector2 targetPos = targetBox.Parent.PointToParent(targetBox.PointToParent(centerPos));
                targetPos = Parent.PointFromParent(PointFromParent(targetPos));

                // Calculate control points
                var dst = (targetPos - centerPos) * new Vector2(0.5f, 0.05f);
                //
                //Vector2 control1 = new Vector2(centerPos.X + dst.X, centerPos.Y);
                //Vector2 control2 = new Vector2(targetPos.X - dst.X, targetPos.Y);
                //
                //Vector2 control1 = control1 + dst;
                //Vector2 control2 = targetPos - dst;
                //
                Vector2 control1 = new Vector2(centerPos.X + dst.X, centerPos.Y + dst.Y);
                Vector2 control2 = new Vector2(targetPos.X - dst.X, targetPos.Y + dst.Y);

                // Draw line
                Render2D.DrawBezier(centerPos, control1, control2, targetPos, _currentTypeColor, 2.2f);
                
                /*
                // Debug drawing control points
                Vector2 bSize = Vector2(4, 4);
                Render2D.FillRectangle(new Rectangle(control1 - bSize * 0.5f, bSize), Color.Blue);
                Render2D.FillRectangle(new Rectangle(control2 - bSize * 0.5f, bSize), Color.Gold);
                */
            }
        }

        /// <inheritdoc />
        public override bool IsOutput => false;

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Box
            DrawBox();

            // Draw text
            var style = Style.Current;
            var rect = new Rectangle(-100, 0, 100 -2, Height);
            Render2D.DrawText(style.FontSmall, Archetype.Text, rect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Far, TextAlignment.Center);
        }
    }
}
