using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoosterPack
{
    public class MouseHandle
    {
        public MouseState lastState;
        public MouseState state;
        public Point lastPosition;
        public Point lastClickPosition;
        public Point mousePosition;

        public MouseHandle()
        {
            lastState = Mouse.GetState();
            state = Mouse.GetState();
            mousePosition = state.Position;
        }

        public void UpdateMouse()
        {
            lastPosition = mousePosition;
            lastState = state;
            state = Mouse.GetState();
            mousePosition = state.Position;
        }

        public bool Click()
        {
            if (lastState.LeftButton == ButtonState.Released &&
                state.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public bool RClick()
        {
            if (lastState.RightButton == ButtonState.Released &&
                state.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public bool MiddleClick()
        {
            if (lastState.MiddleButton == ButtonState.Released &&
                state.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        public bool Contains(Rectangle r)
        {
            return r.Contains(mousePosition);
        }
    }
}
