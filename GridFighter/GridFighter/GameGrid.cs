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
    class GameGrid
    {

        private Cell[,] GridArray = new Cell[8, 6];
        private int cellWidth = 72/1;
        private int cellHeight = 72/1;
        private Vector2 gridPosition = new Vector2(10, 10); //47,15

        private Boolean initialSetup = true;
        private int selectInt, gameState, numberofCellTypes = 7;

        private static Random rand = new Random();
        private Vector2 matches, selectedCell, secondCell;
        private Boolean mouseOver = false;

        Texture2D cellIcon, gridBack;
        SpriteFont font;

        private static int timeValue = 12;
        private int counter = timeValue;
        
        /// <summary>
        /// Initialises the grid by assigning a value to every cell
        /// </summary>
        public GameGrid()
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    GridArray[cX, cY] = new Cell();
                    GridArray[cX, cY].setCellID(rand.Next(1, numberofCellTypes));
                }
            }
        }
        /// <summary>
        /// Sets every cell to unselected on the grid, except for one coordinate
        /// Useful for when you have clicked one cell and need to look for the second cell
        /// </summary>
        /// <param name="x">the x coordinate of the value that isnt unselected</param>
        /// <param name="y">the y coordinate of the value that isnt unselected</param>
        private void setEverythingUnselected(int x, int y)
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    if (cX != x || cY != y)
                    {
                        GridArray[cX, cY].setSelected(false);
                    }
                }
            }
        }
        /// <summary>
        /// sets every cell to unselected on the grid
        /// </summary>
        private void setEverythingUnselected()
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    GridArray[cX, cY].setSelected(false);
                }
            }
        }
        private void setEverythingUnvisited()
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    GridArray[cX, cY].setVisited(false);
                }
            }
        }
        /// <summary>
        /// Starting from the botto (now we're here) right hand side of the grid, if the array finds a cell with an empty cell underneath the original it will 'drag' that cell down
        /// until it gets to the bottom of the column
        /// </summary>
        /// <returns>This returns true if an item is pulled down because it means there is an empty spot at the top of the column</returns>
        private Boolean dropDown()
        {
            Boolean emptySpot = false;
            for (int cX = GridArray.GetLength(0) - 1; cX >= 0; cX--)
            {
                for (int cY = GridArray.GetLength(1) - 1; cY >= 0; cY--)
                {
                    int next = cY;
                    if (cY + 1 != GridArray.GetLength(1))
                    {
                        if ((GridArray[cX, next + 1].getCellID().Equals(0)) && ((!GridArray[cX, cY].getCellID().Equals(0))))
                        {
                            GridArray[cX, next + 1].setCellID(GridArray[cX, next].getCellID());
                            GridArray[cX, next].setCellID(0);
                            next++;
                            emptySpot = true;
                            counter = timeValue;
                        }
                    }
                }
            }
            return emptySpot;
        }
        /// <summary>
        /// If the loop finds an empty cell it will go to the top of the column to place
        /// a new cell
        /// </summary>
        private void fillUp()
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    if (GridArray[cX, cY].getCellID().Equals(0))
                    {
                        GridArray[cX, 0].setCellID(rand.Next(1, numberofCellTypes));
                        counter = timeValue;
                    }
                }
            }
        }
        /// <summary>
        /// If there is a selected cell around the cell that you have just sent this then it will return true
        /// This check excludes the edges
        /// </summary>
        /// <param name="x">x coordinates of second cell to be checked</param>
        /// <param name="y">y coordinates of second cell to be checked</param>
        /// <returns>Returns true if the value it has been given has a selected cell next to it</returns>
        private Boolean isSurroundingSelected(int x, int y)
        {
            bool value = false;

            if (x - 1 >= 0)
            {
                if (GridArray[x - 1, y].getSelected())
                {
                    if (!value) { value = true; }
                }
            }
            if (x + 1 != GridArray.GetLength(0))
            {
                if (GridArray[x + 1, y].getSelected())
                {
                    if (!value) { value = true; }
                }
            }
            if (y - 1 >= 0)
            {
                if (GridArray[x, y - 1].getSelected())
                {
                    if (!value) { value = true; }
                }
            }
            if (y + 1 != GridArray.GetLength(1))
            {
                if (GridArray[x, y + 1].getSelected())
                {
                    if (!value) { value = true; }
                }
            }

            return value;
        }
        /// <summary>
        /// Switches the cells of the first coordinates you give with the second coordinates that you give it
        /// </summary>
        /// <param name="x1">First cell X Coordinates</param>
        /// <param name="y1">First cell Y Coordinates</param>
        /// <param name="x2">Second cell X Coordinates</param>
        /// <param name="y2">Second cell Y Coordinates</param>
        private void switchCells(int x1, int y1, int x2, int y2)
        {
            int tempID;
            tempID = GridArray[x2, y2].getCellID();
            GridArray[x2, y2].setCellID(GridArray[x1, y1].getCellID());
            GridArray[x1, y1].setCellID(tempID);
            setEverythingUnselected();
        }
        /// <summary>
        /// Check the whole grid for an empty cell, if there is an empty cell it returns true 
        /// </summary>
        /// <returns>Returns a boolean depending on if a zero cell is found in the grid</returns>
        private Boolean thereIsntAZeroInGrid()
        {
            Boolean zero = true;
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    if (GridArray[cX, cY].getCellID().Equals(0))
                    {
                        zero = false;
                        return zero;
                    }
                }
            }
            return zero;
        }
        /// <summary>
        /// Sets the cell it finds matching to true but doesnt delete the value itself
        /// Only sets it to matching if 3 or more cells are matching
        /// </summary>
        private void matchCells()
        {
            if (thereIsntAZeroInGrid())
            {
                for (int cX = 0; cX < GridArray.GetLength(0); cX++)
                {
                    for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                    {
                        matches.X = 0;
                        matchCellsX(cX, cY);

                        matches.Y = 0;
                        matchCellsY(cX, cY);

                        //if (counter == 0)
                        {
                            if (matches.X > 1) //check right
                            {
                                for (int c = 0; c < matches.X + 1; c++)
                                {
                                    if (cX + c < GridArray.GetLength(0))
                                    {
                                        GridArray[cX + c, cY].setMatched(true);
                                        //GridArray[cX + c, cY].setVisited(true);
                                    }
                                }
                            }
                            if (matches.Y > 1) //check down
                            {
                                for (int c = 0; c < matches.Y + 1; c++)
                                {
                                    if (cY + c < GridArray.GetLength(1))
                                    {
                                        GridArray[cX, cY + c].setMatched(true);
                                        //GridArray[cX, cY + c].setVisited(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void matchCells(int x1, int y1, int x2, int y2)
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    matches.X = 0;
                    matchCellsX(cX, cY);

                    matches.Y = 0;
                    matchCellsY(cX, cY);

                    if (matches.X > 1) //check right
                    {
                        for (int c = 0; c < matches.X + 1; c++)
                        {
                            if (cX + c < GridArray.GetLength(0))
                            {
                                //GridArray[cX + c, cY].setMatched(true);
                                GridArray[cX + c, cY].setVisited(true);
                            }
                        }
                    }
                    if (matches.Y > 1) //check down
                    {
                        for (int c = 0; c < matches.Y + 1; c++)
                        {
                            if (cY + c < GridArray.GetLength(1))
                            {
                                //GridArray[cX, cY + c].setMatched(true);
                                GridArray[cX, cY + c].setVisited(true);
                            }
                        }
                    }
                    if (GridArray[x1, y1].getVisited() || GridArray[x2, y2].getVisited())
                    {
                        if (matches.X > 1) //check right
                        {
                            for (int c = 0; c < matches.X + 1; c++)
                            {
                                if (cX + c < GridArray.GetLength(0))
                                {
                                    GridArray[cX + c, cY].setMatched(true);
                                }
                            }
                        }
                        if (matches.Y > 1) //check down
                        {
                            for (int c = 0; c < matches.Y + 1; c++)
                            {
                                if (cY + c < GridArray.GetLength(1))
                                {
                                    GridArray[cX, cY + c].setMatched(true);
                                }
                            }
                        }
                    }

                }
            }
        }
        private void matchCellsX(int cX, int cY)
        {
            if ((cX + 1 != GridArray.GetLength(0)) && (GridArray[cX, cY].getCellID().Equals(GridArray[cX + 1, cY].getCellID())) && (!GridArray[cX, cY].getCellID().Equals(0)))
            {
                matches.X++;
                matchCellsX(cX + 1, cY);
            }
        }
        private void matchCellsY(int cX, int cY)
        {
            if ((cY + 1 != GridArray.GetLength(1)) && (GridArray[cX, cY].getCellID().Equals(GridArray[cX, cY + 1].getCellID())))
            {
                matches.Y++;
                matchCellsY(cX, cY + 1);
            }
        }
        /// <summary>
        /// Sets every matched cell in the grid to zero
        /// </summary>
        private void toZero()
        {
            Boolean matchMade = false;
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    if (GridArray[cX, cY].getMatched())
                    {
                        GridArray[cX, cY].setCellID(0);
                        GridArray[cX, cY].setMatched(false);
                        matchMade = true;
                    }
                }
            }
            if (matchMade)
            {
                counter = timeValue;
            }
        }
        /// <summary>
        /// This will check the movements of any 2 coordinates and see if they have cause any new matches to be made within the grid
        /// </summary>
        /// <param name="x1">First X Coordinate</param>
        /// <param name="y1">First Y Coordinate</param>
        /// <param name="x2">Second X Coordinate</param>
        /// <param name="y2">Second Y Coordinate</param>
        /// <returns></returns>
        private Boolean validMove(int x1, int y1, int x2, int y2)
        {
            Boolean valid = false;
            matchCells(x1, y1, x2, y2);
            if (GridArray[x1, y1].getVisited() || GridArray[x2, y2].getVisited())
            {
                valid = true;
            }
            setEverythingUnvisited();
            return valid;

        }
        /// <summary>
        /// Loads the relevant sprites into the grid
        /// </summary>
        /// <param name="g1"></param>
        public void LoadContent(Game g1)
        {
            cellIcon = g1.Content.Load<Texture2D>(@"graphics/CellIcon");
            gridBack = g1.Content.Load<Texture2D>(@"graphics/GridBacking");
            font = g1.Content.Load<SpriteFont>(@"graphics/GridFighterFont");

        }
        /// <summary>
        /// Where the class draws onto the screen
        /// </summary>
        /// <param name="s"></param>
        public void Draw(SpriteBatch s)
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {
                    
                    if (GridArray[cX, cY].getSelected())
                    { selectInt = 1; }
                    else
                    { selectInt = 0; }
                    if (GridArray[cX, cY].getMatched())
                    {
                        s.Draw(cellIcon, new Rectangle((int)gridPosition.X + cX * cellWidth, (int)gridPosition.Y + cY * cellHeight, cellWidth, cellHeight), new Rectangle(36 * GridArray[cX, cY].getCellID(), 36 * selectInt, 36, 36), Color.Black);
                    }
                    else
                    {
                        s.Draw(cellIcon, new Rectangle((int)gridPosition.X + cX * cellWidth, (int)gridPosition.Y + cY * cellHeight, cellWidth, cellHeight), new Rectangle(36 * GridArray[cX, cY].getCellID(), 36 * selectInt, 36, 36), Color.White);
                    }
                    s.Draw(gridBack, new Rectangle((int)gridPosition.X + cX * cellWidth, (int)gridPosition.Y + cY * cellHeight, cellWidth, cellHeight), new Rectangle(0, 0, gridBack.Width, gridBack.Height), Color.White);
                    s.DrawString(font, counter.ToString(), new Vector2(5,2), Color.GhostWhite);
                }
            }
        }
        /// <summary>
        /// Is called every frame and runs whatever is called continuously
        /// </summary>
        public void Update()
        {
            for (int cX = 0; cX < GridArray.GetLength(0); cX++)
            {
                for (int cY = 0; cY < GridArray.GetLength(1); cY++)
                {

                    if (initialSetup)
                    {
                        matchCells();
                        toZero();
                        dropDown();
                        fillUp();
                    }
                    else
                    {
                        matchCells();
                        if (mouseOver)
                        {
                            if (counter == 0)
                            {

                                if (!validMove((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y))
                                {
                                    if ((selectedCell.X != secondCell.X) || (selectedCell.Y != secondCell.Y))
                                    {
                                        switchCells((int)secondCell.X, (int)secondCell.Y, (int)selectedCell.X, (int)selectedCell.Y);
                                    }
                                }
                                counter = timeValue;
                                gameState = 0;
                                mouseOver = false;
                            }
                        }
                        if (counter == 0)
                        {
                            toZero();
                            dropDown();
                            fillUp();
                        }
                        if (InputManager.getMouseX() > (int)gridPosition.X + cX * cellWidth && InputManager.getMouseX() < (int)(gridPosition.X + cX * cellWidth) + cellWidth &&
                            InputManager.getMouseY() > (int)gridPosition.Y + cY * cellHeight && InputManager.getMouseY() < (int)(gridPosition.Y + cY * cellHeight) + cellHeight)
                        {
                            if (gameState == 0)
                            {
                                if (InputManager.getCurrentMouse().LeftButton == ButtonState.Pressed && InputManager.getOldMouse().LeftButton == ButtonState.Released)
                                {
                                    if (!GridArray[cX, cY].getCellID().Equals(0))
                                    {
                                        if (!mouseOver || ((cX != selectedCell.X) && (cY != selectedCell.Y)))
                                        {
                                            GridArray[cX, cY].setSelected(true);
                                            setEverythingUnselected(cX, cY);
                                            selectedCell.X = cX;
                                            selectedCell.Y = cY;
                                            gameState++;
                                        }
                                    }
                                }
                            }
                            else if (gameState == 1)
                            {
                                if (InputManager.getOldMouse().LeftButton == ButtonState.Released && InputManager.getCurrentMouse().LeftButton == ButtonState.Pressed)
                                {
                                    if (!GridArray[cX, cY].getCellID().Equals(0))
                                    {
                                        if (isSurroundingSelected(cX, cY))
                                        {
                                            secondCell.X = cX;
                                            secondCell.Y = cY;
                                            switchCells((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y);
                                            mouseOver = true;
                                            gameState = 0;
                                        }
                                        else if (cX == selectedCell.X && cY == selectedCell.Y)
                                        {
                                            secondCell.X = cX;
                                            secondCell.Y = cY;
                                            setEverythingUnselected();
                                            gameState = 0;
                                        }
                                        else
                                        {
                                            setEverythingUnselected();
                                            GridArray[cX, cY].setSelected(true);
                                            selectedCell.X = cX;
                                            selectedCell.Y = cY;
                                        }
                                        counter = timeValue;
                                    }
                                }
                                else if (InputManager.getOldMouse().LeftButton == ButtonState.Pressed && InputManager.getCurrentMouse().LeftButton == ButtonState.Released && (selectedCell.X != cX || selectedCell.Y != cY))
                                {
                                    if (!GridArray[cX, cY].getCellID().Equals(0))
                                    {
                                        if ((cX > (int)selectedCell.X) && (cY == (int)selectedCell.Y))
                                        {// DRAG TO THE RIGHT
                                            secondCell.X = selectedCell.X + 1;
                                            secondCell.Y = selectedCell.Y;
                                            switchCells((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y);
                                            mouseOver = true;
                                        }
                                        else if ((cX < (int)selectedCell.X) && (cY == (int)selectedCell.Y))
                                        {// DRAG TO THE LEFT
                                            secondCell.X = selectedCell.X - 1;
                                            secondCell.Y = selectedCell.Y;
                                            switchCells((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y);
                                            mouseOver = true;
                                        }
                                        else if ((cY < (int)selectedCell.Y) && (cX == (int)selectedCell.X))
                                        {// DRAG DOWN
                                            secondCell.X = selectedCell.X;
                                            secondCell.Y = selectedCell.Y - 1;
                                            switchCells((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y);
                                            mouseOver = true;
                                        }
                                        else if ((cY > (int)selectedCell.Y) && (cX == (int)selectedCell.X))
                                        {// DRAG UP
                                            secondCell.X = selectedCell.X;
                                            secondCell.Y = selectedCell.Y + 1;
                                            switchCells((int)selectedCell.X, (int)selectedCell.Y, (int)secondCell.X, (int)secondCell.Y);
                                            mouseOver = true;
                                        }
                                        else { setEverythingUnselected(); }
                                        gameState = 0;
                                        //mouseOver = true;
                                        counter = timeValue;
                                    }
                                }

                            }

                        }
                    }
                }
            }

            initialSetup = false;
            if (counter > 0)
            {
                counter--;
            }
        }
    }

}