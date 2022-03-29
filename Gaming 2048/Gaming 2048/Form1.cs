using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaming_2048
{

    /*2048
     1.進行移動
     2.相同相加,不相同停止相加移動方變為0
     3.剩餘0的位置隨機生成2或者4
         */

    public partial class Form1 : Form
    {        
    
        //測試更新
         //測試更新
          //測試更新
                  //測試更新
         //測試更新
          //測試更新
    
        int BlankState = -1;                                            //沒有圖片的狀態顯示   
        int[,] BlockNumber = new int[4, 4];                             //方塊資訊
        PictureBox[,] GamePictureArray = new PictureBox[4, 4];
        Timer[] timer = new Timer[16];                                  //16個計時器
        Point[] BlankPoint = new Point[16];                             //空白的PictureBox
        bool KeyboradEnable = false;                                    //讓鍵盤當下不能動
        int offset = 25;                                                //每次移動的偏移量


        Bitmap two = Properties.Resources._2;
        Bitmap four = Properties.Resources._4;
        Bitmap eight = Properties.Resources._8;
        Bitmap sixteen = Properties.Resources._16;
        Bitmap thirtyTwo = Properties.Resources._32;
        Bitmap sixtyFour = Properties.Resources._64;
        Bitmap oneHoundredTwentyEight = Properties.Resources._128;
        Bitmap twoHoundredFiftySIx = Properties.Resources._256;
        Bitmap fiveHoundredtwelve = Properties.Resources._512;
        Bitmap oneThoundredTwentyFour = Properties.Resources._1024;
        Bitmap twoThoundredFourtyEight = Properties.Resources._2048;

        public Form1()
        {
            InitializeComponent();
            InitlizeGame();
               
        }
        public void InitlizeGame()
        { 
         for (int  x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    PictureBox GamePicture = new PictureBox();
                    GamePicture.Location = new Point(75 * y + 6, 75 * x + 6); //間距空間
                    GamePicture.Size = new Size(70, 70);
                    GamePicture.BorderStyle = BorderStyle.FixedSingle;
                    GamePicture.SizeMode= PictureBoxSizeMode.StretchImage;
                    GamePicture.BackColor = Color.Gray;
                    GamePictureArray[x, y] = GamePicture;          //標記轉換二維轉換一維
                    PanelBoard.Controls.Add(GamePicture);
                }
            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y <4; y++)
                {
                    BlockNumber[x, y] = BlankState;
                }
            }
            PanelBoard.SendToBack();
            KeyboradEnable = true;
            CreatImage();
            CreatImage();
            

        }


        //產生相對物品
        private Bitmap ImageList(int number)
        {
            switch (number)
            {
                case 2:
                    return two;
                case 4:
                    return four;
                case 8:
                    return eight;
                case 16:
                    return sixteen;
                case 32:
                    return thirtyTwo;
                case 64:
                    return sixtyFour;
                case 128:
                    return oneHoundredTwentyEight;
                case 256:
                    return twoHoundredFiftySIx;
                case 512:
                    return fiveHoundredtwelve;
                case 1024:
                    return oneThoundredTwentyFour;
                case 2048:
                    return twoThoundredFourtyEight;
                default:
                    return null;
            }
        }

        //重複複寫
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (KeyboradEnable)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        KeyUp();
                        OpenAllTimerAndBeginWatch();
                        IsGameEnd();
                        break;
                    case Keys.Down:
                        KeyDown();
                        OpenAllTimerAndBeginWatch();
                        IsGameEnd();
                        break;
                    case Keys.Left:
                        KeyLeft();
                        OpenAllTimerAndBeginWatch();
                        IsGameEnd();
                        break;
                    case Keys.Right:
                        KeyRight();
                        OpenAllTimerAndBeginWatch();
                        IsGameEnd();
                        break;
                } 
            }
            return base.ProcessDialogKey(keyData);
        }

        //移動圖片的過程動畫
        private void MoveAnimation(Point start,Point end, int direction , bool isEmpty)   //起點,終點,移動方向0=上 1=下 2=左 3=右 ,移動後是否為空
        {
            if (direction==0||direction==1)   //座標相等不產生動作
            {
                if (start.X==end.X)
                {
                    return;
                }
            }
            if (direction==2 || direction==3)
            {
                if (start.Y==end.Y)
                {
                    return;
                }
            }

            PictureBox newPictureBox = new PictureBox();
            newPictureBox.Size = GamePictureArray[start.X, start.Y].Size;
            newPictureBox.Location = GamePictureArray[start.X, start.Y].Location;
            newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            newPictureBox.Image = GamePictureArray[start.X, start.Y].Image;
            GamePictureArray[start.X, start.Y].Image = null;   //預設為空
            this.PanelBoard.Controls.Add(newPictureBox);
            newPictureBox.BringToFront();
            timer[start.X * 4 + start.Y] = new Timer();
            timer[start.X * 4 + start.Y].Interval = 18;

            if (direction==0)            //向上運動
            {
                if (isEmpty==true)
                {
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Top <= GamePictureArray[end.X, end.Y].Top)
                        {
                            GamePictureArray[end.X, end.Y].Image = newPictureBox.Image;
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Top -= offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y];
                    BlockNumber[start.X, start.Y] = BlankState;

                }
                else
                {
                    int old = BlockNumber[start.X, start.Y];
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Top <= GamePictureArray[end.X, end.Y].Top)
                        {
                            GamePictureArray[end.X, end.Y].Image = ImageList(old * 2);
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Top -= offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y] * 2;
                    BlockNumber[start.X, start.Y] = BlankState;
                }
            }
            else if (direction == 1)            //向下運動
            {
                if (isEmpty == true)
                {
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Top >= GamePictureArray[end.X, end.Y].Top)
                        {
                            GamePictureArray[end.X, end.Y].Image = newPictureBox.Image;
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Top += offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y];
                    BlockNumber[start.X, start.Y] = BlankState;

                }
                else
                {
                    int old = BlockNumber[start.X, start.Y];
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Top >= GamePictureArray[end.X, end.Y].Top)
                        {
                            GamePictureArray[end.X, end.Y].Image = ImageList(old * 2);
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Top += offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y] * 2;
                    BlockNumber[start.X, start.Y] = BlankState;
                }
            }
            else if (direction == 2)            //向左運動
            {
                if (isEmpty == true)
                {
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Left <= GamePictureArray[end.X, end.Y].Left)
                        {
                            GamePictureArray[end.X, end.Y].Image = newPictureBox.Image;
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Left -= offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y];
                    BlockNumber[start.X, start.Y] = BlankState;

                }
                else
                {
                    int old = BlockNumber[start.X, start.Y];
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Left <= GamePictureArray[end.X, end.Y].Left)
                        {
                            GamePictureArray[end.X, end.Y].Image = ImageList(old * 2);
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Left -= offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y] * 2;
                    BlockNumber[start.X, start.Y] = BlankState;
                }
            }
            else            //向右運動
            {
                if (isEmpty == true)
                {
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Left >= GamePictureArray[end.X, end.Y].Left)
                        {
                            GamePictureArray[end.X, end.Y].Image = newPictureBox.Image;
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Left += offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y];
                    BlockNumber[start.X, start.Y] = BlankState;

                }
                else
                {
                    int old = BlockNumber[start.X, start.Y];
                    timer[start.X * 4 + start.Y].Tick += delegate (object sender, EventArgs e)
                    {
                        if (newPictureBox.Left >= GamePictureArray[end.X, end.Y].Left)
                        {
                            GamePictureArray[end.X, end.Y].Image = ImageList(old * 2);
                            newPictureBox.Dispose();
                            timer[start.X * 4 + start.Y].Stop();
                            timer[start.X * 4 + start.Y].Dispose();
                            timer[start.X * 4 + start.Y] = null;
                        }
                        else
                        {
                            newPictureBox.Left += offset;
                        }
                    };
                    BlockNumber[end.X, end.Y] = BlockNumber[start.X, start.Y] * 2;
                    BlockNumber[start.X, start.Y] = BlankState;
                }
            }
        }

      

        private void OpenAllTimerAndBeginWatch()
        {
            bool allNull = true;
            for (int i = 0; i < 16; i++)
            {
                if (timer[i] != null)
                {
                    allNull = false;
                }
            }
            if (allNull)
            {
                return;
            }

            KeyboradEnable = false;
            TimerWatch.Start();
            for (int i = 0; i < 16; i++)
            {
                if (timer[i]!=null)
                {
                    timer[i].Start();
                }
            }
            
        }

        
        //上下左右四個方向判斷
        private new void KeyUp()
        {
            int emptyNumber = 0;
            for (int j = 0; j < 4; j++)
            {
                emptyNumber = CountEmptyNumber(j, 1);
                if (emptyNumber==4)
                {
                    continue;
                }
                else if (emptyNumber==3)
                {
                    if (BlockNumber[0,j]==BlankState)
                    {
                        int index = FindWhichBlockNotEmpty(j, 1);
                        MoveAnimation(new Point(index, j), new Point(0, j), 0, true);
                    }
                }
                else if (emptyNumber==2)
                {
                    Point p = FindWhichTwoBlockEmpty(j, 1);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(p.X);
                    nums.Remove(p.Y);
                    int a = nums[0];
                    int b = nums[1];
                    if (BlockNumber[a, j] == BlockNumber[b, j])
                    {
                        MoveAnimation(new Point(a, j), new Point(0, j), 0, true);
                        MoveAnimation(new Point(b, j), new Point(0, j), 0, false);
                    }
                    else
                    {
                        MoveAnimation(new Point(a, j), new Point(0, j), 0, true);
                        MoveAnimation(new Point(b, j), new Point(1, j), 0, true);
                    }
                }
                else if (emptyNumber == 1)
                {
                    int index = FindWhichBlockEmpty(j, 1);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(index);
                    int a = nums[0];
                    int b = nums[1];
                    int c = nums[2];
                    if (BlockNumber[a, j] == BlockNumber[b, j])
                    {
                        MoveAnimation(new Point(a, j), new Point(0, j), 0, true);
                        MoveAnimation(new Point(b, j), new Point(0, j), 0, false);
                        MoveAnimation(new Point(c, j), new Point(1, j), 0, true);
                    }
                    else
                    {
                        if (BlockNumber[b, j] == BlockNumber[c, j])
                        {
                            MoveAnimation(new Point(a, j), new Point(0, j), 0, true);
                            MoveAnimation(new Point(b, j), new Point(1, j), 0, true);
                            MoveAnimation(new Point(c, j), new Point(1, j), 0, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(a, j), new Point(0, j), 0, true);
                            MoveAnimation(new Point(b, j), new Point(1, j), 0, true);
                            MoveAnimation(new Point(c, j), new Point(2, j), 0, true);
                        }
                    }
                }
                else
                {
                    if (BlockNumber[0, j] == BlockNumber[1, j])
                    {
                        if (BlockNumber[2, j] == BlockNumber[3, j])
                        {
                            MoveAnimation(new Point(1, j), new Point(0, j), 0, false);
                            MoveAnimation(new Point(2, j), new Point(1, j), 0, true);
                            MoveAnimation(new Point(3, j), new Point(1, j), 0, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(1, j), new Point(0, j), 0, false);
                            MoveAnimation(new Point(2, j), new Point(1, j), 0, true);
                            MoveAnimation(new Point(3, j), new Point(2, j), 0, true);
                        }
                    }
                    else
                    {
                        if (BlockNumber[1, j] == BlockNumber[2, j])
                        {
                            MoveAnimation(new Point(2, j), new Point(1, j), 0, false);
                            MoveAnimation(new Point(3, j), new Point(2, j), 0, true);
                        }
                        else
                        {
                            if (BlockNumber[2, j] == BlockNumber[3, j])
                            {
                                MoveAnimation(new Point(3, j), new Point(2, j), 0, false);
                            }
                        }
                    }
                }



            }
        }
        //向下運動
        private new void KeyDown()
        {
            int emptyNumber = 0;
            for (int j = 0; j < 4; j++)
            {
                emptyNumber = CountEmptyNumber(j, 1);
                if (emptyNumber == 4)
                {
                    continue;
                }
                else if (emptyNumber == 3)
                {
                    if (BlockNumber[3, j] == BlankState)
                    {
                        int index = FindWhichBlockNotEmpty(j, 1);
                        MoveAnimation(new Point(index, j), new Point(3, j), 1, true);
                    }
                }
                else if (emptyNumber == 2)
                {
                    Point p = FindWhichTwoBlockEmpty(j, 1);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(p.X);
                    nums.Remove(p.Y);
                    int a = nums[0];
                    int b = nums[1];
                    if (BlockNumber[a, j] == BlockNumber[b, j])
                    {
                        MoveAnimation(new Point(b, j), new Point(3, j), 1, true);
                        MoveAnimation(new Point(a, j), new Point(3, j), 1, false);
                    }
                    else
                    {
                        MoveAnimation(new Point(b, j), new Point(3, j), 1, true);
                        MoveAnimation(new Point(a, j), new Point(2, j), 1, true);
                    }
                }
                else if (emptyNumber == 1)
                {
                    int index = FindWhichBlockEmpty(j, 1);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(index);
                    int a = nums[0];
                    int b = nums[1];
                    int c = nums[2];
                    if (BlockNumber[b, j] == BlockNumber[c, j])
                    {
                        MoveAnimation(new Point(c, j), new Point(3, j), 1, true);
                        MoveAnimation(new Point(b, j), new Point(3, j), 1, false);
                        MoveAnimation(new Point(a, j), new Point(2, j), 1, true);
                    }
                    else
                    {
                        if (BlockNumber[a, j] == BlockNumber[b, j])
                        {
                            MoveAnimation(new Point(c, j), new Point(3, j), 1, true);
                            MoveAnimation(new Point(b, j), new Point(2, j), 1, true);
                            MoveAnimation(new Point(a, j), new Point(2, j), 1, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(c, j), new Point(3, j), 1, true);
                            MoveAnimation(new Point(b, j), new Point(2, j), 1, true);
                            MoveAnimation(new Point(a, j), new Point(1, j), 1, true);
                        }
                    }
                }
                else
                {
                    if (BlockNumber[2, j] == BlockNumber[3, j])
                    {
                        if (BlockNumber[1, j] == BlockNumber[0, j])
                        {
                            MoveAnimation(new Point(2, j), new Point(3, j), 1, false);
                            MoveAnimation(new Point(1, j), new Point(2, j), 1, true);
                            MoveAnimation(new Point(0, j), new Point(2, j), 1, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(2, j), new Point(3, j), 1, false);
                            MoveAnimation(new Point(1, j), new Point(2, j), 1, true);
                            MoveAnimation(new Point(0, j), new Point(1, j), 1, true);
                        }
                    }
                    else
                    {
                        if (BlockNumber[1, j] == BlockNumber[2, j])
                        {
                            MoveAnimation(new Point(1, j), new Point(2, j), 1, false);
                            MoveAnimation(new Point(0, j), new Point(1, j), 1, true);
                        }
                        else
                        {
                            if (BlockNumber[1, j] == BlockNumber[0, j])
                            {
                                MoveAnimation(new Point(0, j), new Point(1, j), 1, false);
                            }
                        }
                    }
                }
            }
        }
        //向左運動
        private void KeyLeft()
        {
            int emptyNumber = 0;
            for (int i = 0; i < 4; i++)
            {
                emptyNumber = CountEmptyNumber(i, 0);
                if (emptyNumber == 4)
                    continue;
                else if (emptyNumber == 3)
                {
                    if (BlockNumber[i, 0] == BlankState)
                    {
                        int index = FindWhichBlockNotEmpty(i, 0);
                        MoveAnimation(new Point(i, index), new Point(i, 0), 2, true);
                    }
                }
                else if (emptyNumber == 2)
                {
                    Point p = FindWhichTwoBlockEmpty(i, 0);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(p.X);
                    nums.Remove(p.Y);
                    int a = nums[0];
                    int b = nums[1];
                    if (BlockNumber[i, a] == BlockNumber[i, b])
                    {
                        MoveAnimation(new Point(i, a), new Point(i, 0), 2, true);
                        MoveAnimation(new Point(i, b), new Point(i, 0), 2, false);
                    }
                    else
                    {
                        MoveAnimation(new Point(i, a), new Point(i, 0), 2, true);
                        MoveAnimation(new Point(i, b), new Point(i, 1), 2, true);
                    }
                }
                else if (emptyNumber == 1)
                {
                    int index = FindWhichBlockEmpty(i, 0);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(index);
                    int a = nums[0];
                    int b = nums[1];
                    int c = nums[2];
                    if (BlockNumber[i, a] == BlockNumber[i, b])
                    {
                        MoveAnimation(new Point(i, a), new Point(i, 0), 2, true);
                        MoveAnimation(new Point(i, b), new Point(i, 0), 2, false);
                        MoveAnimation(new Point(i, c), new Point(i, 1), 2, true);
                    }
                    else
                    {
                        if (BlockNumber[i, b] == BlockNumber[i, c])
                        {
                            MoveAnimation(new Point(i, a), new Point(i, 0), 2, true);
                            MoveAnimation(new Point(i, b), new Point(i, 1), 2, true);
                            MoveAnimation(new Point(i, c), new Point(i, 1), 2, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(i, a), new Point(i, 0), 2, true);
                            MoveAnimation(new Point(i, b), new Point(i, 1), 2, true);
                            MoveAnimation(new Point(i, c), new Point(i, 2), 2, true);
                        }
                    }
                }
                else
                {
                    if (BlockNumber[i, 0] == BlockNumber[i, 1])
                    {
                        if (BlockNumber[i, 2] == BlockNumber[i, 3])
                        {
                            MoveAnimation(new Point(i, 1), new Point(i, 0), 2, false);
                            MoveAnimation(new Point(i, 2), new Point(i, 1), 2, true);
                            MoveAnimation(new Point(i, 3), new Point(i, 1), 2, false);

                        }
                        else
                        {
                            MoveAnimation(new Point(i, 1), new Point(i, 0), 2, false);
                            MoveAnimation(new Point(i, 2), new Point(i, 1), 2, true);
                            MoveAnimation(new Point(i, 3), new Point(i, 2), 2, true);
                        }
                    }
                    else
                    {
                        if (BlockNumber[i, 1] == BlockNumber[i, 2])
                        {
                            MoveAnimation(new Point(i, 2), new Point(i, 1), 2, false);
                            MoveAnimation(new Point(i, 3), new Point(i, 2), 2, true);
                        }
                        else
                        {
                            if (BlockNumber[i, 2] == BlockNumber[i, 3])
                            {
                                MoveAnimation(new Point(i, 3), new Point(i, 2), 2, false);
                            }
                        }
                    }
                }
            }
        }
        //向右運動
        private void KeyRight()
        {
            int emptyNumber = 0;
            for (int i = 0; i < 4; i++)
            {
                emptyNumber = CountEmptyNumber(i, 0);
                if (emptyNumber == 4)
                    continue;
                else if (emptyNumber == 3)
                {
                    if (BlockNumber[i, 3] == BlankState)
                    {
                        int index = FindWhichBlockNotEmpty(i, 0);
                        MoveAnimation(new Point(i, index), new Point(i, 3), 3, true);
                    }
                }
                else if (emptyNumber == 2)
                {
                    Point p = FindWhichTwoBlockEmpty(i, 0);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(p.X);
                    nums.Remove(p.Y);
                    int a = nums[0];
                    int b = nums[1];
                    if (BlockNumber[i, a] == BlockNumber[i, b])
                    {
                        MoveAnimation(new Point(i, b), new Point(i, 3), 3, true);
                        MoveAnimation(new Point(i, a), new Point(i, 3), 3, false);
                    }
                    else
                    {
                        MoveAnimation(new Point(i, b), new Point(i, 3), 3, true);
                        MoveAnimation(new Point(i, a), new Point(i, 2), 3, true);
                    }
                }
                else if (emptyNumber == 1)
                {
                    int index = FindWhichBlockEmpty(i, 0);
                    List<int> nums = new List<int>() { 0, 1, 2, 3 };
                    nums.Remove(index);
                    int a = nums[0];
                    int b = nums[1];
                    int c = nums[2];
                    if (BlockNumber[i, b] == BlockNumber[i, c])
                    {
                        MoveAnimation(new Point(i, c), new Point(i, 3), 3, true);
                        MoveAnimation(new Point(i, b), new Point(i, 3), 3, false);
                        MoveAnimation(new Point(i, a), new Point(i, 2), 3, true);
                    }
                    else
                    {
                        if (BlockNumber[i, a] == BlockNumber[i, b])
                        {
                            MoveAnimation(new Point(i, c), new Point(i, 3), 3, true);
                            MoveAnimation(new Point(i, b), new Point(i, 2), 3, true);
                            MoveAnimation(new Point(i, a), new Point(i, 2), 3, false);
                        }
                        else
                        {
                            MoveAnimation(new Point(i, c), new Point(i, 3), 3, true);
                            MoveAnimation(new Point(i, b), new Point(i, 2), 3, true);
                            MoveAnimation(new Point(i, a), new Point(i, 1), 3, true);
                        }
                    }
                }
                else
                {
                    if (BlockNumber[i, 2] == BlockNumber[i, 3])
                    {
                        if (BlockNumber[i, 0] == BlockNumber[i, 1])
                        {
                            MoveAnimation(new Point(i, 2), new Point(i, 3), 3, false);
                            MoveAnimation(new Point(i, 1), new Point(i, 2), 3, true);
                            MoveAnimation(new Point(i, 0), new Point(i, 2), 3, false);

                        }
                        else
                        {
                            MoveAnimation(new Point(i, 2), new Point(i, 3), 3, false);
                            MoveAnimation(new Point(i, 1), new Point(i, 2), 3, true);
                            MoveAnimation(new Point(i, 0), new Point(i, 1), 3, true);
                        }
                    }
                    else
                    {
                        if (BlockNumber[i, 1] == BlockNumber[i, 2])
                        {
                            MoveAnimation(new Point(i, 1), new Point(i, 2), 3, false);
                            MoveAnimation(new Point(i, 0), new Point(i, 1), 3, true);
                        }
                        else
                        {
                            if (BlockNumber[i, 0] == BlockNumber[i, 1])
                            {
                                MoveAnimation(new Point(i, 0), new Point(i, 1), 3, false);
                            }
                        }
                    }
                }
            }
        }


        //計算空的格數 n為行或者列的標誌 direction為0表示行的空格數 1表示列的空格數
        private int CountEmptyNumber(int n ,int direction)
        {
            int emptyNumber = 0;
            if (direction==0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[n, i] == BlankState)
                    {
                        emptyNumber++;

                    }
                   
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[i, n] == BlankState)
                    {
                        emptyNumber++;

                    }
                }
            }
            return emptyNumber;
        }

        //計算哪些格子不是空的 0為行 1為列
        private int FindWhichBlockNotEmpty(int n,int direction)
        {
            int index = 0;
            if (direction==0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[n,i]!=BlankState)
                    {
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[i,n]!=BlankState)
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        //找到某一行或某一列兩個為空的方塊，one為小的數字，two為大的數字
        private Point FindWhichTwoBlockEmpty(int n ,int direction)
        {
            int one = 0;   //第一個方塊  
            int two = 0;   //第二個方塊
            int number = 0;
            if (direction==0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[n,i]==BlankState)
                    {
                        if (number==0)
                        {
                            one = i;
                            number++;
                        }
                        else if (number==1)
                        {
                            two = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[i, n] == BlankState)
                    {
                        if (number == 0)
                        {
                            one = i;
                            number++;
                        }
                        else if (number == 1)
                        {
                            two = i;
                            break;
                        }
                    }
                }
            }
            return new Point(one, two);
        }

        //找到某一行或者某一列一個為空的方塊
        private int FindWhichBlockEmpty(int n, int direction)
        {
            int index = 0;
            if (direction == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[n, i] == BlankState)
                    {
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BlockNumber[i, n] == BlankState)
                    {
                        index = i;
                        break;
                    }
                }
            }
            return index;
        }

        //隨機產生圖片
        private void CreatImage()
        {
            int sum = 0;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (BlockNumber[x,y]==BlankState)
                    {
                        BlankPoint[sum] = new Point(x, y);
                        sum++;
                    }
                }
            }
            Random random = new Random();
            int index = random.Next(0, sum);
            int number = random.Next(0, 2);
            if (number==0)
            {
                BlockNumber[BlankPoint[index].X, BlankPoint[index].Y] = 2;
                GamePictureArray[BlankPoint[index].X, BlankPoint[index].Y].Image = ImageList(2);
            }
            else if (number == 1)
            {
                BlockNumber[BlankPoint[index].X, BlankPoint[index].Y] = 4;
                GamePictureArray[BlankPoint[index].X, BlankPoint[index].Y].Image = ImageList(4);
            }
            GamePictureArray[BlankPoint[index].X, BlankPoint[index].Y].Refresh();
        }


        private void IsGameEnd()
        {
            bool requirement_one = true; //判断游戏是否结束,游戏结束需要两个条件
            bool requirement_two = true;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (BlockNumber[i, j] == BlankState)
                    {
                        requirement_one = false;
                    }
                }
            }
            if (requirement_one == true)
            {
                for (int i = 0; i < 4; i++) //左右方向的判断
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (BlockNumber[i, j] == BlockNumber[i, j + 1])
                        {
                            requirement_two = false;
                        }
                    }
                }
                for (int j = 0; j < 4; j++)//上下方向的判断
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (BlockNumber[i, j] == BlockNumber[i + 1, j])
                        {
                            requirement_two = false;
                        }
                    }
                }
                if (requirement_two)
                {
                    MessageBox.Show("無法移動", "結束遊戲", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    CreatImage();
                    CreatImage();
                    
                }
            }
        }
        private void Clear()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    BlockNumber[i, j] = BlankState;
                    GamePictureArray[i, j].Image = null;
                }
            }
        }


        private void TimerWatch_Tick(object sender, EventArgs e)
        {
            bool isFinish = true;
            for (int i = 0; i < 16; i++)
            {
                if (timer[i] != null)
                {
                    isFinish = false;
                }
            }
            if (isFinish)
            {
                CreatImage();
                KeyboradEnable = true;
                TimerWatch.Stop();
            }
        }
    }
}
                            
        





       

    


