using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GridFighter
{
    static class InputManager
    {
        static private Vector2 mousePosition;
        static private MouseState currentMouseState;
        static private MouseState oldMouseState;
        static private KeyboardState currentKeyboardState;
        static private KeyboardState oldKeyboardState;

        static public float getMouseX()
        {
            return mousePosition.X;
        }
        static public float getMouseY()
        {
            return mousePosition.Y;
        }
        static public MouseState getCurrentMouse()
        {
            return currentMouseState;
        }
        static public MouseState getOldMouse()
        {
            return oldMouseState;
        }
        static public KeyboardState getCurrentKeyboard()
        {
            return currentKeyboardState;
        }
        static public KeyboardState getOldKeyboard()
        {
            return oldKeyboardState;
        }

        static public void firstStateUpdate()
        {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;
        }
        static public void lastStateUpdate()
        {
            oldMouseState = currentMouseState;
            oldKeyboardState = currentKeyboardState;
        }


    }
}
