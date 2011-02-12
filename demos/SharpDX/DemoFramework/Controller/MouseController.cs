﻿using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Windows;

namespace DemoFramework
{
    public class MouseController
    {
        public Vector3 Vector { get; set; }
        public Vector2 DragPoint { get; private set; }
        public float Sensitivity { get; set; }

        Point MouseOrigin;
        double AngleOriginX, AngleOriginY;
        double AngleDeltaX, AngleDeltaY;
        int RightDragX, RightDragY;
        int RightDragDeltaX, RightDragDeltaY;

        public MouseController()
        {
            Sensitivity = 0.005f;
            SetByAngles(0, 0);
        }

        // HorizontalAngle - left-right movement (parallel to XZ-plane)
        // VerticalAngle - up-down movement (angle between Vector and Y-axis)
        public void SetByAngles(double horizontalAngle, double verticalAngle)
        {
            Vector = new Vector3(
                (float)(Math.Cos(horizontalAngle) * Math.Cos(verticalAngle)),
                (float)Math.Sin(verticalAngle),
                (float)(Math.Sin(horizontalAngle) * Math.Cos(verticalAngle)));
        }

        public bool Update(Input input)
        {
            // Don't allow both actions at once
            if ((input.MouseDown & (MouseButtons.Left | MouseButtons.Right))
                != (MouseButtons.Left | MouseButtons.Right))
            {
                // When mouse button is clicked, store cursor position and angles
                if ((input.MousePressed & MouseButtons.Left) == MouseButtons.Left)
                {
                    MouseOrigin = input.MousePoint;

                    // Get normalized Vector
                    Vector3 Norm = Vector3.Normalize(Vector);

                    // Calculate angles from the vector
                    AngleOriginX = Math.Atan2(Norm.Z, Norm.X);
                    AngleOriginY = Math.Asin(Norm.Y);
                }
                else if ((input.MousePressed & MouseButtons.Right) == MouseButtons.Right)
                {
                    MouseOrigin = input.MousePoint;
                }
            }
            /*
            if ((input.MousePressed & MouseButtonFlags.RightUp) == MouseButtonFlags.RightUp)
            {
                RightDragX += RightDragDeltaX;
                RightDragY += RightDragDeltaY;
            }
            */
            if ((input.MouseDown & MouseButtons.Left) == MouseButtons.Left)
            {
                // Calculate how much to change the angles
                AngleDeltaX = -(input.MousePoint.X - MouseOrigin.X) * Sensitivity;
                AngleDeltaY = (input.MousePoint.Y - MouseOrigin.Y) * Sensitivity;

                SetByAngles(AngleOriginX + AngleDeltaX, AngleOriginY + AngleDeltaY);
            }
            else if ((input.MouseDown & MouseButtons.Right) == MouseButtons.Right)
            {
                RightDragDeltaX = (input.MousePoint.X - MouseOrigin.X);
                RightDragDeltaY = (input.MousePoint.Y - MouseOrigin.Y);
                DragPoint = new Vector2(RightDragDeltaX + RightDragX,
                    RightDragDeltaY + RightDragY);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}