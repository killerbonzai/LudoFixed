using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class LudoBoard
    {
        public const int YELLOW = 0;
        public const int RED = 1;
        public const int BLUE = 2;
        public const int GREEN = 3;
        //private static  long serialVersionUID = 1L;
        private List<GameEndedListener> gameEndedListeners = new List<GameEndedListener>();
        private int currentColor;
        private Random rand;
        private bool brickMoved = true;
        private int dice = 0;
        private int turns = 0;
        private string debugString = "Welcome to this LUDO simulator...";
        //private BufferedImage boardImage, noDiceImage, dice1Image, dice2Image, dice3Image, dice4Image, dice5Image, dice6Image,
        //                redBrickImage1, redBrickImage2, redBrickImage3, redBrickImage4,
        //                blueBrickImage1, blueBrickImage2, blueBrickImage3, blueBrickImage4,
        //                yellowBrickImage1, yellowBrickImage2, yellowBrickImage3, yellowBrickImage4,
        //                greenBrickImage1, greenBrickImage2, greenBrickImage3, greenBrickImage4;
        private int[][] bricks = new int[][] { new int[] { 100, 101, 102, 103 }, new int[] { 200, 201, 202, 203 }, new int[] { 300, 301, 302, 303 }, new int[] { 400, 401, 402, 403 } };
        private int[] startFieldSquares = { 0, 13, 26, 39 };
        private int[] endFieldSquares = { 50, 11, 24, 37 };

        private int[] stars = { 5, 11, 18, 24, 31, 37, 44, 50 };
        private int[] globes = { 0, 8, 13, 21, 26, 34, 39, 47 };

        // painting stuff (UI stuff)
        private int[,,] startSquaresXYPos = { { { 91, 90 }, { 192, 90 }, { 90, 193 }, { 190, 193 } }, { { 563, 92 }, { 662, 88 }, { 564, 193 }, { 659, 194 } }, { { 563, 568 }, { 663, 568 }, { 562, 669 }, { 663, 669 } }, { { 93, 569 }, { 190, 568 }, { 91, 667 }, { 190, 668 } } };
        private int[,] fieldSquaresXYPos = { { 92, 328 }, { 140, 329 }, { 190, 330 }, { 236, 330 }, { 286, 331 }, { 329, 289 }, { 329, 238 }, { 329, 190 }, { 329, 141 }, { 327, 90 }, { 329, 38 }, { 376, 35 }, { 427, 35 }, { 428, 86 }, { 429, 136 }, { 427, 187 }, { 426, 235 }, { 427, 289 }, { 470, 330 }, { 514, 329 }, { 561, 330 }, { 611, 328 }, { 657, 332 }, { 713, 329 }, { 716, 380 }, { 715, 430 }, { 662, 431 }, { 616, 431 }, { 568, 431 }, { 516, 430 }, { 469, 430 }, { 431, 470 }, { 423, 516 }, { 431, 562 }, { 428, 621 }, { 430, 667 }, { 426, 718 }, { 376, 721 }, { 330, 722 }, { 326, 669 }, { 329, 622 }, { 331, 572 }, { 330, 517 }, { 328, 472 }, { 292, 435 }, { 240, 432 }, { 197, 436 }, { 145, 434 }, { 92, 436 }, { 40, 434 }, { 39, 384 }, { 42, 332 } };
        private int[,,] homeSquaresXYPos = { { { 90, 378 }, { 140, 380 }, { 192, 382 }, { 242, 380 }, { 288, 380 }, { 340, 380 } }, { { 378, 90 }, { 376, 140 }, { 378, 186 }, { 378, 240 }, { 380, 292 }, { 378, 344 } }, { { 666, 380 }, { 616, 378 }, { 568, 380 }, { 516, 378 }, { 466, 378 }, { 414, 376 } }, { { 380, 670 }, { 378, 618 }, { 380, 570 }, { 380, 516 }, { 380, 468 }, { 378, 422 } } };

        private static int boardSize = 750;
        private int brickSize = boardSize / 25;
        public bool killed = false;
        private ILudoPlayer yellowPlayer;
        private ILudoPlayer redPlayer;
        private ILudoPlayer bluePlayer;
        private ILudoPlayer greenPlayer;
        private int[] turnsCounter = { 0, 0, 0, 0 };
        private int point = 3;
        private int[] points = new int[4];
        private int startColor = YELLOW;
        public List<ILudoPlayer> GetPlayerList { get; private set; }

        public LudoBoard()
        {
            // do stuff...?
            FixGetPlayerList();
            reset();
        }

        private void FixGetPlayerList()
        {
            if (GetPlayerList == null)
            {
                GetPlayerList = new List<ILudoPlayer>();
                // fill list
                // add standard Players
                GetPlayerList.Add(new FifoLudoPlayer(this));
                GetPlayerList.Add(new RandomLudoPlayer(this));
                GetPlayerList.Add(new SemiSmartLudoPlayer(this));
                GetPlayerList.Add(new AggressiveLudoPlayer(this));
                //GetPlayerList.Add(new ManualLudoPlayer(this)); // not working atm

                // add custom ai/players
                // 'plugin' system?
            }
        }

        public void reset()
        {
            if (rand == null) rand = new Random();
            currentColor = startColor;
            turns = 3;
            yellowPlayer  = new RandomLudoPlayer(this);
            redPlayer = new RandomLudoPlayer(this);
            bluePlayer = new RandomLudoPlayer(this);
            greenPlayer = new RandomLudoPlayer(this);

            int[][] temp = { new int[] { 100, 101, 102, 103 }, new int[] { 200, 201, 202, 203 }, new int[] { 300, 301, 302, 303 }, new int[] { 400, 401, 402, 403 } };
            bricks = temp;
            int[] temp2 = { 0, 0, 0, 0 };
            turnsCounter = temp2;
            killed = false;
            point = 3;
            dice = 0;
            //repaint();
        }

        public void setPlayer(ILudoPlayer player, int color)
        {
            switch (color)
            {
                case YELLOW: yellowPlayer = player;
                    break;
                case RED: redPlayer = player;
                    break;
                case BLUE: bluePlayer = player;
                    break;
                case GREEN: greenPlayer = player;
                    break;
            }
        }

        public void setAllPlayers(ILudoPlayer yellow, ILudoPlayer red, ILudoPlayer blue, ILudoPlayer green)
        {
            yellowPlayer = yellow;
            redPlayer = red;
            bluePlayer = blue;
            greenPlayer = green;
        }

        #region paint stuff ++ fix stuff? need stuff?
        //public void paint(Graphics graphics)
        //{
        //    paintBoardImage(graphics);
        //    paintDice(graphics);
        //    paintBricks(graphics);
        //    paintTurn(graphics);
        //    paintDebugString(graphics);
        //}

        //private void paintDebugString(Graphics graphics)
        //{
        //    graphics.drawString(debugString, 732, 471);
        //}

        //private void paintTurn(Graphics graphics)
        //{
        //    int x = 760;
        //    int y = 200;
        //    switch (currentColor)
        //    {
        //        case RED:
        //            switch (turns)
        //            {
        //                case 1: graphics.drawImage(redBrickImage1, x, y, 100, 100, null); break;
        //                case 2: graphics.drawImage(redBrickImage2, x, y, 100, 100, null); break;
        //                case 3: graphics.drawImage(redBrickImage3, x, y, 100, 100, null); break;
        //                case 4: graphics.drawImage(redBrickImage4, x, y, 100, 100, null); break;
        //            }
        //            break;
        //        case GREEN:
        //            switch (turns)
        //            {
        //                case 1: graphics.drawImage(greenBrickImage1, x, y, 100, 100, null); break;
        //                case 2: graphics.drawImage(greenBrickImage2, x, y, 100, 100, null); break;
        //                case 3: graphics.drawImage(greenBrickImage3, x, y, 100, 100, null); break;
        //                case 4: graphics.drawImage(greenBrickImage4, x, y, 100, 100, null); break;
        //            }
        //            break;
        //        case BLUE:
        //            switch (turns)
        //            {
        //                case 1: graphics.drawImage(blueBrickImage1, x, y, 100, 100, null); break;
        //                case 2: graphics.drawImage(blueBrickImage2, x, y, 100, 100, null); break;
        //                case 3: graphics.drawImage(blueBrickImage3, x, y, 100, 100, null); break;
        //                case 4: graphics.drawImage(blueBrickImage4, x, y, 100, 100, null); break;
        //            }
        //            break;

        //        case YELLOW:
        //            switch (turns)
        //            {
        //                case 1: graphics.drawImage(yellowBrickImage1, x, y, 100, 100, null); break;
        //                case 2: graphics.drawImage(yellowBrickImage2, x, y, 100, 100, null); break;
        //                case 3: graphics.drawImage(yellowBrickImage3, x, y, 100, 100, null); break;
        //                case 4: graphics.drawImage(yellowBrickImage4, x, y, 100, 100, null); break;
        //            }
        //            break;
        //    }
        //}
        //private void paintBoardImage(Graphics graphics)
        //{
        //    graphics.drawImage(boardImage, 0, 0, boardSize, boardSize, null);
        //}

        //private void paintDice(Graphics graphics)
        //{
        //    if (!brickMoved)
        //    {
        //        switch (dice)
        //        {
        //            case 0: graphics.drawImage(noDiceImage, 760, 330, 100, 100, null); break;
        //            case 1: graphics.drawImage(dice1Image, 760, 330, 100, 100, null); break;
        //            case 2: graphics.drawImage(dice2Image, 760, 330, 100, 100, null); break;
        //            case 3: graphics.drawImage(dice3Image, 760, 330, 100, 100, null); break;
        //            case 4: graphics.drawImage(dice4Image, 760, 330, 100, 100, null); break;
        //            case 5: graphics.drawImage(dice5Image, 760, 330, 100, 100, null); break;
        //            case 6: graphics.drawImage(dice6Image, 760, 330, 100, 100, null); break;
        //        }
        //    }
        //    else
        //    {
        //        graphics.drawImage(noDiceImage, 760, 330, 100, 100, null);
        //    }
        //}

        //    private void paintBricks(Graphics graphics)
        //    {
        //        int[,] counter = new int[4,4];
        //        for (int i = 0; i < 4; i++)
        //        { //color
        //            for (int j = 0; j < 4; j++)
        //            { //reference brick
        //                //int ref = bricks[i][j];
        //                for (int k = 0; k < 4; k++)
        //                { //brick of same color
        //                    if (ref== bricks[i][k]) 
        //                    {
        //                        counter[i][j]++;
        //                    }
        //                }
        //            }
        //        }
        //  for(int i = 0; i<4;i++) 
        //        {
        //   switch(counter[0][i])  
        //   {
        //case 1: graphics.drawImage(yellowBrickImage1,index2Pixel(bricks[YELLOW][i], YELLOW)[0]-(brickSize/2), index2Pixel(bricks[YELLOW][i], YELLOW)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 2: graphics.drawImage(yellowBrickImage2,index2Pixel(bricks[YELLOW][i], YELLOW)[0]-(brickSize/2), index2Pixel(bricks[YELLOW][i], YELLOW)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 3: graphics.drawImage(yellowBrickImage3,index2Pixel(bricks[YELLOW][i], YELLOW)[0]-(brickSize/2), index2Pixel(bricks[YELLOW][i], YELLOW)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 4: graphics.drawImage(yellowBrickImage4,index2Pixel(bricks[YELLOW][i], YELLOW)[0]-(brickSize/2), index2Pixel(bricks[YELLOW][i], YELLOW)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //   }
        //   switch(counter[1][i])  
        //   {
        //case 1: graphics.drawImage(redBrickImage1,index2Pixel(bricks[RED][i], RED)[0]-(brickSize/2), index2Pixel(bricks[RED][i], RED)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 2: graphics.drawImage(redBrickImage2,index2Pixel(bricks[RED][i], RED)[0]-(brickSize/2), index2Pixel(bricks[RED][i], RED)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 3: graphics.drawImage(redBrickImage3,index2Pixel(bricks[RED][i], RED)[0]-(brickSize/2), index2Pixel(bricks[RED][i], RED)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 4: graphics.drawImage(redBrickImage4,index2Pixel(bricks[RED][i], RED)[0]-(brickSize/2), index2Pixel(bricks[RED][i], RED)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //   }
        //   switch(counter[2][i])  
        //   {
        //case 1: graphics.drawImage(blueBrickImage1,index2Pixel(bricks[BLUE][i], BLUE)[0]-(brickSize/2), index2Pixel(bricks[BLUE][i], BLUE)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 2: graphics.drawImage(blueBrickImage2,index2Pixel(bricks[BLUE][i], BLUE)[0]-(brickSize/2), index2Pixel(bricks[BLUE][i], BLUE)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 3: graphics.drawImage(blueBrickImage3,index2Pixel(bricks[BLUE][i], BLUE)[0]-(brickSize/2), index2Pixel(bricks[BLUE][i], BLUE)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 4: graphics.drawImage(blueBrickImage4,index2Pixel(bricks[BLUE][i], BLUE)[0]-(brickSize/2), index2Pixel(bricks[BLUE][i], BLUE)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //   }
        //   switch(counter[3][i])  
        //   {
        //case 1: graphics.drawImage(greenBrickImage1,index2Pixel(bricks[GREEN][i], GREEN)[0]-(brickSize/2), index2Pixel(bricks[GREEN][i], GREEN)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 2: graphics.drawImage(greenBrickImage2,index2Pixel(bricks[GREEN][i], GREEN)[0]-(brickSize/2), index2Pixel(bricks[GREEN][i], GREEN)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 3: graphics.drawImage(greenBrickImage3,index2Pixel(bricks[GREEN][i], GREEN)[0]-(brickSize/2), index2Pixel(bricks[GREEN][i], GREEN)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //case 4: graphics.drawImage(greenBrickImage4,index2Pixel(bricks[GREEN][i], GREEN)[0]-(brickSize/2), index2Pixel(bricks[GREEN][i], GREEN)[1]-(brickSize/2),brickSize,brickSize,null);break;
        //   }
        //  }
        // }
        // protected int[] index2Pixel(int index, int color)
        //    {

        //        /*Start Squares*/
        //        if (index >= 100 && index < 104) return startSquaresXYPos[color][index - 100];
        //        else if (index >= 200 && index < 204) return startSquaresXYPos[color][index - 200];
        //        else if (index >= 300 && index < 304) return startSquaresXYPos[color][index - 300];
        //        else if (index >= 400 && index < 404) return startSquaresXYPos[color][index - 400];

        //        /*Field Squares*/
        //        if (index < 52)
        //        {
        //            return fieldSquaresXYPos[index];
        //        }

        //        /*Home Squares*/
        //        if (index >= 104 && index < 110) return homeSquaresXYPos[color][index - 104];
        //        else if (index >= 204 && index < 210) return homeSquaresXYPos[color][index - 204];
        //        else if (index >= 304 && index < 310) return homeSquaresXYPos[color][index - 304];
        //        else if (index >= 404 && index < 410) return homeSquaresXYPos[color][index - 404];

        //        System.out.println("Index ikke genkendt " + index);
        //        return null;
        //    } 
        #endregion

        public int rollDice()
        {
            if (brickMoved || nothingToDo())
            {
                //dice = Math.Abs(rand.Next()) % 6 + 1;
                dice = rand.Next(1, 7);
                brickMoved = false;
                turns--;
                if (!nothingToDo())
                {
                    print("Now move a brick...");
                }
                else
                {
                    if (turns > 0) print("No brick can be moved, roll the dice again..");
                    else turns = 0;
                }
                //repaint();
            }
            else
            {
                print("You must move a brick before you rool the dice");
            }
            return dice;
        }

        public bool nothingToDo()
        {
            return !moveable(0) && !moveable(1) && !moveable(2) && !moveable(3);
        }

        public bool moveable(int nr)
        {
            if (inStartArea(bricks[currentColor][nr], currentColor) && dice != 6)
            {
                return false;
            }
            if (atHome(bricks[currentColor][nr], currentColor))
            {
                return false;
            }
            return true;
        }

        public bool isDone(int color)
        {
            return atHome(bricks[color][0], color) && atHome(bricks[color][1], color) && atHome(bricks[color][2], color) && atHome(bricks[color][3], color);
        }

        public int getDice()
        {
            return dice;
        }

        public bool moveBrick(int nr)
        {
            if (nr < 0 || nr > 3) throw new Exception("Not valid brick nr: " + nr);
            if (brickMoved)
            {
                print("You must roll the dice before moving");
                return false;
            }
            int index = bricks[currentColor][nr];
            /*Moving out of the start area*/
            if (inStartArea(index, currentColor))
            {
                if (dice == 6)
                {
                    //animateMove(currentColor, nr, bricks[currentColor][nr], startFieldSquares[currentColor]);
                    bricks[currentColor][nr] = startFieldSquares[currentColor];
                    //				Hit opponent home from "my" globe
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (bricks[i][j] == bricks[currentColor][nr] && i != currentColor)
                            {
                                //animateMove(i, j, bricks[i][j], (i + 1) * 100 + j);
                                bricks[i][j] = (i + 1) * 100 + j;
                            }
                        }
                    }
                    brickMoved = true;
                    turns = 1;
                    print("Now roll the dice...");
                    return true;
                }
                else
                {
                    print("Unable to move the selected brick...");
                    return false; //Illigal move
                }
            }
            else if (!(index <= endFieldSquares[currentColor] && (index + dice) > endFieldSquares[currentColor]) && index < 100) /*Moving on the field*/
            {
                int moveToIndex = (index + dice) % 52;
                //animateMove(currentColor, nr, bricks[currentColor][nr], moveToIndex);

                //check for stars
                if (moveToIndex != endFieldSquares[currentColor] && isStar(moveToIndex))
                {
                    //animateMove(currentColor,nr,bricks[currentColor][nr],moveToIndex);
                    //animateMove(currentColor, nr, moveToIndex, nextStar(moveToIndex));
                    moveToIndex = nextStar(moveToIndex);
                }
                else if (moveToIndex == endFieldSquares[currentColor])
                {
                    //animateMove(currentColor,nr,bricks[currentColor][nr],moveToIndex);
                    //animateMove(currentColor, nr, moveToIndex, (currentColor + 1) * 100 + 9);
                    moveToIndex = (currentColor + 1) * 100 + 9;
                }
                //check for home hitting
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (bricks[i][j] == moveToIndex && i != currentColor)
                        {
                            //someone needs to be hit home
                            if (isGlobe(moveToIndex))
                            {//he is on a globe
                                //animateMove(currentColor, nr, moveToIndex, (currentColor + 1) * 100 + nr);
                                moveToIndex = (currentColor + 1) * 100 + nr; //I am hit home
                            }
                            else if (countBricksOn(moveToIndex) >= 2)
                            { //there are at least two bricks
                                //animateMove(currentColor, nr, moveToIndex, (currentColor + 1) * 100 + nr);
                                moveToIndex = (currentColor + 1) * 100 + nr; //I am hit home
                            }
                            else
                            { // I hit him home
                                //animateMove(i, j, bricks[i][j], (i + 1) * 100 + j);
                                bricks[i][j] = (i + 1) * 100 + j;
                            }
                        }
                    }
                }
                bricks[currentColor][nr] = moveToIndex;
                brickMoved = true;
                if (dice == 6) turns = 1;
                else turns = 0;
                print("Now roll the dice...");
                return true;
            }
            else
            { /*Moving in(to) the home area*/
                if (index == ((1 + currentColor) * 100 + 9))
                {
                    print("Unable to move the selected brick...");
                    return false; //Illigal move already home
                }
                if (index < 51)
                { /*still on the field*/
                    //animateMove(currentColor, nr, bricks[currentColor][nr], (index + dice) - (1 + endFieldSquares[currentColor]) + ((1 + currentColor) * 100 + 4));
                    bricks[currentColor][nr] = (index + dice) - (1 + endFieldSquares[currentColor]) + ((1 + currentColor) * 100 + 4);
                }
                else
                {
                    if ((index + dice) > ((1 + currentColor) * 100 + 9))
                    {
                        //animateMove(currentColor, nr, bricks[currentColor][nr], 2 * ((1 + currentColor) * 100 + 9) - (index + dice));
                        bricks[currentColor][nr] = 2 * ((1 + currentColor) * 100 + 9) - (index + dice);
                    }
                    else
                    {
                        //animateMove(currentColor, nr, bricks[currentColor][nr], (index + dice));
                        bricks[currentColor][nr] = (index + dice);
                    }
                }
                brickMoved = true;
                if (dice == 6)
                {
                    turns = 1;
                    print("Now roll the dice...");
                }
                else
                {
                    turns = 0;
                    print("Next Player...");
                }
                return true;
            }
        }

        //private int steps = 25;
        //private float waitTime = 1000 / steps;
        //private synchronized void animateMove(int color, int nr, int fromIndex, int toIndex)
        //{
        //    if (!LUDO.visual) return;
        //    float startXPixel = (float)index2Pixel(fromIndex, color)[0] - (brickSize / 2);
        //    float startYPixel = (float)index2Pixel(fromIndex, color)[1] - (brickSize / 2);
        //    float endXPixel = (float)index2Pixel(toIndex, color)[0] - (brickSize / 2);
        //    float endYPixel = (float)index2Pixel(toIndex, color)[1] - (brickSize / 2);

        //    /*float dist = (startXPixel-endXPixel)*(startXPixel-endXPixel)+(startYPixel-endYPixel)*(startYPixel-endYPixel);
        //    float waitTime = dist/20000;
        //    if(waitTime<1) {
        //        System.out.println("wait time="+waitTime+" steps="+steps);
        //        steps= 50;
        //        waitTime = 1; 
        //    }
        //    System.out.println("Dist="+dist);*/
        //    for (float i = 0; i < steps; i++)
        //    {
        //        int x = (int)((endXPixel - startXPixel) * i / steps + startXPixel);
        //        int y = (int)((endYPixel - startYPixel) * i / steps + startYPixel);
        //        if (color == YELLOW) getGraphics().drawImage(yellowBrickImage1, x, y, brickSize, brickSize, null);
        //        if (color == RED) getGraphics().drawImage(redBrickImage1, x, y, brickSize, brickSize, null);
        //        if (color == BLUE) getGraphics().drawImage(blueBrickImage1, x, y, brickSize, brickSize, null);
        //        if (color == GREEN) getGraphics().drawImage(greenBrickImage1, x, y, brickSize, brickSize, null);

        //        //repaint();
        //        try
        //        {
        //            wait((long)waitTime);
        //        }
        //        catch (InterruptedException e)
        //        {
        //            e.printStackTrace();
        //        }
        //    }
        //}

        public int[][] getNewBoardState(int nr, int color, int dice2)
        {
            int new_turns = -1;
            int[][] new_bricks_state = getBoardState();
            int index = new_bricks_state[color][nr];

            /*Moving out of the start area*/
            if (inStartArea(index, color))
            {
                if (dice2 == 6)
                {
                    new_bricks_state[color][nr] = startFieldSquares[color];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (new_bricks_state[i][j] == new_bricks_state[color][nr] && i != color)
                            {
                                new_bricks_state[i][j] = (i + 1) * 100 + j; //Hit opponent home from "my" globe
                            }
                        }
                    }
                    new_turns = 1;
                    return new_bricks_state;
                }
                else
                {
                    return new_bricks_state; //Unable to move the selected brick... Illigal move
                }
            }
            else if (!(index <= endFieldSquares[color] && (index + dice2) > endFieldSquares[color]) && index < 100) /*Moving on the field*/
            {
                int moveToIndex = (index + dice2) % 52;
                //check for stars
                if (moveToIndex != endFieldSquares[color] && isStar(moveToIndex)) moveToIndex = nextStar(moveToIndex);
                else if (moveToIndex == endFieldSquares[color]) moveToIndex = (color + 1) * 100 + 9;
                //check for home hitting
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (new_bricks_state[i][j] == moveToIndex && i != color)
                        {
                            //someone needs to be hit home
                            if (isGlobe(moveToIndex))
                            {//he is on a globe 
                                moveToIndex = (color + 1) * 100 + nr; //I am hit home
                            }
                            else if (countBricksOn(moveToIndex) >= 2)
                            { //there are at least two bricks
                                moveToIndex = (color + 1) * 100 + nr; //I am hit home
                            }
                            else
                            { // I hit him home
                                new_bricks_state[i][j] = (i + 1) * 100 + j;
                            }
                        }
                    }
                }
                new_bricks_state[color][nr] = moveToIndex;
                if (dice2 == 6) new_turns = 1;
                else new_turns = 0;
                return new_bricks_state;
            }
            else
            { /*Moving in(to) the home area*/
                if (index == ((1 + color) * 100 + 9))
                {
                    return new_bricks_state; //Unable to move the selected brick... illigal move already home
                }
                if (index < 51)
                { /*still on the field*/
                    new_bricks_state[color][nr] = (index + dice2) - (1 + endFieldSquares[color]) + ((1 + color) * 100 + 4);
                }
                else
                {
                    if ((index + dice2) > ((1 + color) * 100 + 9))
                    {
                        new_bricks_state[color][nr] = 2 * ((1 + color) * 100 + 9) - (index + dice2);
                    }
                    else new_bricks_state[color][nr] = (index + dice2);
                }
                if (dice2 == 6)
                {
                    new_turns = 1;
                }
                else
                {
                    new_turns = 0;
                }
                return new_bricks_state;
            }
        }

        public int[][] getNewBoardState(int[][] boardState, int nr, int color, int dice2)
        {
            int new_turns = -1;
            int[][] new_bricks_state = (int[][])boardState.Clone();
            for (int i = 0; i < new_bricks_state.Length; ++i)
            {
                new_bricks_state[i] = (int[])boardState[i].Clone();
            }
            int index = new_bricks_state[color][nr];

            /*Moving out of the start area*/
            if (inStartArea(index, color))
            {
                if (dice2 == 6)
                {
                    new_bricks_state[color][nr] = startFieldSquares[color];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (new_bricks_state[i][j] == new_bricks_state[color][nr] && i != color)
                            {
                                new_bricks_state[i][j] = (i + 1) * 100 + j; //Hit opponent home from "my" globe
                            }
                        }
                    }
                    new_turns = 1;
                    return new_bricks_state;
                }
                else
                {
                    return new_bricks_state; //Unable to move the selected brick... Illigal move
                }
            }
            else if (!(index <= endFieldSquares[color] && (index + dice2) > endFieldSquares[color]) && index < 100) /*Moving on the field*/
            {
                int moveToIndex = (index + dice2) % 52;
                //check for stars
                if (moveToIndex != endFieldSquares[color] && isStar(moveToIndex)) moveToIndex = nextStar(moveToIndex);
                else if (moveToIndex == endFieldSquares[color]) moveToIndex = (color + 1) * 100 + 9;
                //check for home hitting
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (new_bricks_state[i][j] == moveToIndex && i != color)
                        {
                            //someone needs to be hit home
                            if (isGlobe(moveToIndex))
                            {//he is on a globe 
                                moveToIndex = (color + 1) * 100 + nr; //I am hit home
                            }
                            else if (countBricksOn(boardState, moveToIndex) >= 2)
                            { //there are at least two bricks
                                moveToIndex = (color + 1) * 100 + nr; //I am hit home
                            }
                            else
                            { // I hit him home
                                new_bricks_state[i][j] = (i + 1) * 100 + j;
                            }
                        }
                    }
                }
                new_bricks_state[color][nr] = moveToIndex;
                if (dice2 == 6) new_turns = 1;
                else new_turns = 0;
                return new_bricks_state;
            }
            else
            { /*Moving in(to) the home area*/
                if (index == ((1 + color) * 100 + 9))
                {
                    return new_bricks_state; //Unable to move the selected brick... illigal move already home
                }
                if (index < 51)
                { /*still on the field*/
                    new_bricks_state[color][nr] = (index + dice2) - (1 + endFieldSquares[color]) + ((1 + color) * 100 + 4);
                }
                else
                {
                    if ((index + dice2) > ((1 + color) * 100 + 9))
                    {
                        new_bricks_state[color][nr] = 2 * ((1 + color) * 100 + 9) - (index + dice2);
                    }
                    else new_bricks_state[color][nr] = (index + dice2);
                }
                if (dice2 == 6)
                {
                    new_turns = 1;
                }
                else
                {
                    new_turns = 0;
                }
                return new_bricks_state;
            }
        }

        private int countBricksOn(int[][] boardState, int index)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (boardState[i][j] == index) count++;
                }
            }
            return count;
        }

        private int countBricksOn(int index)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (bricks[i][j] == index) count++;
                }
            }
            return count;
        }

        private int nextStar(int index)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] == index) return stars[(i + 1) % stars.Length];
            }
            return 0;
        }

        public int[][] getBoardState()
        {
            int[][] bs = new int[4][] { new int[4] { 0, 0, 0, 0 }, new int[4] { 0, 0, 0, 0 }, new int[4] { 0, 0, 0, 0 }, new int[4] { 0, 0, 0, 0 } };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bs[i][j] = bricks[i][j];
                }
            }
            return bs;
        }

        public bool inStartArea(int index, int color)
        {
            return index >= (color + 1) * 100 && index <= (((color + 1) * 100) + 3);
        }

        public bool atHome(int index, int color)
        {
            return index == ((color + 1) * 100 + 9);
        }

        public bool almostHome(int index, int color)
        {
            return index >= ((color + 1) * 100 + 4) && index < ((color + 1) * 100 + 9);
        }

        public bool atField(int index)
        {
            return index < 100;
        }

        public bool isGlobe(int index)
        {
            for (int i = 0; i < globes.Length; i++)
            {
                if (globes[i] == index) return true;
            }
            return false;
        }

        public bool isStar(int index)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] == index) return true;
            }
            return false;
        }

        public void print(String str)
        {
            debugString = currentColor + ": " + str;
            //repaint();
        }

        private bool inPlay(int index, int color)
        {
            return !atHome(index, color) && !inStartArea(index, color);
        }

        private void nextPlayer()
        {
            currentColor = (currentColor + 1) % 4;
            if (!inPlay(bricks[currentColor][0], currentColor) &&
                    !inPlay(bricks[currentColor][1], currentColor) &&
                    !inPlay(bricks[currentColor][2], currentColor) &&
                    !inPlay(bricks[currentColor][3], currentColor))
            {
                turns = 3;
            }
            else
            {
                turns = 1;
            }
            brickMoved = true;
            dice = 0;
            //repaint();
        }

        public int[] getMyBricks()
        {
            int[] b = new int[4];
            b[0] = bricks[currentColor][0];
            b[1] = bricks[currentColor][1];
            b[2] = bricks[currentColor][2];
            b[3] = bricks[currentColor][3];
            return b;
        }

        public int getMyColor()
        {
            return currentColor;
        }

        public int[] getPoints()
        {
            return points;
        }

        public int getTurns()
        {
            return turns;
        }

        public void kill()
        {
            killed = true;
        }

        public void play() //throws InterruptedException
        {
		    while(!killed)
            {
                while (turns > 0)
                {
                    if (killed) return;
                    if (currentColor == YELLOW)
                    {
                        if (!isDone(YELLOW))
                        {
                            yellowPlayer.Play();
                            turnsCounter[YELLOW]++;
                            if (isDone(YELLOW))
                            {
                                points[YELLOW] = point;
                                point--;
                            }
                        }
                        else
                        {
                            turns = 0;
                        }
                    }
                    else if (currentColor == RED)
                    {
                        if (!isDone(RED))
                        {
                            redPlayer.Play();
                            turnsCounter[RED]++;
                            if (isDone(RED))
                            {
                                points[RED] = point;
                                point--;
                            }
                        }
                        else
                        {
                            turns = 0;
                        }
                    }
                    else if (currentColor == BLUE)
                    {
                        if (!isDone(BLUE))
                        {
                            bluePlayer.Play();
                            turnsCounter[BLUE]++;
                            if (isDone(BLUE))
                            {
                                points[BLUE] = point;
                                point--;
                            }
                        }
                        else
                        {
                            turns = 0;
                        }
                    }
                    else if (currentColor == GREEN)
                    {
                        if (!isDone(GREEN))
                        {
                            greenPlayer.Play();
                            turnsCounter[GREEN]++;
                            if (isDone(GREEN))
                            {
                                points[GREEN] = point;
                                point--;
                            }
                        }
                        else
                        {
                            turns = 0;
                        }
                    }
                }
                nextPlayer();
                print("Rool the dice");
                if (isDone(YELLOW) && isDone(RED) && isDone(BLUE) && isDone(GREEN))
                {
                    //System.out.print("{"+turnsCounter[YELLOW]+", "+turnsCounter[RED]+", "+turnsCounter[BLUE]+", "+turnsCounter[GREEN]+"},");
                    //System.out.print("{"+points[YELLOW]+", "+points[RED]+", "+points[BLUE]+", "+points[GREEN]+"},");
                    for (int i = 0; i < gameEndedListeners.Count; i++)
                    {
                        gameEndedListeners[i].gameEnded(points);
                    }
                    return;
                }
            }
        }
        public void addGameEndedListener(GameEndedListener listener)
        {
            gameEndedListeners.Add(listener);
        }
        public void removeGameEndedListener(GameEndedListener listener)
        {
            gameEndedListeners.Remove(listener);
        }
        public static void trap()
        {
            try
            {
                //System.in.read();
                //System.in.read();
            }
            catch (Exception)
            {
                throw;
                //e.printStackTrace();
            }
        }
    }
}
